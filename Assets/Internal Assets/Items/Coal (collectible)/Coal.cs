using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Coal : Item
{
    private void  OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("above");
        if (collision.GetComponent<Player>() != null)
        {
            Debug.Log("above2");
            Player collector = collision.GetComponent<Player>();
            collector.pickupItem(this);
            Destroy(this);
            GetComponent<SpriteRenderer>().enabled = false;
        }
        
    }
}
