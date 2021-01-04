using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : Controller
{
    Player entity;
    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer render;

    private float jumpHeight = 0.5f;




    public playerController(Player entity)
    {
        this.entity = entity;
        Init();
    }
  
  
    // Start is called before the first frame update
    public void Init()
    {
        Debug.Log("init");
        body = entity.GetComponent<Rigidbody2D>();
        anim = entity.GetComponent<Animator>();
        render = entity.GetComponent<SpriteRenderer>();
    }
      
    // Update is called once per frame
    public void Update()
    {
        Debug.Log("update");
        Attack();
        Move();
        checkJumpCollision();
    }

    public void Attack()
    {
        Collider2D[] possibleHits = Physics2D.OverlapCircleAll(entity.attack.position, entity.attackRadius, entity.enemyLayer);

        foreach (Collider2D hit in possibleHits)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                anim.SetBool("Attack1", true);
                hit.GetComponent<UEntity>().takeDamage(entity.getAttackDamage());

            }

        }
        
        
    }

    //do nothing
    public void Jump() { }


    public void Move()
    {
       
        float movementX = Input.GetAxisRaw("Horizontal") * entity.getSpeed();
        body.velocity = new Vector2(movementX, body.velocity.y);
        

        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * jumpHeight);
        }

        if (body.velocity.x < 0) render.flipX = true;
        else render.flipX = false;

        anim.SetFloat("AirSpeedY", body.velocity.y);

        if (body.velocity.x != 0) anim.SetBool("Run", true);
        else anim.SetBool("Run", false);
    }

    void checkJumpCollision()
    {
        Collider2D bottomCollide = Physics2D.OverlapBox(entity.groundCheck.position, entity.range, 0, entity.groundLayer);

        if (bottomCollide != null)
        {
          
            if (bottomCollide.gameObject.tag == "Ground" && Input.GetKeyDown(KeyCode.Space)){
                body.velocity = new Vector2(body.velocity.x, entity.getJumpForce());
                anim.SetBool("Jump", true);
                anim.SetBool("Grounded", false);
            }

        }

        if (bottomCollide == null)
        {
            anim.SetBool("Grounded", false);
        } else
        {
            anim.SetBool("Grounded", true);
        }


    }

    public void HandleAnimations()
    {
        throw new System.NotImplementedException();
    }


}
