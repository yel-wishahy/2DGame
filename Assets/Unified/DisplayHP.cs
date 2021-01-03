using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHP : MonoBehaviour
{
    public UEntity MainEntity;
    Text text;
    
    void Start()
    {
        text = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        text.text = "HP: " + MainEntity.getHealth();

        if (MainEntity.transform.localScale.x < 0)
            text.rectTransform.localScale = new Vector3(-1,1,1);
        else
            text.rectTransform.localScale = new Vector3(1,1,1);
    }
}
