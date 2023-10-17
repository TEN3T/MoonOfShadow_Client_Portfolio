using SKILLCONSTANT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PassiveEffect
{
    public static void PassiveEffectActivation(float param, SKILL_PASSIVE type, CALC_MODE mode)
    {
        float data;
        switch (type)
        {
            case SKILL_PASSIVE.MAGNET:
                data = CalcData(GameManager.Instance.player.playerManager.playerData.getItemRange, param, mode);
                GameManager.Instance.player.playerManager.playerData.GetItemRangeModifier((int)data);
                GameManager.Instance.player.UpdateGetItemRange();
                break;
            case SKILL_PASSIVE.SHIELD:
                data = CalcData(GameManager.Instance.player.playerManager.playerData.shield, param, mode);
                GameManager.Instance.player.playerManager.playerData.ShieldModifier((int)data);
                break;
            case SKILL_PASSIVE.MOVESPEED:
                data = CalcData(GameManager.Instance.player.playerManager.playerData.moveSpeed, param, mode);
                GameManager.Instance.player.playerManager.playerData.MoveSpeedModifier(data);
                break;
            case SKILL_PASSIVE.EXP:
                data = CalcData(GameManager.Instance.player.playerManager.playerData.expBuff, param, mode);
                GameManager.Instance.player.playerManager.playerData.ExpBuffModifier(data);
                break;
            case SKILL_PASSIVE.PROJECTILECOUNT:
                data = CalcData(GameManager.Instance.player.playerManager.playerData.projectileAdd, param, mode);
                GameManager.Instance.player.playerManager.playerData.ProjectileAddModifier((int)data);
                break;
            case SKILL_PASSIVE.HP:
                data = CalcData(GameManager.Instance.player.playerManager.playerData.hp, param, mode);
                GameManager.Instance.player.playerManager.playerData.HpModifier((int)data);
                break;
            case SKILL_PASSIVE.HPREGEN:
                data = CalcData(GameManager.Instance.player.playerManager.playerData.hpRegen, param, mode);
                GameManager.Instance.player.playerManager.playerData.HpRegenModifier((int)data);
                break;
            case SKILL_PASSIVE.ATTACK:
                data = CalcData(GameManager.Instance.player.playerManager.playerData.attack, param, mode);
                GameManager.Instance.player.playerManager.playerData.AttackModifier(data);
                break;
            case SKILL_PASSIVE.ATTACKSPEED:
                data = CalcData(GameManager.Instance.player.playerManager.playerData.coolDown, param, mode);
                GameManager.Instance.player.playerManager.playerData.CoolDownModifier((int)data);
                break;
            case SKILL_PASSIVE.ARMOR:
                data = CalcData(GameManager.Instance.player.playerManager.playerData.armor, param, mode);
                GameManager.Instance.player.playerManager.playerData.ArmorModifier(data);
                break;
            case SKILL_PASSIVE.PROJECTILESIZE:
                GameManager.Instance.player.playerManager.playerData.SetProjectileSize(param, mode);
                break;
            case SKILL_PASSIVE.PROJECTILESPEED:
                GameManager.Instance.player.playerManager.playerData.SetProjectileSpeed(param, mode);
                break;
            case SKILL_PASSIVE.PROJECTILESPLASH:
                GameManager.Instance.player.playerManager.playerData.SetProjectileSplash(param, mode);
                break;
            case SKILL_PASSIVE.PROJECTILEDISTANCE:
                GameManager.Instance.player.playerManager.playerData.SetProjectileDistance(param, mode);
                break;
            case SKILL_PASSIVE.SKILLDAMAGE:
                GameManager.Instance.player.playerManager.playerData.SetSkillDamage(param, mode);
                break;
            default:
                DebugManager.Instance.PrintDebug("[ERROR]: 없는 패시브 스킬입니다");
                break;
        }
    }

    public static float CalcData(float value, float param, CALC_MODE mode)
    {
        if (mode == CALC_MODE.PLUS)
        {
            return param;
        }

        return value == 0 ? param : value * param;
    }
}
