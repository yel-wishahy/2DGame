using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class that displays UEntity HP, make sure to assign MainEntity in inspector 
public class DisplayHP : MonoBehaviour
{
    [SerializeField]
    public UEntity MainEntity;

    Slider slider;
    Text text;
    
    void Start()
    {
        text = GetComponent<Text>();
        slider = GetComponent<Slider>();
        slider.maxValue = MainEntity.Health;
    }
    // Update is called once per frame
    void Update()
    {
        text.text = "HP: " + MainEntity.Health;

        if (MainEntity.transform.localScale.x < 0)
            text.rectTransform.localScale = new Vector3(-1,1,1);
        else
            text.rectTransform.localScale = new Vector3(1,1,1);

        slider.value = MainEntity.Health;



    }
}
