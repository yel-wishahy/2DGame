using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public UEntity entity;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = entity.getHealth();
        
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = entity.getHealth();
        
        if (text != null)
        {
            text.text = slider.value + "/" + slider.maxValue;
        }
    }
}
