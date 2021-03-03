using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

/**
 * Drag and Drop handler class for UI inventory slots
 * This script should be attatched to the item image under itemSlot prefab so that only the item image is dragged
 *
 * Handles the dragging and dropping of items between and out of inventory slots using Unity Engine's
 * helpful IDragHandler, IBeginDragHandler, IEndDragHandler UI eventhandler interfaces
 *
 * Author: Yousif El-Wishahy (GH: yel-wishahy)
 * Date: 28/02/2021
 */
public class DragDropHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private ItemSlot startSlot;
    private bool splitting = false;

    //when drag begins, save starting item slot and starting position
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;

        if (IsPointerOverItemSlot())
        {
            startSlot = GetUIElementFromPointer("ItemSlot");
        }
    }

    //While dragging update the position of the image for the drag animation
    //to follow the mouse
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        if (Input.GetKeyDown(startSlot.player.settings.GetSetting("Secondary")))
            splitting = true;
        else
            splitting = false;
        
        Debug.Log("splitting is: " + splitting);
    }

    //When ending drag either drop item in a slot, or out of inventory depending
    //on location of cursor.
    //When dropping back in inventory swaps between slots happen
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPosition;
        
        if (IsPointerOverItemSlot())
        {
            ItemSlot endSlot = GetUIElementFromPointer("ItemSlot");

            if (startSlot != null && endSlot != null)
            {
                if (splitting)
                {
                    if(endSlot.empty)
                        ItemSlot.SplitSlots(startSlot, endSlot);
                }
                else
                {
                    if(startSlot.itemName == endSlot.itemName)
                        ItemSlot.SumSlots(startSlot, endSlot);
                    else 
                        ItemSlot.SwapSlots(startSlot, endSlot);
                }
            }
        }
        else if (!IsPointerOverInventory())
        {

        }
    }
    
    //Returns 'true' if we touched or hovering an ItemSlot
    public static bool IsPointerOverItemSlot()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults(), "ItemSlot");
    }
    
    //Returns 'true' if we touched or hovering the inventory panel
    public static bool IsPointerOverInventory()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults(), "Inventory");
    }

    //Returns an item slot if we are hovering over or touching one
    public ItemSlot GetUIElementFromPointer(string UIName)
    {
        List<RaycastResult> eventSystemRaysastResults = GetEventSystemRaycastResults();
        for(int index = 0;  index < eventSystemRaysastResults.Count; index ++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults [index];
            if (curRaysastResult.gameObject.name.Contains(UIName))
                return curRaysastResult.gameObject.GetComponent<ItemSlot>();
        }

        return null;
    }
    
    ///Returns 'true' if we touched or hovering on Itemslot
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults, string UIName )
    {
        for(int index = 0;  index < eventSystemRaysastResults.Count; index ++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults [index];
            if (curRaysastResult.gameObject.name.Contains(UIName))
                return true;
        }
        return false;
    }
    
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {   
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position =  Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll( eventData, raysastResults );
        return raysastResults;
    }
}
