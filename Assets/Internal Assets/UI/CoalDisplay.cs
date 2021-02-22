using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoalDisplay : MonoBehaviour
{
    public Player player;
    public Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = "Coal Collected: " + player.getItemQuantity("Coal");
    }
}
