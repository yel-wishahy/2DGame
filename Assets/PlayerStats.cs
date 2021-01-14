using System.Collections;
using System.Collections.Generic;
using System; // #include <system.h>

public class PlayerStats
{
    private string Username = "User"; // 2bytes*len
    private int Score = 0; //4 bytes
    private int Lives = 5;
    private int Coins = 0; //4 bytes
    private int Level = 0;  //4 bytes
    private int Unlocked = 0; // 25+9 = 34 items total  //4 bytes
    private int UID = 0;  //4 bytes
    private HashSet<int> AchievementHashes = new HashSet<int>();
    private string Message = "This is a user."; // 2bytes*15
    private int Verified = 2; // To comply with "COPPA" // 2 - Initalized // 4 bytes

    // Total size: 2*18 + 4 + 4 + 4 + 4 + 2*15 + 4 = 86 bytes for worst case. Subtract 2*17 for best case (1 char input)
    // One server read entry: 112*2 bytes
    // Max cap on memory for WebGL = 512MB => 512000000 bytes
    // Initalizes the class
    public PlayerStats()
    {
        GenerateUID();
    }

    public PlayerStats(string User)
    {
        GenerateUID();
        Username = GetUID().ToString();    
    }

    public void AddScore()
    {
        Score++;
    }

    public int ViewScore()
    {
        return Score;
    }

    public void SetCoins(int coins)
    {
        Coins = coins;
    }

    public int ViewCoins()
    {
        return Coins;
    }

    public void SubtractScore()
    {
        Score--;
    }

    public void SetLives(int lives)
    {
        Lives = lives;
    }

    public void IncrementLives(int amount)
    {
        Lives += amount;
    }

    public int DisplayLives()
    {
        return Lives;
    }
    public string GetUser()
    {
        return Username;
    }

    public void SetUser(string user)
    {
        Username = user;
    }

    public void GenerateUID()
    {
        var rand = new System.Random();
        UID = rand.Next(0, int.MaxValue);

        if (UID <= 0)
            GenerateUID();
    }

    public void SetUID(int newUID)
    {
        UID = newUID;
    }

    public int GetUID()
    {
        return UID;
    }

    public void InsertNewAchievement(string ach)
    {
        int hashVal = 0;

        // Generate a numerical hash identifier for the string as a 32 bit signed int.
        foreach (char c in ach)
        {
            hashVal += c;
        }

        AchievementHashes.Add(hashVal);
    }

    public void RemoveAchievement(int hashVal)
    {
        if (AchievementHashes.Contains(hashVal))
            AchievementHashes.Remove(hashVal);
    }

    public HashSet<int> OutputListAchievement()
    {
        return AchievementHashes;
    }

    
    public int GetScore()
    {
        return ScoreEncryptor(1, Score);
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public string GetMessage()
    {
        return Message;
    }

    public void SetMessage(string MessageData)
    {
        Message = MessageData;
    }

    public int AuthMode(int mode)
    {
        if (mode == 0)
            Verified = 1;
        if (mode == 1)
            Verified = -1;

        return Verified;
    }

    public int ScoreEncryptor(int mode, int score = 0)
    {
        int hash = 0;

        if (mode == 0)
        {
            foreach (char ind in Username)
            {
                hash += (int)ind;
            }
            hash += UID;
            hash += score;
        }
        else
        {
            hash = score;

            foreach (char ind in Username)
            {
                hash -= (int)ind;
            }


            if (UID != 0)
            {
                hash -= UID;
            }
        }

        if (hash > 0)
            return hash;
        else
            return 0;
    }

    public void SetScore(int newScore)
    {
        Score = ScoreEncryptor(0, newScore);
    }

    public void SetLevel(int newLevel)
    {
        Level = newLevel;
    }

    public int GetLevel()
    {
        return Level;
    }

    public void SetUnlockables(int newUnlocked)
    {
        Unlocked = newUnlocked;
    }

    public int GetUnlockables()
    {
        return Unlocked;
    }
};
