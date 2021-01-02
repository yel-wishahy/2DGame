using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

    public float                m_speed = 4.0f;
    public float                m_jumpForce = 7.5f;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Entity       m_groundSensor;
    public bool                m_grounded = false;
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;
    public bool ContactNotGround = false;

    public int Health = 69;

    public bool AlternativeControl = false;
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
    int HurtPhase;

    // Use this for initialization

    void OnCollisionStay2D(Collision2D object2D)
    {
        if (!object2D.collider.isTrigger && !m_grounded)
        {
            ContactNotGround = true;
        }
    }
    void Start () {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Entity>();
    }
	
    public void ResetControls()
    {
        alternativeX = 0;
        alternativeY = 0;
        Attacking = false;
        AttackMode = false;
        AddDamage = false;
        Hurt = false;
    }
	// Update is called once per frame
	void Update () {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            ContactNotGround = false; // If contact on ground, disable ContactNotGround (side collision)
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if(m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = 0;

        if (!AlternativeControl)
        {
            inputX = Input.GetAxis("Horizontal");
            alternativeY = Input.GetAxis("Vertical");
            Attacking = Input.GetMouseButtonDown(0);
            AttackMode = Input.GetKeyDown("f");
        }
        else
            inputX = alternativeX;

        if (Health > 0 && !ContactNotGround)
        {
            // Swap direction of sprite depending on walk direction
            if (inputX > 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if (inputX < 0)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            // Move
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);


            //Change between idle and combat idle
            if (!AlternativeControl && AttackMode)
                m_combatIdle = !m_combatIdle;

            if (AlternativeControl)
                m_combatIdle = AttackMode;

        }

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            AddDamage = true;
        }
        else
        {
            AddDamage = false;
        }

        // -- Handle Animations --
        //Death
        if (Health <= 0)
        {
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                m_isDead = true;
                m_animator.SetTrigger("Death");
            }

            Collider2D collider = GetComponent<CapsuleCollider2D>();

            collider.enabled = false;
            Destroy(m_body2d);
            ResetControls();
            this.enabled = false;
            print("Dead");
        }

        //Hurt
        else if (Hurt && HurtPhase == 0 && !m_isDead)
        {
            m_animator.SetTrigger("Hurt");
            HurtPhase = 1;
        }
        else if (Hurt && HurtPhase == 1 && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt") && !m_isDead)
        {
            HurtPhase = 0;
            Hurt = false;
        }

        //Attack
        else if (Attacking && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            m_combatIdle = true;
            m_animator.SetTrigger("Attack");

            Attacking = false;
        }

        //Jump
        else if (alternativeY > 0 && m_grounded)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);

            alternativeY = 0;
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
}
