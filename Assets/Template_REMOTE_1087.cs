using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class tempControl : Controller
{
    public void Attack()
    {
       MonoBehaviour.print("1");
    }

    public bool Hurt(bool Damaged)
    {
        return true;
    }

    public void Init()
    {
       MonoBehaviour.print("2");
    }

    public void Jump()
    {
       MonoBehaviour.print("3");
    }

    public void Move()
    {
       MonoBehaviour.print("4");
    }

    public void OnCollisionEnter2D(Collision2D collider)
    {
       MonoBehaviour.print("5");
    }

    public void OnCollisionExit2D(Collision2D collider)
    {
       MonoBehaviour.print("6");
    }

    public void OnCollisionStay2D(Collision2D collider)
    {
       MonoBehaviour.print("7");
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
       MonoBehaviour.print("8");
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
       MonoBehaviour.print("9");
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
       MonoBehaviour.print("10");
    }

    public void Update()
    {
       MonoBehaviour.print("11");
    }
};
public class Template : UEntity
{
    // Complexity script
    public override Controller UserController => new tempControl();
    public override Controller AltController => new tempControl();
}
