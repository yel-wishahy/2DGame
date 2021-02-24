using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KFC : Item
{
    [SerializeField] private Collider2D worldCollider;
    public float healthBuff;
    
    private void  OnTriggerEnter2D(Collider2D collision)
    {
        PickupSelfItem(collision);
    }
    
    private void OnCollisionEnter2D(Collision2D object2D)
    {
        IgnoreCollisions(worldCollider, object2D.gameObject.GetComponent<Collider2D>());
    }
    
}
