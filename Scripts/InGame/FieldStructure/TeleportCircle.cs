using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCircle : FieldStructure
{
    [SerializeField] private TeleportCircle connectedTeleport;
    [SerializeField] private float coolTime = 2.0f;

    private SpriteRenderer topSpriteRenderer;
    private SpriteRenderer frontSpriteRenderer;
    private int teleportCount;
    private bool available;

    protected override void Awake()
    {
        base.Awake();

        teleportCount = int.Parse(this.fieldStructureData.gimmickParam[0]);
        available = true;
        topSpriteRenderer = top.GetComponent<SpriteRenderer>();
        frontSpriteRenderer = front.GetComponent<SpriteRenderer>();
        frontSpriteRenderer.enabled = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (available)
        {
            if (collision.transform.parent.TryGetComponent(out Player player))
            {
                StartCoroutine(Activation(player));
            }
        }
    }

    private IEnumerator Activation(Player player)
    {
        available = false;
        yield return new WaitForSeconds(1.0f);

        player.transform.localPosition = connectedTeleport.transform.position + new Vector3(0.0f, 0.25f, 0.0f);
        StartCoroutine(connectedTeleport.CoolTime());
        StartCoroutine(CoolTime());
    }

    public IEnumerator CoolTime()
    {
        available = false;
        teleportCount--;
        topSpriteRenderer.enabled = false;
        frontSpriteRenderer.enabled = true;
        yield return new WaitForSeconds(coolTime);
        if (teleportCount > 0)
        {
            available = true;
            topSpriteRenderer.enabled = true;
            frontSpriteRenderer.enabled = false;
        }
    }
}
