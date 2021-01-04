using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UJasonController : Controller
{
    private Jason entity;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Entity m_groundSensor;
    private SpriteRenderer m_render2d;

    public UJasonController(Jason entity)
    {
        this.entity = entity;
        Init();
    }

    public void Attack()
    {
        
    }

    public void Init()
    {
        m_animator = entity.GetComponent<Animator>();
        m_body2d = entity.GetComponent<Rigidbody2D>();
        m_groundSensor = entity.transform.Find("GroundSensor").GetComponent<Sensor_Entity>();
        m_render2d = entity.GetComponent<SpriteRenderer>();
    }

    public void Jump()
    {
        
    }

    public void Move()
    {
        
    }

    public bool Hurt(bool Damaged)
    {
        if (Damaged)
            entity.Hurt = true;

        return entity.Hurt;
    }

    public void Update()
    {
        //Check if character just landed on the ground
        if (!entity.m_grounded && m_groundSensor.State())
        {
            entity.m_grounded = true;
            m_animator.SetBool("Grounded", entity.m_grounded);
        }

        //Check if character just started falling
        if (entity.m_grounded && !m_groundSensor.State())
        {
            entity.m_grounded = false;
            m_animator.SetBool("Grounded", entity.m_grounded);
        }

        // -- Handle input and movement --
        float inputX = 0;

        
        inputX = Input.GetAxis("Horizontal");
        entity.alternativeY = Input.GetAxis("Vertical");
        entity.Attacking = Input.GetMouseButtonDown(0);
        entity.AttackMode = Input.GetKeyDown("f");
        


        if (entity.getHealth() > 0 && !entity.ContactNotGround)
        {
            // Swap direction of sprite depending on walk direction
            if (inputX > 0)
                entity.transform.localScale = new Vector3(-Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);
            else if (inputX < 0)
                entity.transform.localScale = new Vector3(Mathf.Abs(entity.transform.localScale.x), entity.transform.localScale.y, entity.transform.localScale.z);

            // Move
            m_body2d.velocity = new Vector2(inputX * entity.getSpeed(), m_body2d.velocity.y);

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
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (entity.m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
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
    public void OnTriggerStay2D(Collider2D object2D)
    {
        
    }

    public void OnTriggerEnter2D(Collider2D object2D)
    {
        
    }

    public void OnCollisionStay2D(Collision2D object2D)
    {
        
    }

    public void OnTriggerExit2D(Collider2D object2D)
    {
        
    }

    public void OnCollisionExit2D(Collision2D object2D)
    {
        
    }

    public void OnCollisionEnter2D(Collision2D object2D)
    {
        
    }
}
