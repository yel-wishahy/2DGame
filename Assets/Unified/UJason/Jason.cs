using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jason : UEntity
{
    [SerializeField]
    public Transform attackOrigin;
    public Vector2 attackBox;
    public LayerMask enemyLayer;

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
    [HideInInspector]
    public int HurtPhase;
    [HideInInspector]
    public bool m_grounded = false;
    [HideInInspector]
    public bool m_combatIdle = false;
    [HideInInspector]
    public bool m_isDead = false;
    [HideInInspector]
    public bool ContactNotGround = false;

    private Controller userController;

    private void Awake()
    {
        userController = new UJasonController(this);
    }

    public override Controller UserController => userController;

    void OnCollisionStay2D(Collision2D object2D)
    {
        if (!object2D.collider.isTrigger && !m_grounded)
        {
            ContactNotGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D object2D)
    {
        if (!object2D.collider.isTrigger && !m_grounded)
        {
            ContactNotGround = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(attackOrigin.position, attackBox);
    }


}
