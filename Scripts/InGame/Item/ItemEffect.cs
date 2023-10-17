using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemEffect
{
    public static void ItemEffectActivation(int param, ItemConstant type)
    {
        switch (type)
        {
            case ItemConstant.EXP:
                GameManager.Instance.ExpUp(param);
                break;
            case ItemConstant.KILLALL:
                foreach(Monster monster in MonsterSpawner.Instance.monsters)
                {
                    if (CameraManager.Instance.IsTargetVisible(monster.transform.position))
                    {
                        monster.Die(true);
                    }
                }
                break;
            case ItemConstant.MOVESTOP:
                foreach (Monster monster in MonsterSpawner.Instance.monsters)
                {
                    if (CameraManager.Instance.IsTargetVisible(monster.transform.position))
                    {
                        monster.SkillEffectActivation(SKILLCONSTANT.SKILL_EFFECT.STUN, param);
                    }
                }
                break;
            case ItemConstant.HEAL:
                GameManager.Instance.player.playerManager.playerData.CurrentHpModifier(param);
                break;
            case ItemConstant.MAGNET:
                SkillManager.Instance.CoroutineStarter(Magnet(param));
                break;
            case ItemConstant.GETBOXA:
                GameManager.Instance.boxA += param;
                break;
            case ItemConstant.GETBOXB:
                GameManager.Instance.boxB += param;
                break;
            case ItemConstant.GETBOXC:
                GameManager.Instance.boxC += param;
                break;
            case ItemConstant.GETKEY:
                GameManager.Instance.key += param;
                break;
            default:
                DebugManager.Instance.PrintError("[ERROR]: 없는 아이템 효과입니다");
                break;
        }
    }

    private static IEnumerator Magnet(int param)
    {
        GameManager.Instance.player.playerManager.playerData.GetItemRangeModifier(param);
        GameManager.Instance.player.UpdateGetItemRange();
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.player.playerManager.playerData.GetItemRangeModifier(-param);
        GameManager.Instance.player.UpdateGetItemRange();
    }
}
