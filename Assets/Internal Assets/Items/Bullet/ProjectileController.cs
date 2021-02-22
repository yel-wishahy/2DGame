using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : Controller
{
    private Rigidbody2D body;
    private Bullet self;
    private float dir = 0;
    private SpriteRenderer render;

    public ProjectileController(Bullet projectile)
    {
        self = projectile;
        Init();
    }

    public void Init()
    {
        Debug.Log("init");
        body = self.GetComponent<Rigidbody2D>();
        render = self.GetComponent<SpriteRenderer>();

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
        if (enemy != null && enemy.tag != "Player" && self.isAlive())
        {
            //hurts enemy
            enemy.takeDamage(self.attackDamage);

            //destroys this bullet
            self.Die();
        }

        if (object2D.GetComponent<Collider2D>().tag == "Ground")
        {
            //destroys this bullet if it hits ground
            self.Die();

        }
    }

    public void Update()
    {
        if (self.isAlive())
        {
            body.velocity = new Vector2(dir * self.speed, 0);

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
