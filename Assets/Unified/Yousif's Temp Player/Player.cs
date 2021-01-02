using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UEntity
{
    [SerializeField]
    public float attackRadius = 0.8f;
    public Vector3 range = new Vector3(1, 0.2f, 0);
    public Transform groundCheck, attack;
    public LayerMask groundLayer, enemyLayer;

    //override controller property with player controller class

    public override Controller UserController => new playerController(this);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(attack.position, attackRadius);
    }


}
