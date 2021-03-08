using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * An inventory class for the player
 * Internal representation:
 *      The inventory is a list of items (List<Item>)
 *
 * This class has helper functions that calculate stacks,
 * and quantaties based on raw inventory. This class also
 * sorts the UI into stacks.
 *
 * Author: Yousif El-Wishahy (GH: yel-wishahy)
 * Date: (26/02/2021)
 */
public class Inventory
{
    //inventory
    private List<Item> inventory;

    //helper logs that give details about items in inventory
    private Dictionary<int, int> invLog;
    
    //capacity
    private int capacity;


    //constructor
    public Inventory(int capacity)
    {
        this.capacity = capacity;
        inventory = new List<Item>();
        invLog = new Dictionary<int, int>();
    }

    /**
     * Add an item to the inventory, and updated logs
     *
     * 
     * Returns: true if item added
     * Returns: false if there is no room for item in inventory
     * (calculated based on slot capacity of player and item stack size capacities)
     */
    public bool AddItem(Item item)
    {
        if (CheckSpace(item.ID))
        {
            inventory.Add(item);
            inventory = inventory.OrderBy(go => go.name).ToList();
            UpdateLogs();
            return true;
        }

        return false;
    }

    /**
     * Remove an item for the inventory, and update logs
     *
     * 
     * Returns: true if item removed
     * Returns: false if item did not exist in inventory
     */
    public bool RemoveItem(Item item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            inventory = inventory.OrderBy(go => go.name).ToList();
            UpdateLogsRemove(item.ID);
            return true;
        }

        return false;
    }

    /**
     * Updates the logs from inventory list that store item quanities and stack limits
     * for easy access
     */
    private void UpdateLogs()
    {
        invLog = new Dictionary<int, int>();
        foreach (Item item in inventory)
        {
            if (invLog.Keys.Contains(item.ID))
                invLog[item.ID] += 1;
            else
                invLog.Add(item.ID, 1);
        }
    }

    /**
     * Update logs to remove an item from invLog
     *
     * Decrements quantity value is quantity > 1,
     * removes from dictionary if quantity = 1
     */
    private void UpdateLogsRemove(int itemID)
    {
        if (invLog[itemID] == 1)
            invLog.Remove(itemID);
        else
            invLog[itemID] -= 1;
    }


    /**
     * Returns the quantity of an item in inventory based on item name
     * Returns 0 if none found
     */
    public int CountQuantityItem(int itemID)
    {
        if (invLog.Keys.Contains(itemID))
            return invLog[itemID];
        else
            return 0;
    }

    //returns first instance of item found in inventory,
    //or returns null if no such item is stored or name is wrong
    public Item GetAny(string itemName)
    {
        foreach (Item item in inventory)
        {
            if (item.name == itemName)
                return item;
        }

        return null;
    }
    
    //returns first instance of item found in inventor based on ID,
    //or returns null if no such item is stored or ID is wrong
    public Item GetAny(int itemID)
    {
        foreach (Item item in inventory)
        {
            if (item.ID == itemID)
                return item;
        }

        return null;
    }
    
    //Gets an int number of items from inventory based on item name and returns them in a list
    //Can return less than specified number or none if none in inventory
    public List<Item> GetMultiple(string itemName, int quantityRequired)
    {
        List<Item> items = new List<Item>();
        foreach (Item item in inventory)
        {
            if (item.name == itemName && items.Count < quantityRequired)
                items.Add(item);
        }

        return items;
    }

    //returns first instance of item found in inventory AND REMOVES IT IN THE PROCESS,
    //or returns null if no such item is stored or name is wrong
    public Item GetAnyAndRemove(string itemName)
    {
        foreach (Item item in inventory)
        {
            if (item.name == itemName)
            {
                if (RemoveItem(item))
                    return item;
            }
        }

        return null;
    }
    
    //returns first instance of item found based on ID in inventory AND REMOVES IT IN THE PROCESS,
    //or returns null if no such item is stored or ID is wrong
    public Item GetAnyAndRemove(int itemID)
    {
        foreach (Item item in inventory)
        {
            if (item.ID == itemID)
            {
                if (RemoveItem(item))
                    return item;
            }
        }

        return null;
    }
    
    //Gets an int number of items from inventory based on item name and returns them in a list
    //NOTE: REMOVES ITEM FROM INVENTORY IN THE PROCESS
    //Can return less than specified number or none if none in inventory
    public List<Item> GetMultipleAndRemove(string itemName, int quantityRequired)
    {
        List<Item> items = new List<Item>();
        List<Item> inventoryCopy = new List<Item>(inventory);
        
        foreach (Item item in inventoryCopy)
        {
            if (item.name == itemName && items.Count < quantityRequired)
            {
                if (RemoveItem(item))
                    items.Add(item);
            }
        }

        return items;
    }
    
    //Gets an int number of items from inventory based on item ID and returns them in a list
    //NOTE: REMOVES ITEM FROM INVENTORY IN THE PROCESS
    //Can return less than specified number or none if none in inventory
    public List<Item> GetMultipleAndRemove(int itemID, int quantityRequired)
    {
        List<Item> items = new List<Item>();
        List<Item> inventoryCopy = new List<Item>(inventory);
        
        foreach (Item item in inventoryCopy)
        {
            if (item.ID == itemID && items.Count < quantityRequired)
            {
                if (RemoveItem(item))
                    items.Add(item);
            }
        }

        return items;
    }

    /**
     * Counts how many stacks of an item there are in the inventory
     */
    public int CountStackItem(int itemID)
    {
        int numStack = 0;

        if (invLog.Keys.Contains(itemID) && UnifiedStorage.StackLimitLog.Keys.Contains(itemID))
        {
            numStack = invLog[itemID] / UnifiedStorage.StackLimitLog[itemID];
            int remainder = invLog[itemID] - numStack * UnifiedStorage.StackLimitLog[itemID];

            if (remainder > 0)
                numStack += 1;
        }

        return numStack;
    }

    /**
     * Counts how many stacks of all items there are in inventory
     */
    public int CountStackAll()
    {
        int stackCount = 0;

        foreach (int itemID in invLog.Keys)
        {
            stackCount += CountStackItem(itemID);
        }

        return stackCount;
    }

    /**
     * Checks if inventory has enough space to add item of name : itemName
     */
    public bool CheckSpace(int itemID)
    {
        if (invLog.Keys.Contains(itemID) && UnifiedStorage.StackLimitLog.Keys.Contains(itemID))
        {
            if (invLog[itemID] < UnifiedStorage.StackLimitLog[itemID])
                return true;

            int numStack = invLog[itemID] / UnifiedStorage.StackLimitLog[itemID];
            int remainder = invLog[itemID] - numStack * UnifiedStorage.StackLimitLog[itemID];

            if (remainder > 0)
                return true;
        }

        if (CountStackAll() < capacity)
            return true;

        return false;
    }

    /**
     * Counts incomplete stacks of an item
     * Returns 0 if there are no items or stacks are all complete
     */
    public int CountIncompleteStack(int itemID)
    {
        int num = 0;

        if (invLog.Keys.Contains(itemID) && UnifiedStorage.StackLimitLog.Keys.Contains(itemID))
        {
            int numStack = invLog[itemID] / UnifiedStorage.StackLimitLog[itemID];

            num = invLog[itemID] - numStack * UnifiedStorage.StackLimitLog[itemID];
        }

        return num;
    }

    //a copy instance of the log accessible to other classes
    public Dictionary<int, int> InventoryLog
    {
        get => new Dictionary<int, int>(invLog);
    }

    //a copy instance of the full inventory accessible to other classes
    //unsure if it also creates new instance copies of the items...?
    public List<Item> InventoryList
    {
        get => new List<Item>(inventory);
    }

    /**
     * Status of inventory
     */
    public bool Empty
    {
        get => inventory.Count < 1;
    }
}