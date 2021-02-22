﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken :UEntity
{ 
    //public stuff
    public LayerMask playerLayer;
    public Transform attack;
    public Vector3 attackRange = new Vector3(1.15f, 0.5f, 0);
    public float chargeSpeed = 4;
    public float seekRadius = 2.5f;
    public float attackTime = 1;
    public float turnTime = 3;
    public float jumpTime = 6;
    public float stunTime = 1.5f;
    public float hurtTime = 0.45f;
    public GameObject smokeDeathEffect;
    public GameObject healthBar;
    private Controller AIController;

    private void Awake()
    {
        AIController = new chickenController(this);
    }

    public override Controller AltController => AIController;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(attack.position, attackRange);
    }

}
