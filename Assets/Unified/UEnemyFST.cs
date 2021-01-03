using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UEnemyFST : Controller
{ 
    public enum States
    {
        Seeking,
        Attack
    }

    States CurrentState = States.Seeking;

    DarkJason entity;
    Vector2 currentVtr;

    Jason enemyEntity;
    Vector2 enemyVector;


    public int Direction = 1;
    public int DamageDealt = 6;
    public float searchRadius = 4;
    float TimeUnit = 0;
    public float AttackInterval = 1.5f;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Entity m_groundSensor;
    private Sensor_Entity m_enemySensor;

    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;

    public UEnemyFST(DarkJason entity)
    {
        this.entity = entity;
        Init();
    }


    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Init()
    {
        currentVtr = entity.transform.position;
        entity.alternativeX = Direction;
        m_animator = entity.GetComponent<Animator>();
        m_body2d = entity.GetComponent<Rigidbody2D>();
        m_groundSensor = entity.GroundSensor;
        m_enemySensor = entity.EnemySensor;

    }

    public void Jump()
    {
        throw new System.NotImplementedException();
    }

    public void Move()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        if (entity.getHealth() > 0)
        {

            if (TimeUnit < AttackInterval)
                TimeUnit += Time.deltaTime;

            if (CurrentState == States.Seeking)
            {
                if (entity.alternativeX == 0)
                    entity.alternativeX = Direction;

                if (entity.transform.position.x > currentVtr.x + 4)
                {
                    entity.alternativeX = -1;
                }
                else if (entity.transform.position.x < currentVtr.x - 4)
                {
                    entity.alternativeX = 1;
                }
            }
            else if (CurrentState == States.Attack)
            {
                if (enemyVector.x - entity.transform.position.x > 1.1f)
                {
                    entity.alternativeX = 1;
                }
                else if (enemyVector.x - entity.transform.position.x < -1.1f)
                {
                    entity.alternativeX = -1;
                }
                else if (enemyVector.y - entity.transform.position.y > 1.1f)
                {
                    entity.alternativeY = 1;
                }
                else if (enemyEntity != null && !enemyEntity.Hurt)
                {
                    entity.alternativeX = 0;

                    entity.Attacking = true;

                    if (entity.AddDamage && TimeUnit > AttackInterval)
                    {
                        enemyEntity.Hurt = true;

                        if (enemyEntity.getHealth() > 0)
                            enemyEntity.takeDamage(DamageDealt);
                        else
                            enemyEntity.setHealth(0);

                        TimeUnit = 0;
                    }

                }
            }
        }

        // -- Handle Animations --
        //Death
        if (entity.getHealth() <= 0)
        {
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                m_isDead = true;
                m_animator.SetTrigger("Death");
            }

            Collider2D collider = entity.GetComponent<CapsuleCollider2D>();

            collider.enabled = false;
            Object.Destroy(entity.GetComponent<Rigidbody2D>());
            ResetControls();
            entity.enabled = false;
        }

        //Hurt
        else if (entity.Hurt && entity.HurtPhase == 0 && !m_isDead)
        {
            m_animator.SetTrigger("Hurt");
            entity.HurtPhase = 1;
        }
        else if (entity.Hurt && entity.HurtPhase == 1 && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt") && !m_isDead)
        {
            entity.HurtPhase = 0;
            entity.Hurt = false;
        }

        //Attack
        else if (entity.Attacking && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            m_combatIdle = true;
            m_animator.SetTrigger("Attack");

            entity.Attacking = false;
        }

        //Jump
        else if (entity.alternativeY > 0 && m_grounded)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, entity.getJumpForce());
            m_groundSensor.Disable(0.2f);

            entity.alternativeY = 0;
        }

        //Run
        else if (Mathf.Abs(entity.GetComponent<Rigidbody2D>().velocity.x) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }


    void OnTriggerStay2D(Collider2D enemy)
    {
        if (enemy.tag == "Player" && enemy.GetComponent<Jason>() != null)
        {
            enemyEntity = enemy.GetComponent<Jason>();

            if (enemyEntity != null && enemyEntity.getHealth() > 0)
            {
                CurrentState = States.Attack;
                enemyVector = enemy.transform.position;
                entity.AttackMode = true;
            }
        }
    }


    void OnTriggerExit2D(Collider2D enemy)
    {
        if (enemy.tag == "Player")
        {
            entity.alternativeX = 0;
            entity.alternativeY = 0;
            entity.Attacking = false;
            entity.AttackMode = false;
            currentVtr = entity.transform.position;
            CurrentState = States.Seeking;
        }
    }

    public void ResetControls()
    {
        entity.alternativeX = 0;
        entity.alternativeY = 0;
        entity.Attacking = false;
        entity.AttackMode = false;
        entity.AddDamage = false;
        entity.Hurt = false;
    }
}
