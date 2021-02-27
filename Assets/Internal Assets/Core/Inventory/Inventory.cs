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
    private Dictionary<string, int> invLog;
    private Dictionary<string, int> stackLimLog;
    
    //player
    private Player player;

    
    //constructor
    public Inventory(Player parent)
    {
        player = parent;
        inventory = new List<Item>();
        invLog = new Dictionary<string, int>();
        stackLimLog = new Dictionary<string, int>();
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
        if (CheckSpace(item.name))
        {
            inventory.Add(item);
            inventory = inventory.OrderBy(go=>go.name).ToList();
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
            inventory = inventory.OrderBy(go=>go.name).ToList();
            UpdateLogsRemove(item.name);
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
        invLog = new Dictionary<string, int>();
        foreach (Item item in inventory)
        {
            if (invLog.Keys.Contains(item.name))
                invLog[item.name] += 1;
            else
                invLog.Add(item.name, 1);

            if(!stackLimLog.Keys.Contains(item.name))
                stackLimLog.Add(item.name, item.stackLimit);
        }
    }

    /**
     * Update logs to remove an item from invLog
     *
     * Decrements quantity value is quantity > 1,
     * removes from dictionary if quantity = 1
     */
    private void UpdateLogsRemove(string itemName)
    {
        if (invLog[itemName] == 1)
            invLog.Remove(itemName);
        else
            invLog[itemName] -= 1;
    }
    
    

    /**
     * Returns the quantity of an item in inventory based on item name
     * Returns 0 if none found
     */
    public int CountQuantityItem(string itemName)
    {
        if (invLog.Keys.Contains(itemName))
            return invLog[itemName];
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
    
    //returns first instance of item found in inventory AND REMOVES IT IN THE PROCESS,
    //or returns null if no such item is stored or name is wrong
    public Item GetAnyAndRemove(string itemName)
    {
        foreach (Item item in inventory)
        {
            if (item.name == itemName)
            {
                if(RemoveItem(item))
                    return item;
            }
        }

        return null;
    }

    /**
     * Counts how many stacks of an item there are in the inventory
     */
    public int CountStackItem(string itemName)
    {
        int numStack = 0;
        
        if (invLog.Keys.Contains(itemName) && stackLimLog.Keys.Contains(itemName))
        {
            numStack = invLog[itemName] / stackLimLog[itemName];
            if (numStack < 1) numStack = 1;
            int remainder = invLog[itemName] % numStack;

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
        
        foreach (string itemKey in invLog.Keys)
        {
            stackCount += CountStackItem(itemKey);
        }

        return stackCount;
    }

    /**
     * Checks if inventory has enough space to add item of name : itemName
     */
    public bool CheckSpace(string itemName)
    {
        if (invLog.Keys.Contains(itemName) && stackLimLog.Keys.Contains(itemName))
        {
            if (invLog[itemName] < stackLimLog[itemName])
                return true;
            
            int numStack = invLog[itemName] / stackLimLog[itemName];
            if (numStack < 1) numStack = 1;
            int remainder = invLog[itemName] % numStack;

            if (remainder > 0)
            {
                return true;
            }
        } else if (CountStackAll() < player.inventoryCapacity)
            return true;

        return false;
    }

    /**
     * Counts incomplete stacks of an item
     * Returns 0 if there are no items or stacks are all complete
     */
    public int CountIncompleteStack(string itemName)
    {
        int num = 0;

        if (invLog.Keys.Contains(itemName) && stackLimLog.Keys.Contains(itemName))
        {
            int numStack = invLog[itemName] / stackLimLog[itemName];

            num = invLog[itemName] - numStack * stackLimLog[itemName];
        }

        return num;
    }

    /**
     * Organizes the UI slots based on stacks of items
     * needs the UI slots to be passed to it as a list.
     */
    public void UpdateUISlots(List<ItemSlot> itemSlots)
    {
        int slotNum = 0;
        foreach (string itemName in invLog.Keys)
        {
            int numSlots = CountStackItem(itemName);
            int lastSlot = CountIncompleteStack(itemName);
            int stackLimit = stackLimLog[itemName];
            
            for (int i = 0; i < numSlots; i++, slotNum++)
            {
                ItemSlot itemSlot = itemSlots[slotNum];
                
                itemSlot.empty = false;
                
                itemSlot.quantity = stackLimit;
                if (i == numSlots - 1)
                    itemSlot.quantity = lastSlot;
                
                itemSlot.itemName = itemName;
                itemSlot.itemImage.sprite =
                    player.inventory.GetAny(itemName).GetComponent<SpriteRenderer>().sprite;
                

            }

        }
    }
    
    

    //a copy instance of the log accessible to other classes
    public Dictionary<string, int> InventoryLog
    {
        get => new Dictionary<string, int>(invLog);
    }
    
    //a copy instance of the stack log accessible to other classes
    public Dictionary<string, int> StackLimitLog
    {
        get => new Dictionary<string, int>(stackLimLog);
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