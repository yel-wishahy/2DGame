﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextAsset jsonDialogue;
    [SerializeField] private Text textPanel;
    [SerializeField] private Text characterNamePanel;
    [SerializeField] private float textScrollSpeed;

    private Dictionary<string, string> currentLine;

    private Dialogue dialogue;
    
    void Awake()
    {
        dialogue = new Dialogue(jsonDialogue);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentLine = dialogue.NextLine();
            characterNamePanel.text = currentLine["name"];
            
            StopAllCoroutines();
            StartCoroutine(DisplayText(currentLine["text"]));

        }
    }

    IEnumerator DisplayText(string text)
    {
        textPanel.text = "";

        foreach (char c in text.ToCharArray())
        {
            textPanel.text += c;
            yield return new WaitForSeconds(textScrollSpeed);
        }
    }
    
    
    
}
