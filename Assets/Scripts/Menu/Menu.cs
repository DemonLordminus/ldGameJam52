using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : Singletion<Menu>
{
    public void Restart()
    {
        GameManager.Instance.MenuReset();
        EventHandler.CallStartNewGameEvent(GameManager.Instance.Level);
    }

    public void Back()
    {
        GameManager.Instance.MenuReset();
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

    public void ContinueGame(int index)
    {
        GameManager.Instance.MenuReset();
        SaveLoadManager.Instance.Load(index);
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }
}
