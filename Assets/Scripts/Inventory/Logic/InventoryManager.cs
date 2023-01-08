using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singletion<InventoryManager>
{
    public ItemDataList_SO itemData;
    public Dictionary<ItemName,int> itemCountDict=new Dictionary<ItemName,int>();
    public void AddItem(ItemName itemName)
    {
        if (!itemCountDict.ContainsKey(itemName))
            itemCountDict.Add(itemName, 1);
        else
            itemCountDict[itemName]++;
        EventHandler.CallUpdateUIEvent(itemData.GetItemDetails(itemName), itemCountDict[itemName]);
    }

    private void OnEnable()
    {
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        EventHandler.ItemUsedEvent += OnItemUsedEvent;
    }

    private void OnDisable()
    {
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        EventHandler.ItemUsedEvent -= OnItemUsedEvent;
    }

    private void OnStartNewGameEvent(string level)
    {
        itemCountDict.Clear();
        EventHandler.CallUpdateUIEvent(null, -1);
    }

    private void OnItemUsedEvent(ItemName itemName)
    {
        itemCountDict[itemName] --;
        if (itemCountDict[itemName]==0)
            itemCountDict.Remove(itemName);

        if (itemCountDict.Count == 0)
            EventHandler.CallUpdateUIEvent(null,-1);
    }

}
