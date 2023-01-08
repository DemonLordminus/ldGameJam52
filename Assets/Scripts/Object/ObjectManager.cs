using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ObjectManager : Singletion<ObjectManager>
{
    public Dictionary<string,bool> itemActiveDict=new Dictionary<string,bool>();
    private void OnEnable()
    {
        EventHandler.AfterSceneChangeEvent += OnAfterSceneChangeEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneChangeEvent -= OnAfterSceneChangeEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    private void OnStartNewGameEvent(int obj)
    {
        itemActiveDict.Clear();
    }

    private void OnAfterSceneChangeEvent()
    {
        foreach (var item in FindObjectsOfType<Item>())
        {
            Debug.Log(item.name + item.gameObject.activeInHierarchy);
            if (itemActiveDict.ContainsKey(item.name))
                item.gameObject.SetActive(itemActiveDict[item.name]);
            else
                itemActiveDict.Add(item.name, true);
        }
    }

    public void ItemChange(GameObject item)
    {
        if (itemActiveDict.ContainsKey(item.name))
            itemActiveDict[item.name] = false;
    }
}
