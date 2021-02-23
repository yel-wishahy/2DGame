using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager
{
    Queue<string> CurrentDialog = null;

    public void AddStoryElement(string element)
    {
        CurrentDialog.Enqueue(element);
    }

    public string GetCurrentNode()
    {
        return CurrentDialog.Peek();
    }

    public void PopStoryElement()
    {
        CurrentDialog.Dequeue();
    }

    public StoryManager(string[] set)
    {
        if (CurrentDialog == null)
            CurrentDialog = new Queue<string>();

        try
        {
            foreach (string element in set)
            {
                CurrentDialog.Enqueue(element);
            }
        }
        catch { }
    }

    public StoryManager()
    {
        if (CurrentDialog == null)
            CurrentDialog = new Queue<string>();
    }

};
