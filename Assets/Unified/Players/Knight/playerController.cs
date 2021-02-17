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

    private bool grabbing = false;


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

    public bool Hurt(bool Damaged)
    {
        return true;
    }

    // Update is called once per frame
    public void Update()
    {
        LedgeGrab();
        Attack();
        projectileAttack();
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

    void projectileAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        Player.Instantiate(entity.projectilePrefab, entity.attack.position, entity.attack.rotation);
    }



    //do nothing
    public void Jump() { }

    public void LedgeGrab()
    {

        float distanceAbove = 2;

        RaycastHit2D aboveHit = Physics2D.Raycast(new Vector2(body.position.x, body.position.y + distanceAbove), new Vector2(body.velocity.x, 0));
        RaycastHit2D levelHit = Physics2D.Raycast(new Vector2(body.position.x, body.position.y), new Vector2(body.velocity.x, 0));

        if (Input.GetMouseButton(1) && levelHit.collider != null)
        {
            //im not sure if the first or will work, if C# does optimizations: then yes
            if((aboveHit.collider == null || aboveHit.distance > 1) && levelHit.distance < 0.5)
            {
                body.gravityScale = 0.0f;
                body.velocity = new Vector2(body.velocity.x, 0);
            } else
            {
                body.gravityScale = 1f;
            }
        } else
        {
            body.gravityScale = 1f;
        }
    }

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

    public void Hurt()
    {
        
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
