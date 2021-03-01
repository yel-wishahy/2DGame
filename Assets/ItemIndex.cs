using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemIndex : MonoBehaviour
{
    public GameObject[] Items;
    public string SceneToLoad = "Main";
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject item in Items)
        {
            UnifiedStorage.ListofItems.Add(item);
        }

        SceneManager.LoadScene(SceneToLoad);
    }
}
