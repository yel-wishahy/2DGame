using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KFC : Item
{
    public float healthBuff;
    
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
