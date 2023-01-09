using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fruit : Item
{

    public override void ItemClick()
    {
        TransitionManager.Instance.amount+=3;
        TransitionManager.Instance.AmountChange();
        this.gameObject.SetActive(false);
    }
}
