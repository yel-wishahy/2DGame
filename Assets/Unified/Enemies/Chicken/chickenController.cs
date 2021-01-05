using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chickenController : Controller
{

    //state enum
    private enum chickenState
    {
        moving, charge, charging, attack, idle, stunned
    }

    //private stuff
    private Chicken entity;
    private Rigidbody2D body;
    private SpriteRenderer image;
    private Animator anim;
    private chickenState state;

    //counters and directions
    private float attackTimer, turnTimer, jumpTimer, stunTimer, dir;
    



    public chickenController(Chicken entity)
    {
        this.entity = entity;
        Init();
    }

    // Start is called before the first frame update
    public void Init()
    {
        body = entity.GetComponent<Rigidbody2D>();
        image = entity.GetComponent<SpriteRenderer>();
        anim = entity.GetComponent<Animator>();

        attackTimer = entity.getTime() + entity.attackTime;
        jumpTimer = entity.getTime() + entity.jumpTime;
        turnTimer = entity.getTime() + entity.turnTime;
        stunTimer = 0;

        state = chickenState.moving;

        dir = 1;
    }

    // Update is called once per frame
    public void Update()
    {
        updateHurt();

        if (state != chickenState.stunned)
        {
            if (state != chickenState.charging) changeDir();
            Move();
            Jump();
            Attack();
            renderDirection();
        }
    }

    private void renderDirection()
    {
        if (body.velocity.x < 0) image.flipX = false;
        else image.flipX = true;
    }

    //check
    public bool Hurt(bool Damaged)
    {
        Debug.Log("Hurt");
        if (Damaged)
            anim.SetBool("Hurt", true);

        updateHurt();

        return anim.GetBool("Hurt");

    }

    void updateHurt()
    {
        if (anim.GetBool("Hurt") && stunTimer == 0)
        {
            state = chickenState.stunned;
            stunTimer = entity.getTime() + entity.stunTime;
        }

        if (anim.GetBool("Hurt") && entity.getTime() > stunTimer)
        {
            stunTimer = 0;
            state = chickenState.moving;
            anim.SetBool("Hurt", false);
        }
    }

    //change the direction of the chicken
    private void changeDir()
    {

        if (entity.getTime() > turnTimer)
        {
            turnTimer = entity.getTime() + entity.turnTime;
            dir *= -1;
        }
    }

    //chicken attacks any nearby enemies
    public void Attack()
    {
        Collider2D enemy = null;

        Collider2D[] possibleEnemies = Physics2D.OverlapCircleAll(body.position, entity.seekRadius, entity.playerLayer);

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
            if (entity.getTime() > attackTimer)
            {
                enemy.GetComponent<UEntity>().takeDamage(entity.getAttackDamage());
                attackTimer = entity.getTime() + entity.attackTime;
            }

            state = chickenState.moving;
        }
        else if (state == chickenState.charge || state == chickenState.charging)
        {
            Charge(enemy);
            state = chickenState.charging;
        }
    }

    //charge the chicken towards an enemy
    private void Charge(Collider2D enemy)
    {
        float dir = 1;
        if (body.position.x - enemy.GetComponent<Rigidbody2D>().position.x > 0)
        {
            dir *= -1;
        }

        body.velocity = new Vector2(dir * entity.chargeSpeed, body.velocity.y);
    }

    //detects if another collider enters chicken's collider attack box
    private bool DetectAttackCollision()
    {
        Collider2D attackCollide = Physics2D.OverlapBox(entity.attack.position, entity.attackRange, 0, entity.playerLayer);

        if (attackCollide != null)
        {

            return true;

        }
        else return false;

    }

    //jumps the chicken
    public void Jump()
    {
        if (entity.getTime() > jumpTimer && body.velocity.y == 0)
        {
            jumpTimer = entity.getTime() + entity.jumpTime;
            body.velocity = new Vector2(body.velocity.x, entity.getJumpForce());
        }
    }

    //moves the chicken
    public void Move()
    {
        body.velocity = new Vector2(dir * entity.getSpeed(), body.velocity.y);
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
