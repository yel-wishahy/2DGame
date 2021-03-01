using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private ItemSlot startSlot;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;

        if (IsPointerOverItemSlot())
        {
            startSlot = GetItemSlotFromPointer();
            Debug.Log("over " + startSlot.name);
            
        }
        

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsPointerOverItemSlot())
        {
            ItemSlot endSlot = GetItemSlotFromPointer();

            if (startSlot != null && endSlot != null)
            {
                transform.position = startPosition;
                ItemSlot.SwapSlots(startSlot, endSlot);
            }
        }
    }
    
    ///Returns 'true' if we touched or hovering an ItemSlot
    public static bool IsPointerOverItemSlot()
    {
        return IsPointerOverItemSlot(GetEventSystemRaycastResults());
    }

    //Returns an item slot if we are hovering over or touching one
    public ItemSlot GetItemSlotFromPointer()
    {
        List<RaycastResult> eventSystemRaysastResults = GetEventSystemRaycastResults();
        for(int index = 0;  index < eventSystemRaysastResults.Count; index ++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults [index];
            if (curRaysastResult.gameObject.name.Contains("ItemSlot"))
                return curRaysastResult.gameObject.GetComponent<ItemSlot>();
        }

        return null;
    }
    
    ///Returns 'true' if we touched or hovering on Itemslot
    public static bool IsPointerOverItemSlot(List<RaycastResult> eventSystemRaysastResults )
    {
        for(int index = 0;  index < eventSystemRaysastResults.Count; index ++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults [index];
            if (curRaysastResult.gameObject.name.Contains("ItemSlot"))
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
