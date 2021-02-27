using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class KFC : Item
{
    public float healthBuff;

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
