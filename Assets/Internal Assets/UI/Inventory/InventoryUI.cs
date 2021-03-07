using System;
using System.Collections;
using System.Collections.Generic;
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

    private List<ItemSlot> itemSlots;
    
    // Start is called before the first frame update
    void Awake()
    {
        InitItemSlots();
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
        {
            UpdateUISlots(player.inventory.InventoryLog, UnifiedStorage.StackLimitLog);
        }
        else
        {
            ResetSlots();
        }
    }
    
    /**
     * Ignores previous item slot configuration and re-organizes it based on the
     * order of itemNames in the inventory Log obtained from player
     */
    private void CleanOrganizeUISlots(List<ItemSlot> itemSlots)
    {
        int slotNum = 0;
        foreach (string itemName in player.inventory.InventoryLog.Keys)
        {
            int numSlots = player.inventory.CountStackItem(itemName);
            int lastSlot = player.inventory.CountIncompleteStack(itemName);
            int stackLimit = UnifiedStorage.StackLimitLog[itemName];

            for (int i = 0; i < numSlots; i++, slotNum++)
            {
                ItemSlot itemSlot = itemSlots[slotNum];

                itemSlot.empty = false;

                itemSlot.quantity = stackLimit;
                if (i == numSlots - 1 && lastSlot > 0)
                    itemSlot.quantity = lastSlot;

                itemSlot.itemName = itemName;
                itemSlot.itemImage.sprite =
                    player.inventory.GetAny(itemName).GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    /**
     * Updates the UI slots keeping previous configuration in mind, as well as any changes
     * made by drag and drop!
     */
    private void UpdateUISlots(Dictionary<string, int> invLog, Dictionary<string, int> stackLimLog)
    {
        Dictionary<string, int> itemSlotLogs = new Dictionary<string, int>();
        Dictionary<string, int> changes = new Dictionary<string, int>();

        //populate itemslot log dictionary
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (!itemSlot.empty)
            {
                if (itemSlotLogs.ContainsKey(itemSlot.itemName))
                    itemSlotLogs[itemSlot.itemName] += itemSlot.quantity;
                else
                    itemSlotLogs.Add(itemSlot.itemName, itemSlot.quantity);
            }
        }

        //populate changes dictionary with any changes needed to be made to align with the players inventory logs
        //changes are in the form of item name, (int) change
        //the int is negative if the change is to remove items
        //positive is vice verse
        foreach (string itemName in invLog.Keys)
        {
            if (!itemSlotLogs.ContainsKey(itemName))
                changes.Add(itemName, invLog[itemName]);
            else if (itemSlotLogs[itemName] != invLog[itemName])
                changes.Add(itemName, invLog[itemName] - itemSlotLogs[itemName] );
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
    private void UpdateUISlotsFromChanges(Dictionary<string, int> changes, Dictionary<string, int> stackLimLog)
    {
        foreach (string itemName in changes.Keys)
        {
            if (changes[itemName] != 0)
            {
                foreach (ItemSlot itemSlot in itemSlots)
                {
                    if(changes[itemName] == 0)
                        break;

                    if (!itemSlot.empty && itemSlot.itemName == itemName && itemSlot.quantity < stackLimLog[itemName])
                    {
                        int diff = UpdateItemSlotQuantity(itemSlot, stackLimLog, changes[itemName]);
                        changes[itemName] = diff;
                    }
                    else if (itemSlot.empty)
                    {
                        itemSlot.empty = false;
                        itemSlot.itemName = itemName;
                        itemSlot.itemImage.sprite =
                            player.inventory.GetAny(itemName).GetComponent<SpriteRenderer>().sprite;

                        int diff = UpdateItemSlotQuantity(itemSlot, stackLimLog, changes[itemName]);
                        changes[itemName] = diff;
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
    private int UpdateItemSlotQuantity(ItemSlot itemSlot, Dictionary<string, int> stackLimLog , int change)
    {
        int quantityResult = itemSlot.quantity + change;
        int returnResult = 0;

        if (quantityResult < 0)
        {
            returnResult = quantityResult;
            quantityResult = 0;
        }

        if (quantityResult > stackLimLog[itemSlot.itemName])
        {
            returnResult = quantityResult - stackLimLog[itemSlot.itemName];
            quantityResult = stackLimLog[itemSlot.itemName];
        }

        itemSlot.quantity = quantityResult;
        return returnResult;
    }

}