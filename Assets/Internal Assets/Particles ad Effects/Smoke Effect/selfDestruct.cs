using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestruct : MonoBehaviour
{
    private float internalTimer;

    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        internalTimer = Time.time + timer;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > internalTimer)
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(this);
        }
        
    }
}
