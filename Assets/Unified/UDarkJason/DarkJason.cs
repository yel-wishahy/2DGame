using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkJason : UEntity
{
    [SerializeField]
    public Sensor_Entity GroundSensor;
    public Vector2 EnemySenseRange;

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

    private Controller AIController;

    private void Awake()
    {
        AIController = new UEnemyFST(this);
    }

    public override Controller AltController => AIController;
}
