using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    private List<Item> inventory;
    private Dictionary<string, int> invLog;
    private Dictionary<string, int> stackLimLog;
    private Player player;

    public Inventory(Player parent)
    {
        player = parent;
        inventory = new List<Item>();
        invLog = new Dictionary<string, int>();
        stackLimLog = new Dictionary<string, int>();
    }

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

    private void UpdateLogsRemove(string itemName)
    {
        if (invLog[itemName] == 1)
            invLog.Remove(itemName);
        else
            invLog[itemName] -= 1;
    }
    
    

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

    public int CountStackAll()
    {
        int stackCount = 0;
        
        foreach (string itemKey in invLog.Keys)
        {
            stackCount += CountStackItem(itemKey);
        }

        return stackCount;
    }

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

    // public void UpdateUISlots(List<ItemSlot> itemSlots)
    // {
    //     int i = 0;
    //     
    //     foreach (Item item in inventory)
    //     {
    //         if (i < itemSlots.Count)
    //         {
    //             ItemSlot itemSlot = itemSlots[i];
    //
    //             if (itemSlot.empty)
    //             {
    //                 itemSlot.empty = false;
    //                 itemSlot.quantity = 1;
    //                 itemSlot.itemName = item.name;
    //                 itemSlot.itemImage.sprite =
    //                     player.inventory.GetAny(item.name).GetComponent<SpriteRenderer>().sprite;
    //                 
    //             } else if (itemSlot.name == item.name)
    //             {
    //                 itemSlot.quantity += 1;
    //             } else
    //                 i++;
    //         }
    //         
    //     }
    // }
    
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
    
    public bool Empty
    {
        get => inventory.Count < 1;
    }
    
    
}