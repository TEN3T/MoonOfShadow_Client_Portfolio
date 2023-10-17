using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    private Player player;
    private CircleCollider2D itemCollider;

    private void Awake()
    {
        player = transform.parent.GetComponent<Player>();
        itemCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        gameObject.tag = "ItemCollider";
        itemCollider.isTrigger = true;
        itemCollider.radius = player.playerManager.playerData.getItemRange;
    }

    public void UpdateItemRange()
    {
        itemCollider.radius = player.playerManager.playerData.getItemRange;
    }

}
