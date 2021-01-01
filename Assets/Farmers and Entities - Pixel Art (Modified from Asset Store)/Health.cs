using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float health;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void takeDamage(float dmg)
    {
        health -= dmg;
        anim.SetBool("Hurt", true);
    }

    public bool isAlive()
    {
        return (health > 0);
    }

    public void incrementHealth()
    {
        health++;
    }

    public void decrementHealth()
    {
        health--;
    }

    public void gainHealth(float hlth)
    {
        health += hlth;
    }

}
