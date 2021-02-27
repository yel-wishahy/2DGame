using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//a settings class for the player
public class PlayerSettings
{
    private Dictionary<string, KeyCode> settings;

    public PlayerSettings()
    {
        settings = new Dictionary<string, KeyCode>();
    }

    public void AddorUpdateSetting(string actionName, KeyCode keyCode)
    {
        if (settings.Keys.Contains(actionName))
            settings[actionName] = keyCode;
        else
            settings.Add(actionName, keyCode);
    }

    public void removeSetting(string actionName)
    {
        if (settings.Keys.Contains(actionName))
            settings.Remove(actionName);
    }

    public KeyCode GetSetting(string actionName)
    {
        if (settings.Keys.Contains(actionName))
            return settings[actionName];
        else
            return KeyCode.Mouse0;
    }
}
