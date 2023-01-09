using playerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : Singletion<DialogueUI>
{
    public GameObject panel;
    public Text dialogueText;

    private void ShowDialogue( string dialogue)
    {
        if (dialogue != string.Empty)
            panel.SetActive(true);
        else
            panel.SetActive(false);
        dialogueText.text = dialogue;
    }

    private void OnEnable()
    {
        EventHandler.ShowDialogueEvent += ShowDialogue;
        EventHandler.AfterSceneChangeEvent += OnAfterSceneChangeEvent;
    }

    private void OnDisable()
    {
        EventHandler.ShowDialogueEvent -= ShowDialogue;
        EventHandler.AfterSceneChangeEvent -= OnAfterSceneChangeEvent;
    }

    private void OnAfterSceneChangeEvent()
    {
        ShowDialogue(string.Empty);
    }
}
