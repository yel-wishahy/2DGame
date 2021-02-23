using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public int itemSlotID;
    [SerializeField] public StorableItem item;
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
            GetComponent<Image>().sprite = item.Item.GetComponent<SpriteRenderer>().sprite;
            GetComponent<Image>().enabled = true;
        }
        else
        {
            //GetComponent<Image>().enabled = false;
        }
    }
}
