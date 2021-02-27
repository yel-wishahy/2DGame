using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public int itemSlotID;
    [SerializeField] public string itemName;
    [SerializeField] public Image itemImage;
    [SerializeField] private Text quantityDisplay;
    [SerializeField] private Button trashButton;
    [HideInInspector] public bool empty = true;
    [HideInInspector] public Player player;
    [HideInInspector] public int quantity;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!empty)
        {
            quantityDisplay.text = quantity.ToString();
            quantityDisplay.enabled = true;
            itemImage.enabled = true;
            trashButton.enabled = true;
            trashButton.image.enabled = true;

            if (quantity < 1)
                empty = true;
        }
        else
        {
            trashButton.enabled = false;
            trashButton.image.enabled = false;
            quantityDisplay.enabled = false;
            itemImage.enabled = false;

        }
    }

    public void OnClickUse()
    {
        if (!empty && quantity > 0)
        {
            Item item = player.inventory.GetAnyAndRemove(itemName);
            if (item != null && item.Use(player))
                quantity -= 1;
        }
    }
    
    public void OnClickTrash()
    {
        if (!empty && quantity > 0)
        {
            Item item = player.inventory.GetAnyAndRemove(itemName);
            if (item != null && Item.DropItem(item, player.transform.position))
                quantity -= 1;
        }
    }
    
    
}