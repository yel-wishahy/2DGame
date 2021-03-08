using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * A class to manage the Inventory UI system, should be attacked to an inventory panel UI object under a canvas
 *
 * Pulls data from player inventory and update the UI accordingly, also remembers positions of items in slots,
 * and handles sorting stacks with some help from the inventory class helper functions
 *
 * Author: Yousif El-Wishahy (GH: yel-wishahy)
 * Date: 28/02/2021
 */
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private float openScale = 2.5f;

    private List<ItemSlot> itemSlots;
    private bool isOpen = false;
    private RectTransform _rectTransform;
    private Vector2 normalSize;
    private Vector2 openSize;
    private float normalY;
    private float openY;

    // Start is called before the first frame update
    void Awake()
    {
        InitItemSlots();
        _rectTransform = GetComponent<RectTransform>();
        normalSize = _rectTransform.sizeDelta;
        normalY = _rectTransform.position.y;
        openY = _rectTransform.position.y - normalSize.y * openScale;
        openSize = new Vector2(normalSize.x, normalSize.y * openScale);
    }
    
    void InitItemSlots()
    {
        itemSlots = new List<ItemSlot>();
        for (int i = 0; i < player.inventoryCapacity; i++)
        {
            GameObject child = Instantiate(itemSlotPrefab, transform.position, transform.rotation);
            child.GetComponentInChildren<Image>().name = "ItemSlot" + i;
            itemSlots.Add(child.GetComponent<ItemSlot>());
        }

        ResetSlots();
    }

    void ResetSlots()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].itemSlotID = i;
            itemSlots[i].transform.SetParent(transform);
            itemSlots[i].empty = true;
            itemSlots[i].player = player;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.inventory.Empty)
            UpdateUISlots(player.inventory.InventoryLog, UnifiedStorage.StackLimitLog);
        else 
            ResetSlots();

        if (Input.GetKeyDown(player.settings.GetSetting("Open Inventory")))
            isOpen = !isOpen;

        if (isOpen)
        {
            _rectTransform.sizeDelta = openSize;
            _rectTransform.position = new Vector3(_rectTransform.position.x, openY);
            OpenInventoryUI();
        }
        else
        {
            _rectTransform.sizeDelta = normalSize;
            _rectTransform.position = new Vector3(_rectTransform.position.x, normalY);
            CloseInventoryUI();
        }
    }
    
    private void OpenInventoryUI()
    {
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.itemSlotID > 5)
            {
                itemSlot.Open();
            }
        }
    }
    
    private void CloseInventoryUI()
    {
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.itemSlotID > 5)
            {
                itemSlot.Close();
            }
        }
    }
    
    /**
     * Ignores previous item slot configuration and re-organizes it based on the
     * order of itemNames in the inventory Log obtained from player
     */
    private void CleanOrganizeUISlots(List<ItemSlot> itemSlots)
    {
        int slotNum = 0;
        foreach (int itemID in player.inventory.InventoryLog.Keys)
        {
            int numSlots = player.inventory.CountStackItem(itemID);
            int lastSlot = player.inventory.CountIncompleteStack(itemID);
            int stackLimit = UnifiedStorage.StackLimitLog[itemID];

            for (int i = 0; i < numSlots; i++, slotNum++)
            {
                ItemSlot itemSlot = itemSlots[slotNum];

                itemSlot.empty = false;

                itemSlot.quantity = stackLimit;
                if (i == numSlots - 1 && lastSlot > 0)
                    itemSlot.quantity = lastSlot;

                itemSlot.itemID = itemID;
                itemSlot.itemImage.sprite =
                    player.inventory.GetAny(itemID).GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    /**
     * Updates the UI slots keeping previous configuration in mind, as well as any changes
     * made by drag and drop!
     */
    private void UpdateUISlots(Dictionary<int, int> invLog, Dictionary<int, int> stackLimLog)
    {
        Dictionary<int, int> itemSlotLogs = new Dictionary<int, int>();
        Dictionary<int, int> changes = new Dictionary<int, int>();

        //populate itemslot log dictionary
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (!itemSlot.empty)
            {
                if (itemSlotLogs.ContainsKey(itemSlot.itemID))
                    itemSlotLogs[itemSlot.itemID] += itemSlot.quantity;
                else
                    itemSlotLogs.Add(itemSlot.itemID, itemSlot.quantity);
            }
        }

        //populate changes dictionary with any changes needed to be made to align with the players inventory logs
        //changes are in the form of item name, (int) change
        //the int is negative if the change is to remove items
        //positive is vice verse
        foreach (int itemID in invLog.Keys)
        {
            if (!itemSlotLogs.ContainsKey(itemID))
                changes.Add(itemID, invLog[itemID]);
            else if (itemSlotLogs[itemID] != invLog[itemID])
                changes.Add(itemID, invLog[itemID] - itemSlotLogs[itemID] );
        }

        //calls another helper function to update the UI based on changes calculated above
        UpdateUISlotsFromChanges(changes, stackLimLog);
    }

    /*
     * Function that takes a dictionary of changes to make and a stack limit log for each item
     * and updates the item slot based on changes
     *
     * Logical process:
     * Run through all item names in changes
     * If the change is not 0, then attempt to make it
     * Depending on the sign of changes, either add or remove item quantities
     * If adding, and all slots containing itemName are full, starting adding to new slots
     *
     *Calls helper function to calculate and apply the change amount math
     */
    private void UpdateUISlotsFromChanges(Dictionary<int, int> changes, Dictionary<int, int> stackLimLog)
    {
        foreach (int itemID in changes.Keys)
        {
            if (changes[itemID] != 0)
            {
                foreach (ItemSlot itemSlot in itemSlots)
                {
                    if(changes[itemID] == 0)
                        break;

                    if (!itemSlot.empty && itemSlot.itemID == itemID && itemSlot.quantity < stackLimLog[itemID])
                    {
                        int diff = UpdateItemSlotQuantity(itemSlot, stackLimLog, changes[itemID]);
                        changes[itemID] = diff;
                    }
                    else if (itemSlot.empty)
                    {
                        itemSlot.empty = false;
                        itemSlot.itemID = itemID;
                        itemSlot.itemImage.sprite =
                            player.inventory.GetAny(itemID).GetComponent<SpriteRenderer>().sprite;

                        int diff = UpdateItemSlotQuantity(itemSlot, stackLimLog, changes[itemID]);
                        changes[itemID] = diff;
                    }
                }
            }
        }
    }

    /**
     * Helper function to calculate and apply the change amount
     * Goal: to add changes to the quantity in itemslot within the bounds of (0 <= item quantity <= item stack limit)
     * If the above condition is met, the return value is 0
     * If the change results in not meeting the contion of (0 <= item quantity <= item stack limit), then change is
     * reduced appropraitely until it meets the condtion, the excess is returned.
     */
    private int UpdateItemSlotQuantity(ItemSlot itemSlot, Dictionary<int, int> stackLimLog , int change)
    {
        int quantityResult = itemSlot.quantity + change;
        int returnResult = 0;

        if (quantityResult < 0)
        {
            returnResult = quantityResult;
            quantityResult = 0;
        }

        if (quantityResult > stackLimLog[itemSlot.itemID])
        {
            returnResult = quantityResult - stackLimLog[itemSlot.itemID];
            quantityResult = stackLimLog[itemSlot.itemID];
        }

        itemSlot.quantity = quantityResult;
        return returnResult;
    }

}