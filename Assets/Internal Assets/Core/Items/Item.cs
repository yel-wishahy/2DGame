using UnityEngine;

/*
 * Description: An item class that defines any game object
 * that is an item. An item is defined as anything the player
 * can collect, use, move, interact with etc... (can probably
 * improve this definition later). An item should not have
 * health or take damage etc...
 * 
 * Author: Yousif El-Wishahy (gh: yel-wishahy)
 * (2021/02/19)
 */
public class Item : MonoBehaviour
{
    //variables that are common to all items go below here
    [SerializeField] public bool AlterativeControl;
    [SerializeField] public string name;
    [SerializeField] public int stackLimit;
    [SerializeField] public string itemType = "default";

    //false by default
    [HideInInspector] public bool inStorage = false;


    //controllers that are specieifed as properties.
    public virtual Controller AltController { get; }

    //All common item functions for the game engine to use

    // Update is called once per frame
    void Update()
    {
        if (AlterativeControl)
        {
            AltController.Update();
        }
    }

    void OnCollisionStay2D(Collision2D object2D)
    {
        if (AlterativeControl)
        {
            AltController.OnCollisionStay2D(object2D);
        }
    }

    void OnCollisionEnter2D(Collision2D object2D)
    {
        if (AlterativeControl)
        {
            AltController.OnCollisionEnter2D(object2D);
        }
    }

    void OnCollisionExit2D(Collision2D object2D)
    {
        if (AlterativeControl)
        {
            AltController.OnCollisionExit2D(object2D);
        }
    }

    void OnTriggerStay2D(Collider2D object2D)
    {
        if (AlterativeControl)
        {
            AltController.OnTriggerStay2D(object2D);
        }
    }

    void OnTriggerEnter2D(Collider2D object2D)
    {
        if (AlterativeControl)
        {
            AltController.OnTriggerEnter2D(object2D);
        }
    }

    void OnTriggerExit2D(Collider2D object2D)
    {
        if (AlterativeControl)
        {
            AltController.OnTriggerExit2D(object2D);
        }
    }

    public float getTime()
    {
        return Time.time;
    }

    public void Die()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        enabled = false;
        Destroy(this);
    }

    //Returns life state of entity
    public bool isAlive()
    {
        return enabled;
    }

    //this is limited to only player class for now
    public void PickupSelfItem(Collider2D entity)
    {
        if (entity.GetComponent<Player>() != null)
        {
            Player collector = entity.GetComponent<Player>();
            Debug.Log(inStorage);
            if (collector.pickupItem(this))
            {
                inStorage = true;
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
            }
        }
    }

    public void IgnoreCollisions(Collider2D selfCollider, Collider2D collider)
    {
        if (collider.gameObject.tag != "Ground")
        {
            Physics2D.IgnoreCollision(selfCollider, collider);
        }
    }
}