using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chickenAI : MonoBehaviour
{
    //private stuff
    private Entity currentEntity;
    private Rigidbody2D body;
    private SpriteRenderer image;
    private Animator anim;
    private new Collider2D collider;
    private float attackTimer, turnTimer, jumpTimer, stunTimer, dir;

    //puvblic stuff
    public LayerMask playerLayer;
    public Transform attack;
    public float speed, chargeSpeed, attackDamage, seekRadius, jumpSpeed, attackSpeed;
    public int timer;
    public chickenState state = chickenState.idle;
    public Vector3 attackRange;
    public float stunTime;

    public enum chickenState
    {
       moving, charge, charging, attack, idle, stunned
    }

    // Start is called before the first frame update
    void Start()
    {
        currentEntity = GetComponent<Entity>();
        body = GetComponent<Rigidbody2D>();
        image = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        attackTimer = 0;
        jumpTimer = 0;
        turnTimer = 0;
        stunTimer = 0;

        dir = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Hurt();

        if (state != chickenState.stunned)
        {
            if (state != chickenState.charging) changeDir();
            Move();
            Jump();
            Attack();
            renderDirection();
        }
        else
        {
            currentEntity.ResetControls();
        }
    }

    void renderDirection()
    {
        /*
        if (body.velocity.x < 0) image.flipX = false;
        else image.flipX = true;
        */
    }

    void Hurt()
    {
        if (anim.GetBool("Hurt") && stunTimer == 0)
        {
            state = chickenState.stunned;
            stunTimer = Time.time + stunTime;
        }

        if (anim.GetBool("Hurt") && Time.time > stunTimer)
        {
            stunTimer = 0;
            state = chickenState.moving;
            anim.SetBool("Hurt", false);

        }
    }

    void Move()
    {
        //  body.velocity = new Vector2(dir * speed, body.velocity.y);
        currentEntity.m_speed = Mathf.Abs(speed);
    }

    void changeDir()
    {
        if (turnTimer > timer)
        {
            turnTimer = 0;
            dir *= -1;

            currentEntity.alternativeX = dir;
        }

        turnTimer++;
    }

    void Jump()
    {
        if (jumpTimer > timer * 3 && body.velocity.y == 0)
        {
            jumpTimer = 0;
            currentEntity.alternativeY = 1;
            currentEntity.m_jumpForce = Mathf.Abs(jumpSpeed);
            // body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }

        jumpTimer++;

    }

    void Attack()
    {
        Collider2D enemy = null;

        Collider2D[] possibleEnemies = Physics2D.OverlapCircleAll(body.position, seekRadius, playerLayer);

        if (possibleEnemies.Length < 1) state = chickenState.moving;

        foreach (Collider2D pe in possibleEnemies)
        {
            enemy = pe;

            if (DetectAttackCollision())
            {
                state = chickenState.attack;
            }
            else if (state != chickenState.charge || state != chickenState.charging)
            {
                state = chickenState.charge;
            }
        }


        if (state == chickenState.attack)
        {
            if (attackTimer > attackSpeed)
            {
                var entity = enemy.GetComponent<Entity>();
                entity.Health -= (int) attackDamage;
                entity.Hurt = true;

                attackTimer = 0;
            }

            attackTimer++;
            state = chickenState.moving;
        } else if (state == chickenState.charge || state == chickenState.charging)
        {
            Charge(enemy);
            state = chickenState.charging;
        }
    }

    private bool DetectAttackCollision()
    {
        Collider2D attackCollide = Physics2D.OverlapBox(attack.position, attackRange, 0, playerLayer);

        if (attackCollide != null)
        {

            return true;

        }
        else return false;

    }

    void Charge(Collider2D enemy)
    {
        float dir = 1;
        if (body.position.x - enemy.GetComponent<Rigidbody2D>().position.x > 0)
        {
            dir *= -1;
            currentEntity.alternativeX = dir;
        }

       // body.velocity = new Vector2(dir * chargeSpeed, body.velocity.y);
    }

    //for testing
    //to draw hitbox on the scene screen
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(attack.position, attackRange);

    }
}
