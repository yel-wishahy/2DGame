using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeSwrd : MonoBehaviour
{
    public Jason currentEntity;
    // Start is called before the first frame update
    void OnTriggerStay2D(Collider2D enemy)
    {
        print(enemy.tag);
        if (enemy.tag == "Enemy")
        {
            Jason enemyEntity = enemy.GetComponent<Jason>();
            if (enemyEntity != null && enemyEntity.getHealth() > 0 && currentEntity.getHealth() > 0 && !enemyEntity.Hurt)
            {
                if (currentEntity.AddDamage)
                {
                    enemyEntity.Hurt = true;
                    enemyEntity.takeDamage(currentEntity.getAttackDamage());
                }
            }
        }
    }
}
