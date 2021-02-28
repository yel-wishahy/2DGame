using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

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
            child.GetComponentInChildren<Image>().name = "ItemImage" + i;
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
            ResetSlots();
            player.inventory.UpdateUISlots(itemSlots);
        }
    }

}