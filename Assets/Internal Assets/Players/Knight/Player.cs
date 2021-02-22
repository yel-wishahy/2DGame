using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UEntity
{
    [SerializeField]
    public float attackRadius = 0.8f;
    public Vector3 range = new Vector3(1, 0.2f, 0);
    public Transform groundCheck, attack;
    public LayerMask groundLayer, enemyLayer;
    public Controller userController;
    public GameObject projectilePrefab;
    
    [HideInInspector] public List<StorableItem> inventory;
    [HideInInspector] private int coalsCollected;

    private void Awake()
    {
         userController = new playerController(this);
         coalsCollected = 0;
    }
    

    //override controller property with player controller class

    public override Controller UserController => userController;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(attack.position, attackRadius);
    }

    public void pickupItem(Item item)
    {
        inventory.Add(new StorableItem(item, item.name, 1));
    }

    public int getItemQuantity(string name)
    {
        foreach (StorableItem item in inventory)
        {
            if (item.Name == name)
            {
                return item.Quantity;
            }
        }

        return 0;
    }




}
