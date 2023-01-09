using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(DialogueController))]
[RequireComponent (typeof(CircleCollider2D))]
public class Tip : Event
{
    protected DialogueController controller;
    public int upY;
    Coroutine Coroutine;
    public Item item;
    public Interactive interactive;

    private void Awake()
    {
        controller = GetComponent<DialogueController>();
        try
        {
            item = gameObject.transform.parent.gameObject.GetComponent<Item>();
            interactive = gameObject.transform.parent.gameObject.GetComponent<Interactive>();
        }
        catch { }
    }

    public override void EventAction()
    {
        if (item != null||interactive!=null)
        {
            controller.ShowDialogueItem();
            if (Coroutine != null)
            {
                StopCoroutine(Coroutine);
                Coroutine = null;
            }
            else
                Coroutine = StartCoroutine(UpdatePos());
            return;
        }
        controller.ShowDialogueItem();
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
            Coroutine = null;
        }
        else
            Coroutine = StartCoroutine(UpdatePos());
    }

    IEnumerator UpdatePos()
    {
        while (true)
        {
            Vector3 vector = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
            DialogueUI.Instance.gameObject.transform.position = vector + new Vector3(0, upY);
            yield return new WaitForFixedUpdate();
        }
    }
}
