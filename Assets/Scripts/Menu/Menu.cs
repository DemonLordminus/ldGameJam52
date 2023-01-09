using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Menu : MonoBehaviour
{
    public Button Level1;
    public Button Level2;
    private string jsonFolder;

    private void Awake()
    {
        jsonFolder = Application.persistentDataPath + "/SAVE/";
        var Level2Path= jsonFolder + "(data{0}.sav, Level2)";
        Level2.interactable = false;
        if(File.Exists(Level2Path))
            Level2.interactable = true;
    }

    public void Select(string level)
    {
        GameManager.Instance.Level = level;
        GameManager.Instance.ifStart = true;
        TransitionManager.Instance.Transition(SceneManager.GetActiveScene().name, level, false);
    }

    public void Restart()
    {
        GameManager.Instance.ifStart = true;
        TransitionManager.Instance.Transition(SceneManager.GetActiveScene().name, GameManager.Instance.Level, false);
    }

    public void Back()
    {
        TransitionManager.Instance.Transition(SceneManager.GetActiveScene().name, "Start", false);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();                          
#endif
    }

    public void Check()
    {
        var Level2Path = jsonFolder + "(data{0}.sav, Level2)";
        Level2.interactable = false;
        if (File.Exists(Level2Path))
            Level2.interactable = true;
    }

    private void OnEnable()
    {
        Check();
    }
}
