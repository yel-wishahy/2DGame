using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An Entity class that is the base class for all entities.
//The defined behaviour for an entity is as follows:
//*An entity has health, speed, jumpForce, and AttackDamage
//
//To implement a new entity simply extend this class.
//All entities have the potential for a userController
//and an alternative AI controller. Defined by the 
//Alternative Control boolean.
//
//The Update() behaviour is defined by the controller class.
//
//Start behaviour is defined in the controller class's constructor
public class UEntity : MonoBehaviour
{
    //abstract properties that all entities have (override when extending this abstract class)
    [SerializeField]
    private float Health, Speed, JumpForce, AttackDamage;
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
        } else
        {
            UserController.Update();
        }

    }

    //Entity takes damage and health is reduced by dmg.
    public void takeDamage(float dmg)
    {
        Health -= dmg;
    }

    //Returns life state of entity
    public bool isAlive()
    {
        return (Health > 0);
    }

    //increments entity health by 1
    public void incrementHealth()
    {
        Health++;
    }

    //decrements entity health by 1
    public void decrementHealth()
    {
        Health--;
    }

    //Entity is healed and +hlth health is gained
    public void gainHealth(float hlth)
    {
        Health += hlth;
    }

    public float getAttackDamage()
    {
        return AttackDamage;
    }

    public float getSpeed()
    {
        return Speed;
    }

    public float getJumpForce()
    {
        return JumpForce;
    }

    public float getHealth()
    {
        return Health;
    }

    public void setHealth(float hlth)
    {
        Health = hlth;
    }

    public float getTime()
    {
        return Time.time;
    }
}

