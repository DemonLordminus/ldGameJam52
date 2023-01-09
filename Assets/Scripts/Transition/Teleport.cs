using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public SceneName sceneFrom;
    public SceneName sceneToGO;

    public void TeleportToScene()
    {
        GameManager.Instance.Level = sceneToGO.ToString();
        GameManager.Instance.ifStart = true;
        TransitionManager.Instance.Transition(sceneFrom.ToString(), sceneToGO.ToString(), false);
    }
}
