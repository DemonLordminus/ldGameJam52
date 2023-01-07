using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformManager : Singletion<TransformManager>, ISaveable
{
    public Dictionary<string, float> characterPositionXDict = new Dictionary<string, float>();
    public Dictionary<string, float> characterPositionYDict = new Dictionary<string, float>();

    private void OnEnable()
    {
        EventHandler.BeforeSceneChangeEvent += OnBeforeSceneChangeEvent;
        EventHandler.AfterSceneChangeEvent += OnAfterSceneChangeEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneChangeEvent -= OnBeforeSceneChangeEvent;
        EventHandler.AfterSceneChangeEvent -= OnAfterSceneChangeEvent;
    }

    private void OnAfterSceneChangeEvent()
    {
        foreach (var character in FindObjectsOfType<Character>())
        {
            if (characterPositionXDict.ContainsKey(character.name))
            {
                character.transform.position = new Vector3(characterPositionXDict[character.name], characterPositionYDict[character.name]);
            }
        }
    }

    private void OnBeforeSceneChangeEvent()
    {
        foreach (var character in FindObjectsOfType<Character>())
        {
            if (characterPositionXDict.ContainsKey(character.name))
            {
                characterPositionXDict[character.name] = character.transform.position.x;
                characterPositionYDict[character.name] = character.transform.position.y;
            }
            else
            {
                characterPositionXDict.Add(character.name, character.transform.position.x);
                characterPositionYDict.Add(character.name, character.transform.position.y);
            }
        }
    }

    private void Start()
    {
        characterPositionXDict.Clear();
        characterPositionYDict.Clear();
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.characterPositionXDict = characterPositionXDict;
        saveData.characterPositionYDict = characterPositionYDict;
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        characterPositionXDict = saveData.characterPositionXDict;
        characterPositionYDict = saveData.characterPositionYDict;
    }
}
