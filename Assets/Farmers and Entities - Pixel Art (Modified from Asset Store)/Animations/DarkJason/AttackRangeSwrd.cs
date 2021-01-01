using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeSwrd : MonoBehaviour
{
    public Entity currentEntity;
    public int Damage = 3;
    // Start is called before the first frame update
    void OnTriggerStay2D(Collider2D enemy)
    {
        print(enemy.tag);
        if (enemy.tag == "GameController")
        {
            Entity enemyEntity = enemy.GetComponent<Entity>();
            if (enemyEntity != null && enemyEntity.Health > 0 && currentEntity.Health > 0 && !enemyEntity.Hurt)
            {
                if (currentEntity.AddDamage)
                {
                    enemyEntity.Hurt = true;
                    enemyEntity.Health -= Damage;
                }
            }
        }
    }
}
