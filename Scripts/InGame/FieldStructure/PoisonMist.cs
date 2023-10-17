using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMist : FieldStructure
{
    private float slowValue;

    protected override void Awake()
    {
        base.Awake();

        slowValue = float.Parse(this.fieldStructureData.gimmickParam[0]) * 0.01f;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent(out Player player))
        {
            StartCoroutine(player.Slow(5.0f, slowValue));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent(out Player player))
        {
            StartCoroutine(player.Slow(5.0f, slowValue));
        }
    }
}
