using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

/**
 * An item slot class that represents each individual itemslot in an inventory panel
 *
 * Author: Yousif El-Wishahy 9GH: yel-wishahy)
 * Date: 28/02/2021
 */
public class ItemSlot : MonoBehaviour
{
    public int itemSlotID;
    [SerializeField] public string itemName = "empty";
    [SerializeField] public Image itemImage;
    [SerializeField] private Text quantityDisplay;
    [SerializeField] private Button trashButton;
    [HideInInspector] public bool empty = true;
    [HideInInspector] public Player player;
    [HideInInspector] public int quantity;

    // Update is called once per frame
    void Update()
    {
        if (quantity < 1)
            empty = true;

        if (!empty)
            Fill();
        else
            Clear();
    }

    //fills the UI for the item slot by enabling everything and updating quantity
    public void Fill()
    {
        quantityDisplay.text = quantity.ToString();
        quantityDisplay.enabled = true;
        itemImage.enabled = true;
        trashButton.enabled = true;
        trashButton.image.enabled = true;
    }

    //Clears slot by clearing name, quantity, and disabling graphics
    public void Clear()
    {
        itemName = "empty";
        quantity = 0;
        trashButton.enabled = false;
        trashButton.image.enabled = false;
        quantityDisplay.enabled = false;
        itemImage.enabled = false;
    }

    //When item image is clicked, use the item by calling its use command
    public void OnClickUse()
    {
        if (!empty && quantity > 0)
        {
            Item item = player.inventory.GetAnyAndRemove(itemName);
            if (item != null && item.Use(player))
                quantity -= 1;
        }
    }

    //When the x UI button is clicked, remove item from inventory and drop it
    public void OnClickTrash()
    {
        if (!empty && quantity > 0)
        {
            Item item = player.inventory.GetAnyAndRemove(itemName);
            if (item != null && Item.DropItem(item, player.transform.position))
                quantity -= 1;
        }
    }

<<<<<<< HEAD
=======
    //When an item stack is dragged out of inventory, remove the whole stack from inventory
    //and drop the whole stack into the world
    public void OnDropTrash()
    {
        List<Item> itemsRemoved = player.inventory.GetMultipleAndRemove(itemName, quantity);

        if (Item.DropMultipleItems(itemsRemoved, player.transform.position))
            Clear();
    }

    //Swap two slots
>>>>>>> f9988ae077e260d41f2be7e74950caf2f90bbdf5
    public static void SwapSlots(ItemSlot itemSlot1, ItemSlot itemSlot2)
    {
        string itemName = itemSlot1.itemName;
        Sprite itemSprite = itemSlot1.itemImage.sprite;
        bool empty = itemSlot1.empty;
        int quantity = itemSlot1.quantity;

        itemSlot1.itemName = itemSlot2.itemName;
        itemSlot1.itemImage.sprite = itemSlot2.itemImage.sprite;
        itemSlot1.empty = itemSlot2.empty;
        itemSlot1.quantity = itemSlot2.quantity;

        itemSlot2.itemName = itemName;
        itemSlot2.itemImage.sprite = itemSprite;
        itemSlot2.empty = empty;
        itemSlot2.quantity = quantity;
    }

    //Split a slot in half, itemSlot2 has to be empty
    public static void SplitSlots(ItemSlot itemSlot1, ItemSlot itemSlot2)
    {
        Debug.Log("splitting");
        int quantity = itemSlot1.quantity / 2;
        int remainder = 0;

        if (itemSlot1.quantity % 2 != 0)
            remainder = 1;
        
        itemSlot1.quantity = quantity + remainder;

        itemSlot2.itemName = itemSlot1.itemName;
        itemSlot2.itemImage.sprite = itemSlot1.itemImage.sprite;
        itemSlot2.empty = false;
        itemSlot2.quantity = quantity;
    }

    //Add two slots, both slots have to contain the same item
    public static void SumSlots(ItemSlot itemSlot1, ItemSlot itemSlot2)
    {
        int quantity = itemSlot1.quantity + itemSlot2.quantity;
        int remainder = 0;

        if (quantity > itemSlot1.player.inventory.StackLimitLog[itemSlot1.itemName])
        {
            remainder = quantity - itemSlot1.player.inventory.StackLimitLog[itemSlot1.itemName];
            quantity = itemSlot1.player.inventory.StackLimitLog[itemSlot1.itemName];
        }
        

        itemSlot1.quantity = remainder;
        itemSlot2.quantity = quantity;
    }
}