using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveLoadManager : Singletion<SaveLoadManager>
{
    private string jsonFolder;
    private List<ISaveable> saveableList=new List<ISaveable>();
    private Dictionary<string,GameSaveData> saveDataDict=new Dictionary<string,GameSaveData>();
    protected override void Awake()
    {
        base.Awake();
        jsonFolder = Application.persistentDataPath + "/SAVE/";
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneChangeEvent += OnAfterSceneChangeEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneChangeEvent -= OnAfterSceneChangeEvent;
    }

    private void OnAfterSceneChangeEvent()
    {
        string level = GameManager.Instance.Level;
        var resultPath = jsonFolder + ("data{0}.sav",level);
        if (GameManager.Instance.ifStart)
        {
            if (File.Exists(resultPath))
                Load(level);
            else
                Save(level);
            GameManager.Instance.ifStart = false;
        }
    }

    public void Register(ISaveable saveable)
    {
        saveableList.Add(saveable);
    }

    public void Save(string index)
    {
        saveDataDict.Clear();

        foreach(var saveable in saveableList)
        {
            saveDataDict.Add(saveable.GetType().Name,saveable.GenerateSaveData());
        }

        var resultPath = jsonFolder + ("data{0}.sav",index);

        var jsonData=JsonConvert.SerializeObject(saveDataDict,Formatting.Indented);

        if(!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }

        File.WriteAllText(resultPath,jsonData);
    }

    public void Load(string index)
    {
        var resultPath = jsonFolder + ("data{0}.sav",index);

        if (!File.Exists(resultPath)) return;

        var stringData=File.ReadAllText(resultPath);

        var jsonData=JsonConvert.DeserializeObject<Dictionary<string,GameSaveData>>(stringData);

        foreach(var saveable in saveableList)
        {
            saveable.RestoreGameData(jsonData[saveable.GetType().Name]);
        }
    }
}

