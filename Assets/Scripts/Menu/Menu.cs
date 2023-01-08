using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
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

}
