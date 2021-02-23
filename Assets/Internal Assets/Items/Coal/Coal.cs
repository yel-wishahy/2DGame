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
            if (collector.pickupItem(this))
            {
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
                enabled = false;

            }
        }
        
    }
}
