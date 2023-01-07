using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : Singletion<GameManager>,ISaveable
{
    public float gameTime;
    public GameObject timer;
    public GameObject menu;
    public Light2D globalLight;

    private void Start()
    {
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    private void LateUpdate()
    {
        TimeMgr.SetCurTime();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (menu.activeInHierarchy)
            {
                Time.timeScale = 1;
                MenuReset();
            }
            else
                Time.timeScale = 0;
            menu.SetActive(!menu.activeInHierarchy);
        }
    }

    public void MenuReset()
    {
       
    }

    public void Timer(float time)
    {
        timer.SetActive(true);
        StartCoroutine(TimerReset(time));
    }

    private IEnumerator TimerReset(float time)
    {
        Text text = timer.GetComponent<Text>();
        while (time > float.Epsilon)
        {
            text.text = ((int)time).ToString();
            time--;
            yield return new WaitForSeconds(1);
        }
        timer.SetActive(false);
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData savedata = new GameSaveData();
        savedata.globalLightColorA = globalLight.color.a;
        savedata.globalLightColorR = globalLight.color.r;
        savedata.globalLightColorG = globalLight.color.g;
        savedata.globalLightColorB = globalLight.color.b;
        return savedata;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        globalLight.color = new Color(saveData.globalLightColorR, saveData.globalLightColorG, saveData.globalLightColorB, saveData.globalLightColorA);
    }
}
