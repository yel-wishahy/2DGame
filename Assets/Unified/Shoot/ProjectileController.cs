using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : Controller
{
    private Rigidbody2D body;
    private UEntity entity;

    public ProjectileController(UEntity entity)
    {
        this.entity = entity;
        Init();
    }

    public void Init()
    {
        Debug.Log("init");
        body = entity.GetComponent<Rigidbody2D>();
    }

    public void OnTriggerEnter2D(Collider2D object2D)
    {
        // when bullet hits something
        UEntity enemy = object2D.GetComponent<UEntity>();
        if (enemy != null && enemy.tag != "Player")
        {
            //hurts enemy
            enemy.takeDamage(entity.getAttackDamage());

            //destroys this bullet
            entity.Die();
            GameObject.Destroy(entity);
        }
    }

    public void Update()
    {
        Debug.Log("update");
        body.velocity = entity.transform.right * entity.getSpeed();
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
