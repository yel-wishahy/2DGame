using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Coal : Item
{
    [SerializeField] private Collider2D worldCollider;
    [SerializeField] private float healthBuff;

    public override bool Use(Player user)
    {
        try
        {
            user.gainHealth(healthBuff);
            return true;
        }
        catch
        {
            return false;
        }

    }
}
