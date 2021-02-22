using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Coal : Item
{
    private void  OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Player collector = collision.GetComponent<Player>();
            collector.pickupItem(this);
            Destroy(this);
            GetComponent<SpriteRenderer>().enabled = false;
        }
        
    }
}
