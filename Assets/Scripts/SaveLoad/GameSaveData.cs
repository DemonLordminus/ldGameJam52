using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveData 
{
    public string currentScene;
    
    public Dictionary<string, float> characterPositionXDict = new Dictionary<string, float>();
    public Dictionary<string, float> characterPositionYDict = new Dictionary<string, float>();

    public float globalLightColorR;
    public float globalLightColorG;
    public float globalLightColorB;
    public float globalLightColorA;

    public Dictionary<string, bool> itemActiveDict = new Dictionary<string, bool>();
}
