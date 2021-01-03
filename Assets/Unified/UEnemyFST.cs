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

    public enum States
    {
        Seeking,
        Attack
    }

    public States CurrentState = States.Seeking;

    Vector2 enemyVector;

    Jason enemyEntity;

    Vector2 currentVtr;

    DarkJason entity;

    private Sensor_Entity m_groundSensor;
    private Vector2 m_enemySenseRange;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private SpriteRenderer m_renderer2d;

    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;
    private bool ContactNotGround = false;

    public int Direction = 1;

    float TimeUnit = 0;

    public float AttackInterval = 1.5f;

    void searchSurroundings()
    {
        Collider2D[] possibleEnemies = Physics2D.OverlapBoxAll(currentVtr, m_enemySenseRange, 0);

        foreach(Collider2D enemy in possibleEnemies)
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
    }

    // Use this for initialization
    public void Init()
    {
        currentVtr = entity.transform.position;
        entity.alternativeX = Direction;
        m_groundSensor = entity.GroundSensor;
        m_enemySenseRange = entity.EnemySenseRange;
        m_body2d = entity.GetComponent<Rigidbody2D>();
        m_animator = entity.GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (entity.getHealth() > 0)
        {
            searchSurroundings();
           
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
                            enemyEntity.takeDamage(entity.getAttackDamage());
                        else
                            enemyEntity.setHealth(0);

                        TimeUnit = 0;
                    }

                }
            }
        }

        if (entity.getHealth() > 0 && !ContactNotGround)
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
                m_combatIdle = !m_combatIdle;

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
        else if (Mathf.Abs(entity.alternativeX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
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
