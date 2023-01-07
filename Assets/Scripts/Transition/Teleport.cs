using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public SceneName sceneFrom;
    public SceneName sceneToGO;

    public void TeleportToScene()
    {
        TransitionManager.Instance.Transition(sceneFrom.ToString(), sceneToGO.ToString(), false);
    }
}
