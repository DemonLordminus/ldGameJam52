using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : Singletion<TransitionManager>, ISaveable
{
    public SceneName startScene;

    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration;
    public bool isFade;
    private bool canTransition = true;
    public int amount;
    public int level1Amount;
    public int level2Amount;
    public Image tp;
    public Text tpText;

    private void OnEnable()
    {
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        EventHandler.AfterSceneChangeEvent += OnAfterSceneChangeEvent;
    }

    private void OnDisable()
    {
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        EventHandler.AfterSceneChangeEvent += OnAfterSceneChangeEvent;
    }

    private void OnAfterSceneChangeEvent()
    {
        try
        {
            if (amount == 0)
            {
                tp.color = Color.gray;
                tpText.text = "无法传送";
            }
            else
            {
                AmountChange();
            }
        }
        catch { }
    }

    public void AmountChange()
    {
        tp.color = Color.white;
        tpText.text = "可传送次数" + " : " + amount;
    }

    private void OnStartNewGameEvent(string level)
    {
        amount = level switch { "Level1" => level1Amount, "Level2" => level2Amount, _ => 0 };
    }

    private void Start()
    {
        StartCoroutine(TransitionToScene("Persistent", startScene.ToString(), false));
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    public void Transition(string from, string to, bool ifNow)
    {
        if (!isFade && canTransition)
            StartCoroutine(TransitionToScene(from, to, ifNow));
    }

    private IEnumerator TransitionToScene(string from, string to, bool ifNow)
    {
        if (!ifNow)
            EventHandler.CallBeforeSceneChangeEvent();
        yield return Fade(1);
        if (from != string.Empty && from != "Persistent")
        {
            yield return SceneManager.UnloadSceneAsync(from);
        }
        yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);

        EventHandler.CallAfterSceneChangeEvent();
        yield return Fade(0);
    }

    /// <summary>
    /// 淡入淡出场景
    /// </summary>
    /// <param name="targetAlpha"></param>
    /// <returns></returns>
    private IEnumerator Fade(float targetAlpha)
    {
        isFade = true;

        fadeCanvasGroup.blocksRaycasts = true;
        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;

        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }

        fadeCanvasGroup.blocksRaycasts = false;

        isFade = false;
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.currentScene = SceneManager.GetActiveScene().name;
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        bool ifNow = SceneManager.GetActiveScene().name == saveData.currentScene;
        Transition(SceneManager.GetActiveScene().name, saveData.currentScene, ifNow);
    }

}
