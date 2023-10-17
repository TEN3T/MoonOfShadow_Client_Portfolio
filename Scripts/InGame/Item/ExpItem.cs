using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : Item
{
    private void Start()
    {
        itemCollider = GetComponent<CircleCollider2D>();
        itemCollider.isTrigger = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
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
            GameManager.Instance.ExpUp(itemData.itemTypeParam);
        }
    }
}
