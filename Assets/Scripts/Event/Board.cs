using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Board : Tip
{
    public override void EventAction()
    {
        controller.ShowDialogueEmpty();
    }
}
