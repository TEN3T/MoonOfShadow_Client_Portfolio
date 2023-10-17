using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Scanner
{
    private static RaycastHit2D[] targets;

    public static Vector2 GetTarget(SKILLCONSTANT.SKILL_TARGET skillTarget, Transform shooter, float attackDistance)
    {
        return GetTarget(skillTarget, shooter, attackDistance, new List<Transform>());
    }

    public static Vector2 GetTarget(SKILLCONSTANT.SKILL_TARGET skillTarget, Transform shooter, float attackDistance, List<Transform> exceptions)
    {
        try
        {
            switch (skillTarget)
            {
                case SKILLCONSTANT.SKILL_TARGET.MELEE:
                    return MeleeTarget(shooter, attackDistance, exceptions).position;
                case SKILLCONSTANT.SKILL_TARGET.FRONT:
                    return FrontTarget(shooter, attackDistance);
                case SKILLCONSTANT.SKILL_TARGET.BACK:
                    return BackTarget(shooter, attackDistance);
                case SKILLCONSTANT.SKILL_TARGET.TOP:
                    return TopTarget(shooter, attackDistance);
                case SKILLCONSTANT.SKILL_TARGET.BOTTOM:
                    return BottomTarget(shooter, attackDistance);
                case SKILLCONSTANT.SKILL_TARGET.RANDOM:
                    return RandomTarget(shooter, attackDistance).position;
                case SKILLCONSTANT.SKILL_TARGET.BOSS:
                    return BossTarget().position;
                case SKILLCONSTANT.SKILL_TARGET.LOWEST:
                    return LowestTarget(shooter, attackDistance).position;
                case SKILLCONSTANT.SKILL_TARGET.MOUSE:
                    return CameraManager.Instance.cam.ScreenToWorldPoint(Input.mousePosition);
                default:
                    return RandomTarget(shooter, attackDistance).position;
            }
        }
        catch
        {
            return Vector2.one;
        }
    }

    public static Transform GetTargetTransform(SKILLCONSTANT.SKILL_TARGET skillTarget, Transform shooter, float attackDistance)
    {
        return GetTargetTransform(skillTarget, shooter, attackDistance, new List<Transform>());
    }

    public static Transform GetTargetTransform(SKILLCONSTANT.SKILL_TARGET skillTarget, Transform shooter, float attackDistance, List<Transform> exceptions)
    {
        switch (skillTarget)
        {
            case SKILLCONSTANT.SKILL_TARGET.MELEE:
                return MeleeTarget(shooter, attackDistance, exceptions);
            case SKILLCONSTANT.SKILL_TARGET.RANDOM:
                return RandomTarget(shooter, attackDistance);
            case SKILLCONSTANT.SKILL_TARGET.BOSS:
                return BossTarget();
            case SKILLCONSTANT.SKILL_TARGET.PLAYER:
                return PlayerTarget(shooter);
            case SKILLCONSTANT.SKILL_TARGET.LOWEST:
                return LowestTarget(shooter, attackDistance);
            case SKILLCONSTANT.SKILL_TARGET.MOUSE:
                return MouseTarget(exceptions);
            default:
                return RandomTarget(shooter, attackDistance);
        }
    }

    private static Transform MouseTarget(List<Transform> exceptions)
    {
        Vector2 pos = CameraManager.Instance.cam.ScreenToWorldPoint(Input.mousePosition);
        targets = Physics2D.CircleCastAll(pos, 99999, Vector2.zero, 0, 1 << (int)LayerConstant.MONSTER);
        Transform resultTarget = null;
        float distance = float.MaxValue;
        foreach (RaycastHit2D target in targets)
        {
            if (exceptions.Contains(target.transform))
            {
                continue;
            }

            float diff = (pos - (Vector2)target.transform.position).sqrMagnitude;
            if (diff < distance)
            {
                distance = diff;
                resultTarget = target.transform;
            }
        }
        return resultTarget;
    }

    private static Transform MeleeTarget(Transform shooter, float attackDistance, List<Transform> exceptions)
    {
        Vector2 shooterPos = shooter.position;
        targets = Physics2D.CircleCastAll(shooterPos, attackDistance, Vector2.zero, 0, 1 << (int)LayerConstant.MONSTER);
        Transform resultTarget = null;
        float distance = float.MaxValue;
        foreach (RaycastHit2D target in targets)
        {
            if (target.transform == shooter || exceptions.Contains(target.transform))
            {
                continue;
            }

            float diff = (shooterPos - (Vector2)target.transform.position).sqrMagnitude;
            if (diff < distance)
            {
                distance = diff;
                resultTarget = target.transform;
            }
        }
        return resultTarget;
    }

    private static Vector2 FrontTarget(Transform shooter, float attackDistance)
    {
        Vector2 pos = shooter.position;
        pos.x += attackDistance;
        return pos;
    }

    private static Vector2 BackTarget(Transform shooter, float attackDistance)
    {
        Vector2 pos = shooter.position;
        pos.x -= attackDistance;
        return pos;
    }

    private static Vector2 TopTarget(Transform shooter, float attackDistance)
    {
        Vector2 pos = shooter.position;
        pos.y += attackDistance;
        return pos;
    }

    private static Vector2 BottomTarget(Transform shooter, float attackDistance)
    {
        Vector2 pos = shooter.position;
        pos.y -= attackDistance;
        return pos;
    }

    private static Transform RandomTarget(Transform shooter, float attackDistance)
    {
        targets = Physics2D.CircleCastAll(shooter.position, attackDistance, Vector2.zero, 0, 1 << (int)LayerConstant.MONSTER);
        try
        {
            return targets[UnityEngine.Random.Range(0, targets.Length)].transform;
        }
        catch
        {
            return MeleeTarget(shooter, attackDistance, new List<Transform>());
        }
    }

    private static Transform BossTarget()
    {
        Transform resultTarget = null;
        foreach (Monster monster in MonsterSpawner.Instance.monsters)
        {
            if (monster.monsterId / 100 == 4)
            {
                resultTarget = monster.transform;
            }
        }
        return resultTarget;
    }

    private static Transform PlayerTarget(Transform shooter)
    {
        return shooter;
    }

    private static Transform LowestTarget(Transform shooter, float attackDistance)
    {
        targets = Physics2D.CircleCastAll(shooter.position, attackDistance, Vector2.zero, 0, 1 << (int)LayerConstant.MONSTER);
        Transform resultTarget = null;
        float hp = float.MaxValue;
        foreach (RaycastHit2D target in targets)
        {
            if (target.transform == shooter)
            {
                continue;
            }

            if (target.transform.TryGetComponent(out Monster monster))
            {
                if (hp > monster.monsterData.hp)
                {
                    hp = monster.monsterData.hp;
                    resultTarget = target.transform;
                }
            }

        }
        return resultTarget;
    }

    public static List<Transform> RangeTarget(Transform shooter, float attackDistance, params int[] layers)
    {
        List<Transform> resultTargets = new List<Transform>();
        foreach (int layer in layers)
        {
            targets = Physics2D.CircleCastAll(shooter.position, attackDistance, Vector2.zero, 0, 1 << layer);

            foreach (RaycastHit2D target in targets)
            {
                resultTargets.Add(target.transform);
            }
        }

        return resultTargets;
    }

    //public static Vector2 RandomTargetPos(Transform shooter, float attackDistance, float angle)
    //{
    //    Vector2 pos = shooter.position;
    //    float rad = angle * Mathf.Rad2Deg;
    //    pos.x += Mathf.Cos(rad) * attackDistance;
    //    pos.y += Mathf.Sin(rad) * attackDistance;
    //    return pos;
    //}
}
