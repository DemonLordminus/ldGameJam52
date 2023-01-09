using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public SceneName sceneFrom;
    public SceneName sceneToGO;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q) && TransitionManager.Instance.amount > 0)
        {
            TransitionManager.Instance.amount --;
            TeleportToScene();
        }
    }

    public void TeleportToScene()
    {
        TransitionManager.Instance.Transition(sceneFrom.ToString(), sceneToGO.ToString(), false);
    }
}
