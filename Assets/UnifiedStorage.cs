using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnifiedStorage
{
    public static PlayerStats PlayerModel = null;
    public static Hashtable PossibleAchievements = new Hashtable() {
            {GenerateHash("NULL"), "NULL"},
            {GenerateHash("Player = nullptr;"), "Player = nullptr;"}
        };
    public static Queue<KeyValuePair<int, string>> PendingAchievements = new Queue<KeyValuePair<int, string>>();

    public static int GenerateHash(string Description)
    {
        int hashVal = 0;

        // Generate a numerical hash identifier for the string as a 32 bit signed int.
        foreach (char c in Description)
        {
            hashVal += c;
        }

        return hashVal;
    }
    public static KeyValuePair<int, string> GenerateAchievementPair(string Description)
    {
        int hashVal = GenerateHash(Description);

        KeyValuePair<int, string> pair = new KeyValuePair<int, string>(hashVal, Description);

        return pair;
    }
}
