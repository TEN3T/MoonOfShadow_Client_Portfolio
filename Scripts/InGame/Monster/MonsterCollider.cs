using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollider : MonoBehaviour
{
    private float sizeX;

    public CapsuleCollider2D attackCollider { get; private set; }

    private void Awake()
    {
        attackCollider = GetComponent<CapsuleCollider2D>();
        attackCollider.isTrigger = true;
        sizeX = attackCollider.size.x;
        gameObject.layer = (int)LayerConstant.HIT;
    }

    private void OnEnable()
    {
        attackCollider.enabled = false;
    }

    public void SetAttackDistance(float atkDistance)
    {
        Vector2 size = attackCollider.size;
        size.x = sizeX + atkDistance;
        attackCollider.size = size;

        Vector2 offSet = attackCollider.offset;
        offSet.x += -atkDistance;
        attackCollider.offset = offSet;
    }

    public void AttackColliderSwitch(bool flag)
    {
        attackCollider.enabled = flag;
    }

}
