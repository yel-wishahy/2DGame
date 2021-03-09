using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class Dialogue
{
    private Dictionary<string, Dictionary<string, string>> dialogue;
    private int currentLineNumber = 1;
    private Dictionary<string, string> currentLine = null;

    public Dictionary<string, Dictionary<string, string>> DialogueDictionary
    {
        get => new Dictionary<string, Dictionary<string, string>>(dialogue);
    }

    public Dialogue(Dictionary<string, Dictionary<string, string>> dialogue)
    {
        this.dialogue = dialogue;
    }

    public Dialogue(TextAsset JSONFILE)
    {
        dialogue = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(JSONFILE.text);
    }

    public Dialogue(string JSONString)
    {
        dialogue = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(JSONString);
    }

    public Dictionary<string, string> NextLine()
    {
        string key = GetKey();
        if (dialogue.ContainsKey(key))
        {
            currentLineNumber++;
            return dialogue[key];
        }

        return null;
    }

    private string GetKey()
    {
        string key = "";
        if (currentLineNumber < 10)
            key = "00" + currentLineNumber;
        else if (currentLineNumber < 100)
            key = "0" + currentLineNumber;
        else
            key = currentLineNumber.ToString();

        return key;
    }
}
