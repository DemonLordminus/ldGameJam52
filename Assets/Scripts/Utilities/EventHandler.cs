using System;
using UnityEngine;

public static class EventHandler
{
    public static event Action<string> StartNewGameEvent;
    public static void CallStartNewGameEvent(string level)
    {
        StartNewGameEvent?.Invoke(level);
    }

    public static event Action<string> ShowDialogueEvent;
    public static void CallShowDialogueEvent(string dialogue)
    {
        ShowDialogueEvent?.Invoke(dialogue);
    }

    public static event Action<GameState> GameStateChangedEvent;
    public static void CallGameStateChangerEvent(GameState gameState)
    {
        GameStateChangedEvent?.Invoke(gameState);
    }

    public static event Action BeforeSceneChangeEvent;
    public static void CallBeforeSceneChangeEvent()
    {
        BeforeSceneChangeEvent?.Invoke();
    }

    public static event Action AfterSceneChangeEvent;
    public static void CallAfterSceneChangeEvent()
    {
        AfterSceneChangeEvent?.Invoke();
    }

    public static event Action<ItemDetails,int> UpdateUIEvent;
    public static void CallUpdateUIEvent(ItemDetails itemDetails,int amount)
    {
        UpdateUIEvent?.Invoke(itemDetails,amount);
    }

    public static event Action<ItemName> ItemUsedEvent;
    public static void CallItemUsedEvent(ItemName itemName)
    {
        ItemUsedEvent?.Invoke(itemName);
    }
}


