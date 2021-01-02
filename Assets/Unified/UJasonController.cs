﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UJasonController : Controller
{
    private Jason entity;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Entity m_groundSensor;

    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;
    private bool ContactNotGround = false;

    public UJasonController(Jason entity)
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
        m_animator = entity.GetComponent<Animator>();
        m_body2d = entity.GetComponent<Rigidbody2D>();
        m_groundSensor = entity.transform.Find("GroundSensor").GetComponent<Sensor_Entity>();
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
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = 0;

        
        inputX = Input.GetAxis("Horizontal");
        entity.alternativeY = Input.GetAxis("Vertical");
        entity.Attacking = Input.GetMouseButtonDown(0);
        entity.AttackMode = Input.GetKeyDown("f");
        


        if (entity.getHealth() > 0 && !ContactNotGround)
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
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
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