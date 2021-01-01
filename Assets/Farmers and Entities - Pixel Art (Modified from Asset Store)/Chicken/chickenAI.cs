using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chickenAI : MonoBehaviour
{
    //private stuff
    private Rigidbody2D body;
    private SpriteRenderer image;
    private Animator anim;
    private new Collider2D collider;
    private float attackTimer, turnTimer, jumpTimer, stunTimer, dir;

    //puvblic stuff
    public LayerMask playerLayer;
    public Transform attack;
    public float speed, chargeSpeed, attackDamage, seekRadius, jumpSpeed, attackSpeed, turnTime, jumpTime;
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
        body = GetComponent<Rigidbody2D>();
        image = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        attackTimer = Time.time + attackSpeed;
        jumpTimer = Time.time + jumpTime;
        turnTimer = Time.time + turnTime;
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
    }

    void renderDirection()
    {
        if (body.velocity.x < 0) image.flipX = false;
        else image.flipX = true;
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
        body.velocity = new Vector2(dir * speed, body.velocity.y);
    }

    void changeDir()
    {
        if (Time.time > turnTimer)
        {
            turnTimer = Time.time + turnTime;
            dir *= -1;
        }
    }

    void Jump()
    {
        if (Time.time > jumpTimer && body.velocity.y == 0)
        {
            jumpTimer = Time.time + jumpTime;
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
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
            if (Time.time > attackTimer)
            {
                enemy.GetComponent<Health>().takeDamage(attackDamage);
                attackTimer = Time.time + attackSpeed;
            }

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
        }

        body.velocity = new Vector2(dir * chargeSpeed, body.velocity.y);
    }

    //for testing
    //to draw hitbox on the scene screen
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(attack.position, attackRange);

    }
}
