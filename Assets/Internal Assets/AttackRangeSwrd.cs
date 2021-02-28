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
            UEntity enemyEntity = enemy.GetComponent<UEntity>();
            print(enemyEntity + "EE");
            if (enemyEntity != null && enemyEntity.Health > 0 && currentEntity.Health > 0)
            {
                if (currentEntity.AddDamage)
                {
                   // UnifiedStorage.WriteToDisk();
                    print(UnifiedStorage.Sensitivity);
                    enemyEntity.takeDamage(currentEntity.AttackDamage);
                }
            }
        }
    }
}
