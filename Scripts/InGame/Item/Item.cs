using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField] public int itemId { get; private set; }
    private float itemSpeed;

    protected Player player;
    protected Transform target;
    protected ItemData itemData;
    protected Collider2D itemCollider;
    protected WaitForFixedUpdate frame;

    private void Awake()
    {
        itemSpeed = float.Parse(CSVReader.Read("BattleConfig", "ItemFollowSpeed", "ConfigValue").ToString());
        gameObject.tag = "Item";
        itemData = new ItemData();
        frame = new WaitForFixedUpdate();
        transform.localScale *= float.Parse(CSVReader.Read("BattleConfig", "ImageMultiple", "ConfigValue").ToString());
        transform.GetComponent<Renderer>().sortingLayerName = LayerConstant.SPAWNOBJECT.ToString();
    }

    private void OnEnable()
    {
        ItemSetting(itemId.ToString());
    }

    protected virtual IEnumerator Move()
    {
        while (true)
        {
            transform.Translate((target.position - transform.position).normalized * Time.deltaTime * itemSpeed);
            yield return frame;
        }
    }

    private void ItemSetting(string itemId)
    {
        Dictionary<string, Dictionary<string, object>> itemTable = CSVReader.Read("ItemTable");
        if (itemTable.ContainsKey(itemId))
        {
            Dictionary<string, object> table = itemTable[itemId];
            itemData.SetItemName(Convert.ToString(table["ItemName"]));
            itemData.SetItemImage(Convert.ToString(table["ItemImage"]));
            //itemData.SetItemType((ItemConstant)Enum.Parse(typeof(ItemConstant), Convert.ToString(table["ItemType"])));
            //Enum.TryParse(table["ItemType"].ToString(), true, out ItemConstant result);
            try
            {
                itemData.SetItemTypeParam(Convert.ToInt32(table["ItemTypeParam"]));
            }
            catch
            {
                itemData.SetItemTypeParam(0);
            }

            //itemData.SetImagePath(Convert.ToString(table["ImagePath"]));

            if (Enum.TryParse(Convert.ToString(table["ItemType"]), true, out ItemConstant type))
            {
                itemData.SetItemType(type);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)LayerConstant.ITEM)
        {
            target = GameManager.Instance.player.character;
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(Move());
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            ItemEffect.ItemEffectActivation(itemData.itemTypeParam, itemData.itemType);
        }
    }
}
