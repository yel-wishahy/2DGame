using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public int itemSlotID;
    [SerializeField] public StorableItem item;
    [SerializeField] public Image itemImage;
    [SerializeField] public Text quantityDisplay;
    [HideInInspector] public bool empty = false;

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
        }
        else
        {
            itemImage.enabled = false;
            quantityDisplay.enabled = false;
        }
    }
}
