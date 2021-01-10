using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Player collector = collision.GetComponent<Player>();
            collector.collectCoal();
            Destroy(this);
            GetComponent<SpriteRenderer>().enabled = false;
        }
        
    }
}
