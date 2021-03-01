using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//manages inventory system UI sorting, updates from player, has itemslot children that display each item
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
            itemSlots[i].transform.parent = transform;
            itemSlots[i].empty = true;
            itemSlots[i].player = player;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.inventory.Empty)
        {
            UpdateUISlots(player.inventory.InventoryLog, player.inventory.StackLimitLog);
        }
        else
        {
            ResetSlots();
        }
    }
    
        /**
     * Organizes the UI slots based on stacks of items
     * needs the UI slots to be passed to it as a list.
     */
    public void CleanOrganizeUISlots(List<ItemSlot> itemSlots)
    {
        int slotNum = 0;
        foreach (string itemName in player.inventory.InventoryLog.Keys)
        {
            int numSlots = player.inventory.CountStackItem(itemName);
            int lastSlot = player.inventory.CountIncompleteStack(itemName);
            int stackLimit = player.inventory.StackLimitLog[itemName];

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

    public void UpdateUISlots(Dictionary<string, int> invLog, Dictionary<string, int> stackLimLog)
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

        foreach (var itemName in invLog.Keys)
        {
            if (!itemSlotLogs.ContainsKey(itemName))
                changes.Add(itemName, invLog[itemName]);
            else if (itemSlotLogs[itemName] != invLog[itemName])
                changes.Add(itemName, invLog[itemName] - itemSlotLogs[itemName] );
        }

        UpdateUISlotsFromChanges(changes, stackLimLog);
    }

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