using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour,IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler    
{
    public Image itemImage;
    public ItemTooltip tooltip;
    public ItemDetails currentItem;
    public ItemAmount itemAmount;
    private bool isSelected;

    public void SetItem(ItemDetails itemDetails,int amount)
    {
        currentItem = itemDetails;
        this.gameObject.SetActive(true);
        itemImage.sprite=itemDetails.itemSprite;
        itemImage.SetNativeSize();
        itemAmount.gameObject.SetActive(true);
        itemAmount.UpdateItemAmount(amount);
    }

    public void SetEmpty()
    {
        this.gameObject.SetActive(false);
        itemAmount.gameObject.SetActive(false);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        isSelected=!isSelected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(this.gameObject.activeInHierarchy)
        {
            tooltip.gameObject.SetActive(true);
            tooltip.UpdateItemName(currentItem.itemName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }
}
