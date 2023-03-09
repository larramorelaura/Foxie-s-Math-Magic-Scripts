using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public enum ItemType{
        HintScroll, 
        ManipulativesPotion,
        HealthPotion
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch(itemType)
        {
            default: 
            case ItemType.HintScroll: return ItemAssets.Instance.hintScrollSprite;
            case ItemType.ManipulativesPotion: return ItemAssets.Instance.manipulativesPotionSprite;
            case ItemType.HealthPotion: return ItemAssets.Instance.healthPotionSprite;
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default: 
            case ItemType.HintScroll:
            case ItemType.HealthPotion:
            case ItemType.ManipulativesPotion:
            return true;

        }
    }
}
