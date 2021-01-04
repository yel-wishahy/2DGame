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
//
//Any behaviour that is deemed common to all entities can be 
//added to this class. Ensure that this variable will be intilizaed properly
//either as a property for the child class to use or a SerializedField
//to change in unity inspector.
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

    //Returns attack damage of entity
    public float getAttackDamage()
    {
        return AttackDamage;
    }

    //Returns speed of entity
    public float getSpeed()
    {
        return Speed;
    }

    //Returns jumpForce of entity
    public float getJumpForce()
    {
        return JumpForce;
    }

    //Return current health of entity
    public float getHealth()
    {
        return Health;
    }

    //Sets health of entity to hlth
    //PreCondition: hlth >= 0;
    public void setHealth(float hlth)
    {
        Health = hlth;
    }
    
    //Returns current game time of entity's runtime game instance
    public float getTime()
    {
        return Time.time;
    }
}

