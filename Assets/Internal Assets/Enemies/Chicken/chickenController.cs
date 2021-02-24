using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class chickenController : Controller
{

    //state enum
    private enum chickenState
    {
        moving, charge, charging, attack, idle, stunned, hurt
    }

    //private stuff
    private Chicken entity;
    private Rigidbody2D body;
    private SpriteRenderer image;
    private Animator anim;
    private chickenState state;
    private GameObject instantiatedEffect;

    //counters and directions
    private float attackTimer, turnTimer, jumpTimer, stunTimer, hurtTimer, dir;
    
    private float fallDistance = 2;
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
        hurtTimer = 0;

        state = chickenState.moving;

        dir = 1;
    }

    // Update is called once per frame
    public void Update()
    {
        //check and update stunned or hurt animations and states 
        updateHurtAnimations();

        //only do these if not stunned or hurt
        if (state != chickenState.stunned && state != chickenState.hurt)
        {
            Move();
            Jump();
            Attack();
            
            //change directions if need be
            //based on timer
            if (state != chickenState.charging) changeDir(); 
            //check for edges to prevent falling when moving
            CheckEdge();
        }
        
        //render direction of sprite
        renderDirection();
    }

    private void renderDirection()
    {
        if (body.velocity.x < 0) image.flipX = false;
        else image.flipX = true;
    }

    //make sure chicken doesn't fall
    private void CheckEdge()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(new Vector2(body.position.x - 1, body.position.y), new Vector2(0, -1));
        RaycastHit2D rightHit = Physics2D.Raycast(new Vector2(body.position.x + 1, body.position.y), new Vector2(0, -1));

        if (leftHit.distance > fallDistance)
        {
            dir = 1;

        }
        
        if (rightHit.distance > fallDistance)
        {
            dir = -1;

        }
        
    }
    
    //called by the game engine when entity is damaged by another entity
    public bool Hurt(bool Damaged)
    {
        //check if alive
        if (!entity.isAlive())
        {
            instantiatedEffect = Chicken.Instantiate(entity.smokeDeathEffect, body.position, entity.transform.rotation);
            Chicken.Destroy(entity.healthBar);
            Chicken.Instantiate(entity.foodDrop, body.position, entity.transform.rotation);
            entity.Die();
        }
        
        //update hurt states when damaged
        if (Damaged)
        {
            anim.SetBool("Hurt", true);
            hurtTimer = entity.getTime() + entity.hurtTime;
            state = chickenState.hurt;
        }
        
        updateHurtAnimations();
        

        return anim.GetBool("Hurt");
    }

    void updateHurtAnimations()
    {
        if (hurtTimer != 0 && entity.getTime() > hurtTimer)
        {
            hurtTimer = 0;
            anim.SetBool("Hurt", false);
            anim.SetBool("Stunned", true);
            stunTimer = entity.getTime() + entity.stunTime;
            state = chickenState.stunned;
        }

        if (stunTimer != 0 && entity.getTime() > stunTimer)
        {
            stunTimer = 0;
            state = chickenState.moving;
            anim.SetBool("Stunned", false);
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
