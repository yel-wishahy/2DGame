using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infector : MonoBehaviour
{
    public float InfectInterval = 2.5f; // Time before damage from debuff
    public float Duration = 50;
    public int MinimumHealth = 25;
    public float InfectDmg = 5f;

    public string[] InfectTag = { "NPC", "Player", "MainCharacter" };

    public LayerMask DefaultLayer = 1;
    public float SeekRadius = 2.0f;

    UEntity currEntity;
    bool damage = false;
    bool active = true;
    float currentTime = 0.0f;

    Transform body;
    
    // Start is called before the first frame update
    void Start()
    {
        currEntity = GetComponent<UEntity>();

        foreach (string comp in InfectTag)
        {
            if (this.gameObject.tag == comp)
            {
                damage = true;
                break;
            }
        }

        body = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            Collider2D[] possibleEnemies = Physics2D.OverlapCircleAll(body.position, SeekRadius, DefaultLayer);

            print("C: " + possibleEnemies);

            foreach (Collider2D entity in possibleEnemies)
            {
                if (entity.gameObject.GetComponent<Infector>() == null)
                    entity.gameObject.AddComponent<Infector>();

                entity.gameObject.GetComponent<Infector>().InfectInterval = InfectInterval;
                entity.gameObject.GetComponent<Infector>().InfectDmg = InfectDmg;
                entity.gameObject.GetComponent<Infector>().InfectTag = InfectTag;
                entity.gameObject.GetComponent<Infector>().Duration = Duration;
                entity.gameObject.GetComponent<Infector>().MinimumHealth = MinimumHealth;

                entity.gameObject.GetComponent<Infector>().DefaultLayer = DefaultLayer;
                entity.gameObject.GetComponent<Infector>().SeekRadius = SeekRadius;

                active = false;
            }
        }

        if (damage)
        {
            if (Duration > 0)
            {
                if (currentTime < InfectInterval)
                {
                    currentTime += Time.deltaTime;
                    Duration -= Time.deltaTime;
                }
                else
                {
                    if (currEntity.Health > MinimumHealth)
                        currEntity.setHealth(currEntity.Health - InfectDmg);

                    currentTime = 0;
                }
            }
            else
            {
                Destroy(this);
            }
        }
    }
}
