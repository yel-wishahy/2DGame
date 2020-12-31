using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chickenAI : MonoBehaviour
{
    //private stuff
    private Rigidbody2D body;
    private SpriteRenderer image;
    private new Collider2D collider;
    private float moveCount;
    private float jumpCount;
    private float dir;

    //puvblic stuff
    public LayerMask playerLayer;
    public float speed, chargeSpeed, attackDamage, seekRadius, attackDistance, turnTimer, jumpTimer, jumpSpeed;
    public chickenState state = chickenState.idle;

    public enum chickenState
    {
        idle, charge, attack
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        image = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        moveCount = 0;
        jumpCount = 0;
        dir = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        Collider2D enemy = null;

        Collider2D[] possibleEnemies = Physics2D.OverlapCircleAll(body.position, seekRadius, playerLayer);

        foreach(Collider2D pe in possibleEnemies)
        {
                enemy = pe;

            if (enemy.Distance(collider).distance <= attackDistance)
            {
                state = chickenState.attack;
            }
            else state = chickenState.charge;
        }

        switch (state)
        {
            case chickenState.idle:
                if (Random.Range(0.0f, 1.0f) >= 0.25f) Move();
                else
                {
                    Jump();
                }
                break;
            case chickenState.charge:
                Charge(enemy);
                state = chickenState.idle;
                break;
            case chickenState.attack:
                Attack(enemy);
                state = chickenState.idle;
                break;
        }
    }

    void Move()
    {
        if (moveCount > turnTimer)
        {
            moveCount = 0;
            dir *= -1;
        }

        if (dir == 1) image.flipX = true;
        else image.flipX = false;
     

        body.velocity = new Vector2(dir * speed, body.velocity.y);

        moveCount++;
    }

    void Attack(Collider2D enemy)
    {
        enemy.GetComponent<Health>().takeDamage(attackDamage);
    }


    void Jump()
    {
        if (jumpCount > jumpTimer)
        {
            jumpCount = 0;
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
        
        jumpCount++;

    }

    void Charge(Collider2D enemy)
    {
        float dir = 1;
        if (body.position.x - enemy.GetComponent<Rigidbody2D>().position.x > 0)
        {
            dir *= -1;
        }

        if (dir == 1) image.flipX = true;
        else image.flipX = false;

        body.velocity = new Vector2(dir * chargeSpeed, body.velocity.y);
    }
}
