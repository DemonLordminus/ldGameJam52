using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAmount : MonoBehaviour
{
    public Text itemAmountText;

    public void UpdateItemAmount(int amount)
    {
        itemAmountText.text = amount.ToString();
    }
}
