using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("AssetsStart");
        Instance = this;
    }
    public static ItemAssets Instance;


    public Transform pfItemWorld;
    public Sprite weapon;
    public Sprite healtPotionSprite;
    public Sprite manaPotionSprite;
    public Sprite coinSprite;
    public Sprite scythe;
    public Sprite ForWeapon;
    public Sprite hummer;
}
