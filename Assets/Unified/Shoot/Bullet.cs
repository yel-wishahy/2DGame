using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : UEntity
{
    private Controller bulletController;

    private void Awake()
    {
        bulletController = new ProjectileController(this);
    }

    public override Controller AltController => bulletController;
}
