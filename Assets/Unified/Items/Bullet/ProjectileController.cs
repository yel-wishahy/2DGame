using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : Controller
{
    private Rigidbody2D body;
    private UEntity entity;
    private float dir = 0;
    private SpriteRenderer render;

    public ProjectileController(UEntity entity)
    {
        this.entity = entity;
        Init();
    }

    public void Init()
    {
        Debug.Log("init");
        body = entity.GetComponent<Rigidbody2D>();
        render = entity.GetComponent<SpriteRenderer>();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x >= body.position.x)
            dir = 1;
        else
            dir = -1;

        if (dir < 0)
            render.flipX = true;


    }

    public void OnTriggerEnter2D(Collider2D object2D)
    {
        // when bullet hits something
        UEntity enemy = object2D.GetComponent<UEntity>();
        if (enemy != null && enemy.tag != "Player" && entity.isAlive())
        {
            //hurts enemy
            enemy.takeDamage(entity.getAttackDamage());

            //destroys this bullet
            entity.Die();
            entity.GetComponent<SpriteRenderer>().enabled = false;
            
        }
    }

    public void Update()
    {
        if (entity.isAlive())
        {
            body.velocity = new Vector2(dir * entity.getSpeed(), 0);

        }
    }


    //unused interface methods
    public void OnTriggerExit2D(Collider2D object2D)
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerStay2D(Collider2D object2D)
    {
        throw new System.NotImplementedException();
    }
    public void Jump()
    {
        throw new System.NotImplementedException();
    }

    public void Move()
    {
        throw new System.NotImplementedException();
    }

    public void OnCollisionEnter2D(Collision2D object2D)
    {
        throw new System.NotImplementedException();
    }

    public void OnCollisionExit2D(Collision2D object2D)
    {
        throw new System.NotImplementedException();
    }

    public void OnCollisionStay2D(Collision2D object2D)
    {
        throw new System.NotImplementedException();
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public bool Hurt(bool Damaged)
    {
        throw new System.NotImplementedException();
    }

}
