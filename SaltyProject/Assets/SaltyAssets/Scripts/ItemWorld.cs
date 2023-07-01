using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position,Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld,position , Quaternion.identity);
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);
        return itemWorld;
    }
    public static ItemWorld DropItem(Vector3 position,Item item)
    {
        ItemWorld itemWorld = SpawnItemWorld(position, item);
        return itemWorld;
    }
    private Item item;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
    }
    public Item GetItem() {  return this.item; }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.item.itemType == Item.ItemType.Coin && collision.gameObject.CompareTag("Shop"))
        {
            int kolvo;
            kolvo = this.item.amount / 2;
            ItemWorld.SpawnItemWorld(gameObject.transform.position, new Item { itemType = Item.ItemType.HealthPotion, amount = kolvo });
            Destroy(gameObject);
        }
    }
}
