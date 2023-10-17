using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFlame : FieldStructure
{
    private float damage;
    private float dotTime;
    private float dotDamage;
    private float slow;
    private float currentDotTime;

    protected override void Awake()
    {
        base.Awake();

        damage = float.Parse(this.fieldStructureData.gimmickParam[0]);
        dotTime = float.Parse(this.fieldStructureData.gimmickParam[1]);
        dotDamage = float.Parse(this.fieldStructureData.gimmickParam[2]);
        slow = float.Parse(this.fieldStructureData.gimmickParam[3]);
        currentDotTime = dotTime;
    }

    private void Update()
    {
        if (currentDotTime > 0.0f)
        {
            currentDotTime -= Time.deltaTime;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (currentDotTime <= 0.0f)
        {
            if (collision.TryGetComponent(out Monster monster))
            {
                monster.Hit(-(int)damage);
            }
            Player player = collision.GetComponentInParent<Player>();
            if (player != null)
            {
                StartCoroutine(player.Invincible());
                player.playerManager.playerData.CurrentHpModifier(-(int)damage);
                player.Slow(0.5f, slow);
            }
            currentDotTime = dotTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Monster monster))
        {
            StartCoroutine(monster.FireDot(dotTime, dotDamage));
            return;
        }
        Player player = collision.GetComponentInParent<Player>();
        if (player != null)
        {
            StartCoroutine(player.FireDot(dotTime, dotDamage));
        }
    }
}
