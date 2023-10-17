using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ActiveSkill : Skill
{
    private Dictionary<string, Dictionary<string, object>> skillTable;

    protected int skillNum;
    protected Transform shooter;
    //protected List<Projectile> projectiles;
    protected ActiveData skillData;
    protected Vector2 originSize;

    protected WaitForFixedUpdate frame;
    //protected WaitForSeconds coolTime;
    protected WaitForSeconds intervalTime;
    protected WaitForSeconds duration;

    private float originDamage;

    //public abstract void Init();
    public abstract IEnumerator Activation();

    public ActiveSkill(int skillId, Transform shooter, int skillNum)
    {
        skillTable = CSVReader.Read("SkillTable");
        //this.projectiles = new List<Projectile>();
        this.skillData = new ActiveData();
        this.shooter = shooter;
        SetSkillData(skillId);
        //SkillDataUpdate();
        this.skillNum = skillNum;
        frame = new WaitForFixedUpdate();
    }

    public IEnumerator SkillActivation()
    {
        if (!skillData.isEffect)
        {
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        }

        do
        {
            if (skillData.skillCut)
            {
                SceneManager.LoadScene(skillData.cutDire, LoadSceneMode.Additive);
                yield return new WaitForSeconds(0.1f);
            }
            yield return Activation();
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        } while (skillData.coolTime > 0.0f);
    }

    public void SkillLevelUp()
    {
        SetSkillData(skillData.skillId + 1);
        //SkillDataUpdate();
    }

    public void SetSkillData(int skillId)
    {
        Dictionary<string, object> data = skillTable[skillId.ToString()];

        skillData.SetSkillId(skillId);
        try
        {
            skillData.SetCoolTime(Convert.ToInt32(data["Cooltime"]) / 1000.0f);
        }
        catch
        {
            skillData.SetCoolTime(0);
        }
        //finally
        //{
        //    coolTime = new WaitForSeconds(skillData.coolTime);
        //}
        skillData.SetAttackDistance(float.Parse(data["AttackDistance"].ToString()));
        skillData.SetDamage(float.Parse(data["Damage"].ToString()));
        originDamage = skillData.damage;

        List<string> list = data["SkillEffectParam"] as List<string>;
        if (list == null)
        {
            list = new List<string>
            {
                data["SkillEffectParam"].ToString(),
            };
            skillData.SetSkillEffectParam(list);
        }
        else
        {
            skillData.SetSkillEffectParam(list);
        }
        
        skillData.SetSkillCut(Convert.ToBoolean(Convert.ToString(data["Skill_Cut"]).ToLower()));
        skillData.SetIsEffect(Convert.ToBoolean(Convert.ToString(data["IsEffect"]).ToLower()));
        skillData.SetIsUltimate(Convert.ToBoolean(Convert.ToString(data["IsUltimate"]).ToLower()));
        skillData.SetName(Convert.ToString(data["Name"]));
        skillData.SetDesc(Convert.ToString(data["Desc"]));
        skillData.SetIcon(Convert.ToString(data["Icon"]));
        skillData.SetCutDire(Convert.ToString(data["Cut_dire"]));
        skillData.SetSkillImage(Convert.ToString(data["SkillImage"]));
        try
        {
            List<SKILL_EFFECT> list2 = new List<SKILL_EFFECT>();
            foreach (string str in (data["SkillEffect"] as List<string>))
            {
                list2.Add((SKILL_EFFECT)Enum.Parse(typeof(SKILL_EFFECT), str, true));
            }
            skillData.SetSkillEffect(list2);
        }
        catch
        {
            try
            {
                List<SKILL_EFFECT> list2 = new List<SKILL_EFFECT>()
            {
                (SKILL_EFFECT)Enum.Parse(typeof(SKILL_EFFECT), Convert.ToString(data["SkillEffect"]), true),
            };
                skillData.SetSkillEffect(list2);
            }
            catch
            {
                skillData.SetSkillEffect(new List<SKILL_EFFECT>());
            }
        }
        skillData.SetSkillTarget((SKILL_TARGET)Enum.Parse(typeof(SKILL_TARGET), Convert.ToString(data["SkillTarget"]).ToUpper()));
        skillData.SetProjectileCount(Convert.ToInt32(data["ProjectileCount"]));
        skillData.SetIntervalTime(Convert.ToInt32(data["IntervalTime"]) / 1000.0f);
        intervalTime = new WaitForSeconds(skillData.intervalTime);
        skillData.SetDuration(Convert.ToInt32(data["Duration"]) / 1000.0f);
        duration = new WaitForSeconds(skillData.duration);
        skillData.SetSpeed(float.Parse(Convert.ToString(data["Speed"])));
        skillData.SetSplashRange(float.Parse(Convert.ToString(data["SplashRange"])));
        skillData.SetProjectileSizeMulti(float.Parse(Convert.ToString(data["ProjectileSizeMulti"])));
        skillData.SetIsPenetrate(Convert.ToBoolean(data["IsPenetrate"]));

        //PlayerData playerData = GameManager.Instance.player.playerManager.playerData;
        //skillData.SetProjectileSizeMulti(skillData.projectileSizeMulti + playerData.projectileSize);
        //skillData.SetProjectileCount(skillData.projectileCount + playerData.projectileAdd);
        //skillData.SetSplashRange(skillData.splashRange + playerData.projectileSplash);
        //skillData.SetSpeed(skillData.speed + playerData.projectileSpeed);
    }
}
