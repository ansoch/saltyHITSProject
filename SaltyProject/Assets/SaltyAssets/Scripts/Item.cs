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
        ForWeapon,
        Hummer,
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
            case ItemType.ForWeapon:return ItemAssets.Instance.ForWeapon;
            case ItemType.Hummer: return ItemAssets.Instance.hummer;
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
            case ItemType.ForWeapon:
            case ItemType.Hummer:
                return false;
        }
    }
    public bool IsWeapon()
    {
        switch (itemType)
        {
            default:
            case ItemType.Coin:
            case ItemType.HealthPotion:
            case ItemType.ManaPotion:
                return false;
            case ItemType.Weapon:
            case ItemType.Scythe:
            case ItemType.ForWeapon:
            case ItemType.Hummer:
                return true;
        }
    }
}
