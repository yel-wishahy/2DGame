using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkJason : UEntity
{
    [SerializeField]
    public Sensor_Entity GroundSensor;
    public Vector2 EnemySenseRange;
    public float AttackInterval;

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

    [HideInInspector]
    public Vector2 enemyVector;
    [HideInInspector]
    public Jason enemyEntity;
    [HideInInspector]
    public Vector2 currentVtr;

    private UEnemyFST AIController;

    private void Awake()
    {
        AIController = new UEnemyFST(this);
    }

    public override Controller AltController => AIController;

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

    void OnTriggerStay2D(Collider2D enemy)
    {
        print(enemy.tag);
        if (enemy.tag == "Player" && enemy.GetComponent<Jason>() != null)
        {
            enemyEntity = enemy.GetComponent<Jason>();

            if (enemyEntity != null && enemyEntity.getHealth() > 0)
            {
                AIController.CurrentState = UEnemyFST.States.Attack;
                enemyVector = enemy.transform.position;
                AttackMode = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D enemy)
    {
        if (enemy.tag == "Player")
        {
            alternativeX = 0;
            alternativeY = 0;
            Attacking = false;
            AttackMode = false;
            currentVtr = this.transform.position;
            AIController.CurrentState = UEnemyFST.States.Seeking;
        }
    }

}
