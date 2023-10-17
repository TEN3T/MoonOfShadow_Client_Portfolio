using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantuaryCircle : FieldStructure
{
    private bool isHeal;
    private int hpRegen;
    private int duration;
    private WaitForSeconds tick;
    private WaitForSeconds sec;

    protected override void Awake()
    {
        base.Awake();

        isHeal = false;
        front.enabled = false;
        hpRegen = int.Parse(this.fieldStructureData.gimmickParam[0]);
        duration = int.Parse(this.fieldStructureData.gimmickParam[1]);
        tick = new WaitForSeconds(0.01f);
        sec = new WaitForSeconds(1.0f);
    }

    private void OnEnable()
    {
        StartCoroutine(Activation());
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent(out Player player) && isHeal)
        {
            player.playerManager.playerData.CurrentHpModifier(hpRegen);
            isHeal = false;
        }
    }

    private IEnumerator Activation()
    {
        for (int i = 0; i < duration; i++)
        {
            front.enabled = true;
            isHeal = true;
            yield return tick;
            front.enabled = false;
            yield return sec;
        }

        this.gameObject.SetActive(false);
    }
}
