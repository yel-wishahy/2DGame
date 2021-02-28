using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Description: 
    An Entity class that is the base class for all entities.
    The defined behaviour for an entity is as follows:
    An entity has health, speed, jumoForce, and AttackDamage

How to Use:
    To implement a new entity simply extend this class.
    All entities have the potential for a userController
    and an alternative AI controller. Defined by the 
    Alternative Control boolean.

Notes:
    The Update() behaviour is defined by the controller class.
    The start behaviour initializes the controller class

Authors: Yousif El-Wishahy and Kenny Zhang
Date: (2020/12/31)
*/
public class UEntity : MonoBehaviour
{
    //abstract properties that all entities have (override when extending this abstract class)
    [SerializeField]
    private float health, speed, jumpForce, attackDamage;
    [SerializeField]
    public bool AlterativeControl;

    //controllers that are specieifed as properties.
    public virtual Controller AltController { get; }
    public virtual Controller UserController { get; }

    // Update is called once per frame
    void Update()
    {
        if (AlterativeControl)
        {
            AltController.Update();
        }
        else
        {
            UserController.Update();
        }

    }

    void OnCollisionStay2D(Collision2D object2D)
    {
        if (AlterativeControl)
        {
            AltController.OnCollisionStay2D(object2D);
        }
        else
        {
            UserController.OnCollisionStay2D(object2D);
        }
    }

    void OnCollisionEnter2D(Collision2D object2D)
    {
        if (AlterativeControl)
        {
            AltController.OnCollisionEnter2D(object2D);
        }
        else
        {
            UserController.OnCollisionEnter2D(object2D);
        }
    }

    void OnCollisionExit2D(Collision2D object2D)
    {
        if (AlterativeControl)
        {
            AltController.OnCollisionExit2D(object2D);
        }
        else
        {
            UserController.OnCollisionExit2D(object2D);
        }
    }

    void OnTriggerStay2D(Collider2D object2D)
    {
        if (AlterativeControl)
        {
            AltController.OnTriggerStay2D(object2D);
        }
        else
        {
            UserController.OnTriggerStay2D(object2D);
        }
    }

    void OnTriggerEnter2D(Collider2D object2D)
    {
        if (AlterativeControl)
        {
            AltController.OnTriggerEnter2D(object2D);
        }
        else
        {
            UserController.OnTriggerEnter2D(object2D);
        }
    }

    void OnTriggerExit2D(Collider2D object2D)
    {
        if (AlterativeControl)
        {
            AltController.OnTriggerExit2D(object2D);
        }
        else
        {
            UserController.OnTriggerExit2D(object2D);
        }
    }

    //Entity takes damage and health is reduced by dmg.
    public void takeDamage(float dmg)
    {
        health -= dmg;

        if (AlterativeControl)
        {
            AltController.Hurt(true);
        }
        else
        {
            UserController.Hurt(true);
        }
    }

    //Returns life state of entity
    public bool isAlive()
    {
        return (health > 0);
    }

    public bool isHurt()
    {
        if (AlterativeControl)
        {
            return AltController.Hurt(false);
        }
        else
        {
            return UserController.Hurt(false);
        }
    }

    //increments entity health by 1
    public void incrementHealth()
    {
        health++;
    }

    //decrements entity health by 1
    public void decrementHealth()
    {
        health--;
    }

    //Entity is healed and +hlth health is gained
    public void gainHealth(float hlth)
    {
        health += hlth;
    }

    public float AttackDamage
    {
        get => attackDamage;
    }

    public float Speed
    {
        get => speed;
    }

    public float JumpForce
    {
        get => jumpForce;
    }

    public float Health
    {
        get => health;
    }
    public void setHealth(float hlth)
    {
        health = hlth;
    }

    public float time
    {
        get => Time.time;
    }

    public void Die()
    {
        enabled = false;
        health = 0;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<Canvas>().enabled = false;
        Destroy(this);
    }
}

