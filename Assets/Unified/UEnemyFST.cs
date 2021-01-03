using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UEnemyFST : Controller
{
    public UEnemyFST(DarkJason entity)
    {
        this.entity = entity;
        Init();
    }

    DarkJason entity;

    public States CurrentState = States.Seeking;

    public enum States
    {
        Seeking,
        Attack
    }


    private Sensor_Entity m_groundSensor;
    private Vector2 m_enemySenseRange;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private SpriteRenderer m_renderer2d;

    public int Direction = 1;

    float TimeUnit = 0;

    public float AttackInterval = 1.5f;

    // Use this for initialization
    public void Init()
    {
        entity.currentVtr = entity.transform.position;
        entity.alternativeX = Direction;
        m_groundSensor = entity.GroundSensor;
        m_enemySenseRange = entity.EnemySenseRange;
        m_body2d = entity.GetComponent<Rigidbody2D>();
        m_animator = entity.GetComponent<Animator>();
        m_renderer2d = entity.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
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

                if (entity.transform.position.x > entity.currentVtr.x + 4)
                {
                    entity.alternativeX = -1;
                }
                else if (entity.transform.position.x < entity.currentVtr.x - 4)
                {
                    entity.alternativeX = 1;
                }
            }
            else if (CurrentState == States.Attack)
            {
                if (entity.enemyVector.x - entity.transform.position.x > 1.1f)
                {
                    entity.alternativeX = 1;
                }
                else if (entity.enemyVector.x - entity.transform.position.x < -1.1f)
                {
                    entity.alternativeX = -1;
                }
                else if (entity.enemyVector.y - entity.transform.position.y > 1.1f)
                {
                    entity.alternativeY = 1;
                }
                else if (entity.enemyEntity != null && !entity.enemyEntity.Hurt)
                {
                    entity.alternativeX = 0;

                    entity.Attacking = true;

                    if (entity.AddDamage && TimeUnit > AttackInterval)
                    {
                        entity.enemyEntity.Hurt = true;

                        if (entity.enemyEntity.getHealth() > 0)
                            entity.enemyEntity.takeDamage(entity.getAttackDamage());
                        else
                            entity.enemyEntity.setHealth(0);

                        TimeUnit = 0;
                    }

                }
            }
        }

        if (entity.getHealth() > 0 && !entity.ContactNotGround)
        {
            // Swap direction of sprite depending on walk direction
            if (m_body2d.velocity.x > 0)
            {
                m_renderer2d.flipX = false;
            }
            else m_renderer2d.flipX = true;

            // Move
            m_body2d.velocity = new Vector2(entity.alternativeX * entity.getSpeed(), m_body2d.velocity.y);

            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);


            //Change between idle and combat idle
            if (entity.AttackMode)
                entity.m_combatIdle = !entity.m_combatIdle;

        }

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            entity.AddDamage = true;
        }
        else
        {
            entity.AddDamage = false;
        }

        // -- Handle Animations --
        //Death
        if (entity.getHealth() <= 0)
        {
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                entity.m_isDead = true;
                m_animator.SetTrigger("Death");
            }

            Collider2D collider = entity.GetComponent<CapsuleCollider2D>();

            collider.enabled = false;
            Object.Destroy(entity.GetComponent<Rigidbody2D>());
            ResetControls();
            entity.enabled = false;
        }

        //Hurt
        else if (entity.Hurt && entity.HurtPhase == 0 && !entity.m_isDead)
        {
            m_animator.SetTrigger("Hurt");
            entity.HurtPhase = 1;
        }
        else if (entity.Hurt && entity.HurtPhase == 1 && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt") && !entity.m_isDead)
        {
            entity.HurtPhase = 0;
            entity.Hurt = false;
        }

        //Attack
        else if (entity.Attacking && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            entity.m_combatIdle = true;
            m_animator.SetTrigger("Attack");

            entity.Attacking = false;
        }

        //Jump
        else if (entity.alternativeY > 0 && entity.m_grounded)
        {
            m_animator.SetTrigger("Jump");
            entity.m_grounded = false;
            m_animator.SetBool("Grounded", entity.m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, entity.getJumpForce());
            m_groundSensor.Disable(0.2f);

            entity.alternativeY = 0;
        }

        //Run
        else if (Mathf.Abs(entity.alternativeX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (entity.m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }

    public void Move()
    {
        throw new System.NotImplementedException();
    }

    public void Jump()
    {
        throw new System.NotImplementedException();
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    void OnCollisionStay2D(Collision2D object2D)
    {
        if (!object2D.collider.isTrigger && !entity.m_grounded)
        {
            entity.ContactNotGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D object2D)
    {
        if (!object2D.collider.isTrigger && !entity.m_grounded)
        {
            entity.ContactNotGround = false;
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
