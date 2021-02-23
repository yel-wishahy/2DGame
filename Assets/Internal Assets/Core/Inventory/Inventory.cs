using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

//manages inventory system UI sorting, updates from player, has itemslot children that display each item
public class Inventory : MonoBehaviour
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
            child.GetComponentInChildren<Image>().name = "ItemImage" + i;
            itemSlots.Add(child.GetComponent<ItemSlot>());
        }

        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].itemSlotID = i;
            itemSlots[i].transform.parent = this.transform;
            itemSlots[i].empty = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < player.inventory.Count; i++)
        {
            Debug.Log("Item name to be added to inventory slot " + i + " : " + player.inventory[i].Name);
            itemSlots[i].item = player.inventory[i];
            itemSlots[i].empty = false;
        }
    }
}
