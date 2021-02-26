using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class playerController : Controller
{
    Player entity;
    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer render;

    private float jumpHeight = 0.5f;

    private bool sliding = false;




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
        Physics2D.queriesStartInColliders = false;
    }

    public bool Hurt(bool Damaged)
    {
        anim.SetBool("Hurt", true);
        return true;
    }

    // Update is called once per frame
    public void Update()
    {
        Attack();
        projectileAttack();
        Move();
        Jump();
        WallSlide();
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
        if (Input.GetButtonDown("Fire3"))
        Player.Instantiate(entity.projectilePrefab, entity.attack.position, entity.attack.rotation);
    }


    
    public void Jump()
    {
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * jumpHeight);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit2D rayCastDown = Physics2D.Raycast(entity.groundCheck.position, new Vector2(0, -1));
            Collider2D bottomCollide = rayCastDown.collider;
            
            if (bottomCollide != null && rayCastDown.distance < 1)
            {
                Debug.Log(bottomCollide.gameObject.tag);
                if (bottomCollide.gameObject.tag == "Ground"){
                    body.velocity = new Vector2(body.velocity.x, entity.getJumpForce());
                    anim.SetBool("Jump", true);
                    anim.SetBool("Grounded", false);
                }
                else
                {
                    anim.SetBool("Grounded", true);
                }
            }
        }
        
        anim.SetFloat("AirSpeedY", body.velocity.y);
    }
    
    public void WallSlide()
    {
        if (sliding)
        {
            body.velocity = new Vector2(body.velocity.x, -1f);
        }

        //how much slower should a player fall if theyre against the wall
        float fallSpeed = -0.5f;

        if (Input.GetMouseButton(1))
        {
            RaycastHit2D hitSide = Physics2D.Raycast(body.position, new Vector2(body.velocity.x, 0));
            RaycastHit2D hitBottom = Physics2D.Raycast(body.position, new Vector2(0, -1));

            if (hitSide.collider != null)
            {
                //these numbers define how close a player should be to a wall to count as a wall slide
                if (hitSide.distance < 0.5 && (hitBottom.distance > 1 || hitBottom.collider == null))
                {
                    sliding = true;
                    body.velocity = new Vector2(body.velocity.x, fallSpeed);
                    anim.SetBool("WallSlide", true);
                    anim.SetFloat("AirSpeedY", body.velocity.y);
                    anim.SetBool("Run", false);
                    Debug.Log("SLIDING!");
                }
                else
                {
                    sliding = false;
                    anim.SetBool("WallSlide", false);
                    anim.SetBool("Grounded", true);

                }
            }
            {

            }
        } else
        {
            sliding = false;
            anim.SetBool("WallSlide", false);

        }
    }


    public void Move()
    {
       
        float movementX = Input.GetAxisRaw("Horizontal") * entity.getSpeed();
        body.velocity = new Vector2(movementX, body.velocity.y);

        if (body.velocity.x < 0) render.flipX = true;
        else render.flipX = false;

        if (body.velocity.x != 0) anim.SetBool("Run", true);
        else anim.SetBool("Run", false);
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
