using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public interface Controller
{
    void Move();

    void Jump();

    void Attack();

    void Update();

    void Init();

    bool Hurt(bool Damaged);
    void OnTriggerStay2D(Collider2D object2D);
    void OnTriggerEnter2D(Collider2D object2D);
    void OnCollisionStay2D(Collision2D object2D);
    void OnTriggerExit2D(Collider2D object2D);
    void OnCollisionExit2D(Collision2D object2D);
    void OnCollisionEnter2D(Collision2D object2D);
}
