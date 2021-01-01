using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20f;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity= transform.right*speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo){
        // when bullet hits something
        TakeDamage enemy=hitInfo.GetComponent<TakeDamage>();
        if (enemy!=null){
            enemy.GotHit(1);
             Destroy(gameObject); 
             //destroys bullet
        }
        
    }

}
