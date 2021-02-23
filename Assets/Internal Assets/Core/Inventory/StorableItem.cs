using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.AI;

//Item Wrapper to store items in an inventory
//the internal representation of an inventory is probably 'List<Item>'
public class StorableItem
{
    private int quantity;
    private string name;
    private Item item;
    

    public StorableItem(Item item, string name, int quantity)
    {
        this.item = item;
        this.name = name;
        this.quantity = quantity;
    }

    public int Quantity
    {
        get => quantity;
        set => quantity = value;
    }

    public string Name
    {
        get => name;
    }

    public Item Item
    {
        get => item;
    }
}
