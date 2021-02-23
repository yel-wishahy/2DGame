using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DataLoader : MonoBehaviour
{
    string persistPath;
    public bool DisplayError;
    public bool LoadData = true;
    public GameObject NewComer;
    public GameObject Menu;

    public int remotestatus = 0;
    // Start is called before the first frame update

    public bool WebAppInstalled(bool install, string rootPath = "")
    {
        if (!install)
        {
            if (File.Exists(rootPath + "/installed.dat"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            using (BinaryReader reader = new BinaryReader(File.Open(rootPath + "/installed.dat", FileMode.Create))) { }
            return true;
        }
    }

    /*
    void PopulateLB(string jsonTxt)
    {
        // [{"user":"simonfraser","score":"28941","uid":"2056863347"},{"user":"Player","score":"0","uid":"1224736103"}]

        // Parse the formatted json data into PlayerStats leaderboard format.
        int bracketstage = 0;
        int chunkstage = 0;
        int iterator = 0;

        string Name = "";
        string Level = "";
        string Score = "";
        string Unlockables = "";
        string UID = "";

        UnifiedStorage.RegisteredUIDs.Clear();
        UnifiedStorage.RegisteredNames.Clear();

        for (int i = 0; i < jsonTxt.Length; i++)
        {
            if (jsonTxt[i] == '{')
                bracketstage++;

            if (jsonTxt[i] == '}')
            {
                bracketstage--;

                if (bracketstage == 0) // Each unit must be properly balance or no storage/use will occur. (In case of data corruption)
                {
                    chunkstage = 0;
                    iterator = 0;

                    int resultS = 0;
                    int resultU = 0;
                    int resultL = 0;
                    int resultUL = 0;

                    bool temp = Int32.TryParse(Score, out resultS);
                    temp = Int32.TryParse(UID, out resultU);
                    temp = Int32.TryParse(Level, out resultL);
                    temp = Int32.TryParse(Unlockables, out resultUL);

                    PlayerStats newUser = new PlayerStats();

                    newUser.SetUser(Name);
                    newUser.SetUID(resultU);
                    newUser.SetLevel(resultL);
                    newUser.SetUnlockables(resultUL);
                    newUser.SetScore(resultS);

                    UnifiedStorage.RegisteredUIDs.Add(resultU, Name);
                    UnifiedStorage.RegisteredNames.Add(Name);

                    Name = "";
                    Level = "";
                    Score = "";
                    Unlockables = "";
                    UID = "";

                    print("Extracted: " + newUser.GetUser() + " " + " " + newUser.GetLevel() + " " + newUser.GetScore() + " " + " " + newUser.GetUnlockables() + " " + newUser.GetUID());
                    // Rest is similar to Block-Align in terms of data management.
                    AddtoLeaderboards appender = new AddtoLeaderboards();
                    appender.AppendLeaderboards(newUser, UnifiedStorage.Leaderboards, true);

                    foreach (PlayerStats element in UnifiedStorage.Leaderboards)
                    {
                        if (UnifiedStorage.FavoriteUIDsHT.Contains(element.GetUID()))
                        {
                            AddtoLeaderboards appender2 = new AddtoLeaderboards();
                            appender.AppendLeaderboards(element, UnifiedStorage.Favourites, true);
                            appender = null; // free up memory when done.
                        }
                    }

                }
            }

            if (jsonTxt[i] == ':' && bracketstage == 1)
            {
                chunkstage++;
            }

            if (jsonTxt[i] == ',' && bracketstage == 1)
            {
                chunkstage--;

                if (chunkstage == 0)
                    iterator++;
            }

            if (chunkstage == 1)
            {
                if (iterator == 0)
                    if (jsonTxt[i] != '"' && jsonTxt[i] != ':' && jsonTxt[i] != ',')
                        Name += jsonTxt[i];

                if (iterator == 1)
                    if (jsonTxt[i] != '"' && jsonTxt[i] != ':' && jsonTxt[i] != ',')
                        Level += jsonTxt[i];

                if (iterator == 2)
                    if (jsonTxt[i] != '"' && jsonTxt[i] != ':' && jsonTxt[i] != ',')
                        Score += jsonTxt[i];

                if (iterator == 3)
                    if (jsonTxt[i] != '"' && jsonTxt[i] != ':' && jsonTxt[i] != ',')
                        Unlockables += jsonTxt[i];

                if (iterator == 4)
                    if (jsonTxt[i] != '"' && jsonTxt[i] != ':' && jsonTxt[i] != ',')
                        UID += jsonTxt[i];
            }
        }
    }

    */

    public void POSTRequest()
    {
        if (UnifiedStorage.PlayerModel.AuthMode(2) == 1)
            StartCoroutine(Upload());
    }

    public void GetRequest()
    {
       // StartCoroutine(Download());
    }

    public void DeleteRequest(int uid = 0)
    {
        if (uid == 0)
            StartCoroutine(Delete(UnifiedStorage.PlayerModel.GetUID()));
        else
            StartCoroutine(Delete(uid));
    }
    IEnumerator Upload()
    {
        WWWForm webForm = new WWWForm();
        System.Random rand = new System.Random();
        int hash = rand.Next(Int32.MaxValue);
        //   print(hash);

        webForm.AddField("user", UnifiedStorage.PlayerModel.GetUser());
        webForm.AddField("level", UnifiedStorage.PlayerModel.GetLevel());
        webForm.AddField("score", UnifiedStorage.PlayerModel.GetScore());
        webForm.AddField("unlockables", UnifiedStorage.PlayerModel.GetUnlockables());
        webForm.AddField("uid", UnifiedStorage.PlayerModel.GetUID());
        webForm.AddField("hash", hash);

        print("Uploading...");
        UnityWebRequest www = UnityWebRequest.Post("https://infiniteblockmdns.azurewebsites.net/append", webForm);
        www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            print(www.error);
        }
        else
        {
            print("Form upload complete!");
        }
    }

    IEnumerator Delete(int uid)
    {
        // Not part of leaderboards anymore.

        WWWForm webForm = new WWWForm();
        webForm.AddField("uid", uid);

        print("Deleting...");
        UnityWebRequest www = UnityWebRequest.Post("https://infiniteblockmdns.azurewebsites.net/delete", webForm);
        www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            print(www.error);
        }
        else
        {
            print("Deletion complete!");
            UnifiedStorage.PlayerModel.AuthMode(1); // Not part after deletion complete!
        }
    }

    /*
    IEnumerator Download()
    {
        print("Downloading...");
        UnityWebRequest www = UnityWebRequest.Get("https://infiniteblockmdns.azurewebsites.net/");
        www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            print(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            PopulateLB(www.downloadHandler.text);

            if (UnifiedStorage.RegisteredUIDs.Contains(UnifiedStorage.PlayerModel.GetUID()))
            {
                if ((UnifiedStorage.PlayerModel.AuthMode(2) != 2) && UnifiedStorage.PlayerModel.GetUser() != UnifiedStorage.RegisteredUIDs[UnifiedStorage.PlayerModel.GetUID()].ToString())
                    UnifiedStorage.PlayerModel.AuthMode(1);

                if ((UnifiedStorage.PlayerModel.AuthMode(2) == 2))
                {
                    UnifiedStorage.PlayerModel.GenerateUID();
                    if (!UnifiedStorage.RegisteredNames.Contains(UnifiedStorage.PlayerModel.GetUser()))
                        UnifiedStorage.PlayerModel.AuthMode(0);
                    else
                        UnifiedStorage.PlayerModel.AuthMode(1);
                }

            }
            else if (UnifiedStorage.RegisteredNames.Contains(UnifiedStorage.PlayerModel.GetUser()))
            {
                UnifiedStorage.PlayerModel.AuthMode(1);
            }
            else if (UnifiedStorage.PlayerModel.AuthMode(2) != -1)
            {
                UnifiedStorage.PlayerModel.AuthMode(0); // New unique user!
            }


        }
    }
    */

    // Use this for initialization

    void Loader(bool tutorial = true)
    {
        UnifiedStorage.ReadfromFile();

        if (tutorial)
        {
            if (UnifiedStorage.PlayerModel == null)
            {
                UnifiedStorage.PlayerModel = new PlayerStats("Player");
                UnifiedStorage.PlayerModel.SetUser(UnifiedStorage.PlayerModel.GetUID().ToString());
              //  UnifiedStorage.PlayerModel.SetUnlockables(UnifiedStorage.UnlockedModels.Count + UnifiedStorage.SelectableColours.Count);
                if (NewComer != null)
                    NewComer.SetActive(true);
            }
            else
            {
                if (Menu != null)
                    Menu.SetActive(true);
            }
        }
        else
        {

        }

    }

    void LoadStoryData()
    {
        StoryManager manager = UnifiedStorage.CurrentStoryProgress;

        manager.AddStoryElement("Welcome to Shallow World! My name is THE CORRUPTOR! My goal is to capitalize the whole kingdom and you shall be my servant,");
        manager.AddStoryElement("JASON!");
        manager.AddStoryElement("So what do you say?");
        manager.AddStoryElement("JASON: No! We will not allow you to monopolize our Kingdom! YOU have no idea how much our RICE FARM means to US!");
        manager.AddStoryElement("ADA: Yeah! You evil meanie!");
        manager.AddStoryElement("Shut up ADA. These people don't understand the meaning of our RICE! Especially the RICE BALLS that exist on our TREES.");
        manager.AddStoryElement("CORRUPTOR: This is your final warning!");
        manager.AddStoryElement("JASON throws ADA with RICE BALL in hand.");
        manager.AddStoryElement("CORRUPTOR: Ugh! YOU asked for it! Mutant zombie chickens, DAA! CWAY!! (Attack!)");
        manager.AddStoryElement("The game starts here.");
    }
    void Start()
    {
        persistPath = Application.persistentDataPath;

        UnifiedStorage.persistPath = persistPath;

        if (LoadData)
            Loader();

        LoadStoryData();
    }

    /*
    // Update is called once per frame
    void Update()
    {
        DisplayError = UnifiedStorage.ErrorStatus;
        
        if (UnifiedStorage.AccessMode == -1)
        {
            UnifiedStorage.AccessMode = 0;
            EraseData();
        }
    }
    */
}
