using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Item
{
    //controller
    public Controller bulletController;
    
    //can edit in inspector
    [SerializeField] 
    public float attackDamage;
    public float speed;
    

    private void Awake()
    {
        bulletController = new ProjectileController(this);
    }

    public override Controller AltController => bulletController;
}
