using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFST : MonoBehaviour
{

    public enum States{
        Seeking,
        Attack
    }

    public States CurrentState = States.Seeking;

    Vector2 enemyVector;

    Entity enemyEntity;

    Vector2 currentVtr;

    Entity currentEntity;

    public int Direction = 1;
    public int DamageDealt = 6;

    float TimeUnit = 0;

    public float AttackInterval = 1.5f;

    void OnTriggerStay2D(Collider2D enemy)
    {
        print(enemy.tag);
        if (enemy.tag == "Player" && enemy.GetComponent<Entity>() != null)
        {
            enemyEntity = enemy.GetComponent<Entity>();

            if (enemyEntity != null && enemyEntity.Health > 0)
            {
                CurrentState = States.Attack;
                enemyVector = enemy.transform.position;
                currentEntity.AttackMode = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D enemy)
    {
        if (enemy.tag == "Player")
        {
            currentEntity.alternativeX = 0;
            currentEntity.alternativeY = 0;
            currentEntity.Attacking = false;
            currentEntity.AttackMode = false;
            currentVtr = currentEntity.transform.position;
            CurrentState = States.Seeking;
        }
    }
    
    // Use this for initialization
    void Start()
    {
        currentEntity = GetComponentInParent<Entity>();
        currentVtr = currentEntity.transform.position;
        currentEntity.alternativeX = Direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEntity.Health > 0)
        {
            if (TimeUnit < AttackInterval)
                TimeUnit += Time.deltaTime;

            if (CurrentState == States.Seeking)
            {
                if (currentEntity.alternativeX == 0)
                    currentEntity.alternativeX = Direction;

                if (currentEntity.transform.position.x > currentVtr.x + 4)
                {
                    currentEntity.alternativeX = -1;
                }
                else if (currentEntity.transform.position.x < currentVtr.x - 4)
                {
                    currentEntity.alternativeX = 1;
                }
            }
            else if (CurrentState == States.Attack)
            {
                print((enemyVector.x - currentEntity.transform.position.x).ToString() + ", " + (enemyVector.y - currentEntity.transform.position.y).ToString());
                if (enemyVector.x - currentEntity.transform.position.x > 1.1f)
                {
                    currentEntity.alternativeX = 1;
                }
                else if (enemyVector.x - currentEntity.transform.position.x < -1.1f)
                {
                    currentEntity.alternativeX = -1;
                }
                else if (enemyVector.y - currentEntity.transform.position.y > 1.1f)
                {
                    currentEntity.alternativeY = 1;
                }
                else if (enemyEntity != null && !enemyEntity.Hurt)
                {
                    currentEntity.alternativeX = 0;

                    currentEntity.Attacking = true;

                    if (currentEntity.AddDamage && TimeUnit > AttackInterval)
                    {
                        enemyEntity.Hurt = true;

                        if (enemyEntity.Health > 0)
                            enemyEntity.Health -= DamageDealt;
                        else
                            enemyEntity.Health = 0;

                        TimeUnit = 0;
                    }

                }
            }
        }
    }
}
