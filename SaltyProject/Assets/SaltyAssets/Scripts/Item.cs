using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item 
{
    public enum ItemType
    {
        Weapon,
        HealthPotion,
        ManaPotion,
        Coin,
        Scythe,
    }
    public ItemType itemType;
    public int amount;
    public Sprite GetSprite()
    {
        switch(itemType)
        {
            default:
            case ItemType.Weapon: return ItemAssets.Instance.weapon;
            case ItemType.HealthPotion: return ItemAssets.Instance.healtPotionSprite;
            case ItemType.ManaPotion: return ItemAssets.Instance.manaPotionSprite;
            case ItemType.Coin: return ItemAssets.Instance.coinSprite;
            case ItemType.Scythe: return ItemAssets.Instance.scythe;
        }
    }
    public bool IsStackable()
    {
        switch(itemType)
        {
            default:
            case ItemType.Coin:
            case ItemType.HealthPotion:
            case ItemType.ManaPotion:
                return true;
            case ItemType.Weapon:
            case ItemType.Scythe:
                return false;
        }
    }
}
