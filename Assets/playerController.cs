using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer render;
   

    public Vector3 range;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float speed, jumpVelocity, jumpHeight;
    
    


  
  


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();

    }
      
    // Update is called once per frame
    void Update()
    {

        Move();
        checkJumpCollision();
        
    }

    void Move()
    {
       
        float movementX = Input.GetAxisRaw("Horizontal") * speed;
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
        Collider2D bottomCollide = Physics2D.OverlapBox(groundCheck.position, range, 0, groundLayer);

        if (bottomCollide != null)
        {
          
            if (bottomCollide.gameObject.tag == "Ground" && Input.GetKeyDown(KeyCode.Space)){
                body.velocity = new Vector2(body.velocity.x, jumpVelocity);
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


    

    //testCode
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(groundCheck.position, range);
    }


}
