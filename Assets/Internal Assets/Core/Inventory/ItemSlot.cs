using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public int itemSlotID;
    [SerializeField] public StorableItem item;
    [SerializeField] public Image itemImage;
    [SerializeField] private Text quantityDisplay;
    [SerializeField] private Button trashButton;
    [HideInInspector] public bool empty = false;
    [HideInInspector] public Player player;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!empty)
        {
            itemImage.sprite = item.Item.GetComponent<SpriteRenderer>().sprite;
            quantityDisplay.text = item.Quantity.ToString();
            quantityDisplay.enabled = true;
            itemImage.enabled = true;
            trashButton.enabled = true;
            trashButton.image.enabled = true;

            if (item.Quantity < 1)
                RemoveItem();
        }
        else
        {
            trashButton.enabled = false;
            trashButton.image.enabled = false;
            quantityDisplay.enabled = false;
            itemImage.enabled = false;

        }
    }

    private void RemoveItem()
    {
        trashButton.enabled = false;
        trashButton.image.enabled = false;
        quantityDisplay.enabled = false;
        itemImage.enabled = false;
        empty = true;
        item = null;
        player.RemoveItem(item);
    }

    public void OnClickUse()
    {
        if (!empty&& item.Quantity > 0)
        {
            if(item.Item.Use(player))
                item.Quantity -= 1;
        }
    }
    
    public void OnClickTrash()
    {
        if (!empty && item.Quantity > 0)
        {
            item.Quantity -= 1;
            
        }
    }
    
    
}