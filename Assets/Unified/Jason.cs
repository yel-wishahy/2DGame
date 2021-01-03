using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jason : UEntity
{
    [HideInInspector]
    public float alternativeX;
    [HideInInspector]
    public float alternativeY;
    [HideInInspector]
    public bool AttackMode;
    [HideInInspector]
    public bool Attacking;
    [HideInInspector]
    public bool AddDamage;
    [HideInInspector]
    public bool Hurt;
    public int HurtPhase;

    private Controller userController;

    private void Awake()
    {
        userController = new UJasonController(this);
    }

    public override Controller UserController => userController);
    

}
