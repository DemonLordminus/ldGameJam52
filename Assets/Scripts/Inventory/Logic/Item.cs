using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemName itemName;

    public void ItemClick()
    {
        ObjectManager.Instance.ItemChange(gameObject);
        InventoryManager.Instance.AddItem(itemName);
        this.gameObject.SetActive(false);
    }
}
