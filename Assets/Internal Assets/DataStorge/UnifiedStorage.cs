using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public static class UnifiedStorage
{
    public static string persistPath;
    public static PlayerStats PlayerModel = null;
    public static Hashtable PossibleAchievements = new Hashtable() {
            {GenerateHash("NULL"), "NULL"},
            {GenerateHash("Player = nullptr;"), "Player = nullptr;"}
        };
    public static Queue<KeyValuePair<int, string>> PendingAchievements = new Queue<KeyValuePair<int, string>>();

    public static List<GameObject> ListofItems = new List<GameObject>();
    public static int ControlFormat = 0;
    public static float Sensitivity = 0.75f;

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


    public static int _WriteToDisk(int mode = 0, string rootPath = "")
    {
        int status = 0;
        // Uploads user data to server. The server handles all of the storage and processing! We do just the front-end.

        // Writes data to disk.
        if (UnifiedStorage.PlayerModel != null)
        {
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(rootPath + "/playerdata.udat", FileMode.Create)))
                {
                   // MonoBehaviour.print("DATAW+: " + UnifiedStorage.PlayerModel.ToString() + " Path: " + rootPath);
                    writer.Write(UnifiedStorage.PlayerModel.GetUser());
                    writer.Write(UnifiedStorage.PlayerModel.GetUID());
                    writer.Write(UnifiedStorage.PlayerModel.GetMessage());
                    writer.Write(UnifiedStorage.PlayerModel.GetScore());
                    writer.Write(UnifiedStorage.PlayerModel.GetLevel());
                    writer.Write(UnifiedStorage.PlayerModel.DisplayLives());
                  //  MonoBehaviour.print("GCW: " + UnifiedStorage.PlayerModel.ViewCoins());
                    writer.Write(UnifiedStorage.PlayerModel.ViewCoins());
                  //  MonoBehaviour.print("COPPASTATE: " + UnifiedStorage.PlayerModel.AuthMode(2));
                    writer.Write(UnifiedStorage.PlayerModel.AuthMode(2));

                    writer.Write(UnifiedStorage.ControlFormat);
                    writer.Write(UnifiedStorage.Sensitivity);

                }
            }
            catch (Exception) { }
        }
        try
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(rootPath + "/achievements.udat", FileMode.Create)))
            {
               // MonoBehaviour.print("A: " + UnifiedStorage.PlayerModel.OutputListAchievement().Count);
                foreach (DictionaryEntry data in UnifiedStorage.PlayerModel.OutputListAchievement())
                {
                    MonoBehaviour.print("DATAW2+: " + data.ToString() + " Path: " + rootPath);
                    writer.Write(((string)data.Value));
                    writer.Write(((int)data.Key));
                }

            }
        }
        catch (Exception) { }

        return status;
    }

    public static int _ReadfromFile(int mode = 0, string rootPath = "")
    {
        int status = 0;

        if (File.Exists(rootPath + "/playerdata.udat") && mode == 0)
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(rootPath + "/playerdata.udat", FileMode.Open)))
                {
                    try
                    {
                        PlayerStats newUser = new PlayerStats(); // allocate a new user (temp)
                        newUser.SetUser(reader.ReadString());
                        newUser.SetUID(reader.ReadInt32());
                        newUser.SetMessage(reader.ReadString());
                        newUser.SetScore(reader.ReadInt32());
                        newUser.SetLevel(reader.ReadInt32());
                        newUser.SetLives(reader.ReadInt32());
                        newUser.SetCoins(reader.ReadInt32());
                        MonoBehaviour.print("GCW2a: " + newUser.ViewCoins());

                        int coppabt = reader.ReadInt32();
                        if (coppabt == -1)
                        {
                            newUser.AuthMode(1);
                        }

                        if (coppabt == 1)
                        {
                            newUser.AuthMode(0);
                        }

                        MonoBehaviour.print("DATAR+: " + newUser.ToString());
                        if (newUser != null)
                        {
                            MonoBehaviour.print("Read successfully");
                            UnifiedStorage.PlayerModel = newUser; // invalid memory location?
                        }

                        // Settings data read here

                        UnifiedStorage.ControlFormat = reader.ReadInt32();
                        UnifiedStorage.Sensitivity = reader.ReadSingle();

                    }
                    catch (EndOfStreamException)
                    {

                    }
                }
            }
            catch (Exception) { }
        }

        if (File.Exists(rootPath + "/achievements.udat"))
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(rootPath + "/achievements.udat", FileMode.Open)))
                {
                    bool EOF = false;
                    while (!EOF)
                    {
                        try
                        {
                            string des = reader.ReadString();
                            int val = reader.ReadInt32();

                            MonoBehaviour.print("ACHIEVEMENT: " + des + ": " + val);
                            if (!UnifiedStorage.PlayerModel.OutputListAchievement().Contains(val))
                            {
                                UnifiedStorage.PlayerModel.InsertNewAchievement(des);
                            }

                        }
                        catch (EndOfStreamException)
                        {
                            EOF = true;
                        }
                    }

                }
            }
            catch (Exception) { }
        }

        return status;
    }


    public static void ReadfromFile()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            Thread callback = new Thread(() => _ReadfromFile(0, persistPath));
            callback.Start();
        }
        else
        {
            _ReadfromFile(0, persistPath);
        }

        //  GetRequest();
    }

    public static void WriteToDisk()
    {
        //  GetRequest();
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            Thread callback = new Thread(() => _WriteToDisk(0, persistPath));
            callback.Start();
        }
        else
        {
            _WriteToDisk(0, persistPath);
        }
        //   POSTRequest();
        ReadfromFile();
    }

    public static void WriteDiskForce()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            Thread callback = new Thread(() => _WriteToDisk(0, persistPath));
            callback.Start();
        }
        else
        {
            _WriteToDisk(0, persistPath);
        }

        ReadfromFile();
    }

    public static void EraseData()
    {

        if (UnifiedStorage.PlayerModel != null && UnifiedStorage.PlayerModel.AuthMode(2) == 1)
        {
            if (_ReadfromFile() == 0 && Application.internetReachability != NetworkReachability.NotReachable)
            {
                /*
                for (int i = 0; i < UnifiedStorage.Leaderboards.Count; i++)
                {
                    if (UnifiedStorage.Leaderboards[i].GetUID() == UnifiedStorage.PlayerModel.GetUID())
                    {
                        UnifiedStorage.Leaderboards.RemoveAt(i);

                        if (UnifiedStorage.RegisteredUIDs.Contains(UnifiedStorage.PlayerModel.GetUID()))
                        {
                            UnifiedStorage.RegisteredUIDs.Remove(UnifiedStorage.PlayerModel.GetUID());
                        }
                    }
                }
                */
            }
        }
        else // We assume that if a user is not enrolled in leaderboards, that we delete it locally without any impact on global leaderboards (no garbage data)
        {
            UnifiedStorage.PlayerModel = null;

            try
            {
                File.Delete(persistPath + "/playerdata.udat");
                File.Delete(persistPath + "/leaderboards.udat");
                File.Delete(persistPath + "/localscores.udat");
            }
            catch (Exception) { }

        }
    }


    public static void ChangePrivacyMode(int mode)
    {
        if (UnifiedStorage.PlayerModel != null && Application.internetReachability != NetworkReachability.NotReachable)
        {
            UnifiedStorage.PlayerModel.AuthMode(mode);
        }
    }
}
