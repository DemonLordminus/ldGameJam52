using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectManager : Singletion<ObjectManager>
{
    public List<GameObject> itemlist = new List<GameObject>();
    public List<List<GameObject>> list = new List<List<GameObject>>();
    public Dictionary<string, bool> itemActiveDict = new Dictionary<string, bool>();

    public Dictionary<string, bool> interactiveIsDoneDict = new Dictionary<string, bool>();

    private void OnEnable()
    {
        EventHandler.BeforeSceneChangeEvent += OnBeforeSceneChangeEvent;
        EventHandler.AfterSceneChangeEvent += OnAfterSceneChangeEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneChangeEvent -= OnBeforeSceneChangeEvent;
        EventHandler.AfterSceneChangeEvent -= OnAfterSceneChangeEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    private void OnBeforeSceneChangeEvent()
    {
        var interactives = FindObjectsOfType<Interactive>();
        if (interactives == null)
            return;
        foreach (var interactive in interactives)
        {
            if (interactiveIsDoneDict.ContainsKey(interactive.name))
            {
                Debug.Log(interactive.name + interactive.isDone);
                interactiveIsDoneDict[interactive.name] = interactive.isDone;
            }
            else
            {
                interactiveIsDoneDict.Add(interactive.name, interactive.isDone);
            }
        }
    }

    private void OnStartNewGameEvent(string level)
    {
        itemActiveDict.Clear();
        itemlist.Clear();
        interactiveIsDoneDict.Clear();
        int index = level switch { "Level1" => 0, "Level2" => 1, _ => 0 };
        foreach (GameObject item in list[index])
            item.SetActive(true);
    }

    private void OnAfterSceneChangeEvent()
    {
        foreach (var item in FindObjectsOfType<Item>())
        {
            if (itemActiveDict.ContainsKey(item.name))
            {
                item.gameObject.SetActive(itemActiveDict[item.name]);
            }
            else
            {
                itemActiveDict.Add(item.name, true);
                itemlist.Add(item.gameObject);
            }
        }
        list.Add(itemlist);

        foreach (var interactive in FindObjectsOfType<Interactive>())
        {
            if (interactiveIsDoneDict.ContainsKey(interactive.name))
            {
                interactive.isDone = interactiveIsDoneDict[interactive.name];
                if (interactive.isDone)
                {
                    interactive.confiner.SetActive(false);
                    interactive.Check();
                }
            }
            else
            {
                interactiveIsDoneDict.Add(interactive.name, interactive.isDone);
            }
        }
    }

    public void ItemChange(GameObject item)
    {
        if (itemActiveDict.ContainsKey(item.name))
            itemActiveDict[item.name] = false;
    }
}
