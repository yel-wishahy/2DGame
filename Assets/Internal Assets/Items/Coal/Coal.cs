using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Coal : Item
{
    [SerializeField] private Collider2D worldCollider;
    
    private void  OnTriggerEnter2D(Collider2D collision)
    {
        PickupSelfItem(collision);
    }
    
    private void OnCollisionEnter2D(Collision2D object2D)
    {
        IgnoreCollisions(worldCollider, object2D.gameObject.GetComponent<Collider2D>());
    }
}
