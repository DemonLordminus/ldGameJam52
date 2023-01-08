using playerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : Singletion<GameManager>, ISaveable
{
    public float gameTime;
    public GameObject timer;
    public GameObject menu;
    public Light2D globalLight;
    public string Level;
    public bool ifStart;
    public Player player;

    private void OnEnable()
    {
        EventHandler.BeforeSceneChangeEvent += OnBeforeSceneChangeEvent;
        EventHandler.AfterSceneChangeEvent += OnAfterSceneChangeEvent;
        EventHandler.UpdateUIEvent += OnUpdateUIEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneChangeEvent -= OnBeforeSceneChangeEvent;
        EventHandler.AfterSceneChangeEvent -= OnAfterSceneChangeEvent;
        EventHandler.UpdateUIEvent -= OnUpdateUIEvent;
    }

    private void OnUpdateUIEvent(ItemDetails itemDetails, int amount)
    {
        if (itemDetails == null)
        {
            player.holdItem = false;
            player.currentItem=ItemName.None;
        }
        else
        {
            player.holdItem = true;
            player.currentItem = itemDetails.itemName;
        }
    }

    private void OnAfterSceneChangeEvent()
    {
        menu.SetActive(false);
        player = FindObjectOfType<Player>();
    }

    private void OnBeforeSceneChangeEvent()
    {
        MenuReset();
        menu.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Start()
    {
        MenuReset();
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
                Time.timeScale = 1f;
                MenuReset();
            }
            else
                Time.timeScale = 0f;
            menu.SetActive(!menu.activeInHierarchy);
        }
    }

    public void MenuReset()
    {
        menu.transform.GetChild(0).gameObject.SetActive(true);
        menu.transform.GetChild(1).gameObject.SetActive(false);
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
