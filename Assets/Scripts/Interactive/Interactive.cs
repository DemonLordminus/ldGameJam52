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

    private void Start()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
        spriteRenderer.sprite=normal;
    }

    public virtual void CheckItem(ItemName itemName)
    {
        if (itemName == requireItem && !isDone)
        {
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
    }

    public virtual void EmptyAction()
    {
        if (requireItem == ItemName.None)
        {
            OnAction(); 
            return;
        }
        Debug.Log("�յ�");
    }
}