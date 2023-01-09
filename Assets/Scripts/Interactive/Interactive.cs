using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public ItemName requireItem;
    public bool isDone;
    public Sprite normal;
    public Sprite done;
    public GameObject confiner;
    private SpriteRenderer spriteRenderer;
    private Tip tip;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normal;
        try
        {
            tip = gameObject.transform.GetChild(0).gameObject.GetComponent<Tip>();
        }
        catch { }
    }

    public virtual void CheckItem(ItemName itemName)
    {
        if (itemName == requireItem && !isDone)
        {
            if (requireItem != ItemName.None)
                EventHandler.CallItemUsedEvent(itemName);
            isDone = true;
            OnAction();
        }
    }

    /// <summary>
    /// Ĭ������ȷ����Ʒ���������
    /// </summary>
    protected virtual void OnAction()
    {
        confiner.SetActive(false);
        spriteRenderer.sprite=done;
        tip.gameObject.SetActive(false);
    }

    public virtual void EmptyAction()
    {
        if (requireItem == ItemName.None)
        {
            isDone = true;
            OnAction();
            return;
        }
        Debug.Log("�յ�");
    }

    public virtual void Check()
    {
        if (isDone)
            OnAction();
    }
}
