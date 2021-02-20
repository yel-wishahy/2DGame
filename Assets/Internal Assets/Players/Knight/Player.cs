﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UEntity
{
    [SerializeField]
    public float attackRadius = 0.8f;
    public Vector3 range = new Vector3(1, 0.2f, 0);
    public Transform groundCheck, attack;
    public LayerMask groundLayer, enemyLayer;
    public Controller userController;
    public GameObject projectilePrefab;

    [HideInInspector]
    private int coalsCollected;

    private void Awake()
    {
         userController = new playerController(this);
         coalsCollected = 0;
    }
    

    //override controller property with player controller class

    public override Controller UserController => userController;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(attack.position, attackRadius);
    }

    public void collectCoal()
    {
        coalsCollected++;
    }

    public int getCoalsCollected()
    {
        return coalsCollected;
    }

    


}