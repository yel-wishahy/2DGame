using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int health = 1;
    public void GotHit (int damage){
        health-=damage;

        if (health<=0)
        {
            Die();
        }
    }

    void Die(){
        Destroy (gameObject);
    }
}
