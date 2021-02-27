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
    public int inventoryCapacity;
    
    [HideInInspector] public Inventory inventory;
    [HideInInspector] public PlayerSettings settings;

    private void Awake()
    {
         userController = new playerController(this);
         inventory = new Inventory(this);
         settings = new PlayerSettings();
         InitSettings();
    }

    private void InitSettings()
    {
        settings.AddorUpdateSetting("Primary", KeyCode.Mouse0);
        settings.AddorUpdateSetting("Secondary", KeyCode.Mouse1);
        settings.AddorUpdateSetting("Ranged", KeyCode.Mouse2);
        settings.AddorUpdateSetting("Jump", KeyCode.Space);
    }
    

    //override controller property with player controller class

    public override Controller UserController => userController;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(attack.position, attackRadius);
    }

}
