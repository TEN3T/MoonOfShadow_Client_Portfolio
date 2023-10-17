using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveSkill : Skill
{
    private Dictionary<string, Dictionary<string, object>> passiveTable;

    protected int skillNum;
    protected Transform shooter;
    protected PassiveData skillData;
    //protected WaitForSeconds coolTime;
    protected WaitForFixedUpdate frame;

    public abstract IEnumerator Activation();

    public PassiveSkill(int skillId, Transform shooter, int skillNum)
    {
        passiveTable = CSVReader.Read("PassiveTable");
        this.skillData = new PassiveData();
        this.shooter = shooter;
        SetSkillData(skillId);
        //SkillDataUpdate();
        this.skillNum = skillNum;
        frame = new WaitForFixedUpdate();
    }

    public IEnumerator SkillActivation()
    {
        do
        {
            yield return Activation();
            yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        } while (skillData.coolTime > 0);
    }

    public void SkillLevelUp()
    {
        //DeActivation();
        SetSkillData(skillData.skillId + 1);
        //SkillDataUpdate();
        SkillManager.Instance.CoroutineStarter(Activation());
    }

    public void SetSkillData(int skillId)
    {
        Dictionary<string, object> data = passiveTable[skillId.ToString()];

        skillData.SetSkillId(skillId);
        skillData.SetName(Convert.ToString(data["Name"]));
        skillData.SetExplain(Convert.ToString(data["Desc"]));
        skillData.SetIconPath(Convert.ToString(data["Icon"]));
        skillData.SetImagePath(Convert.ToString(data["PassiveImage"]));
        
        try
        {
            List<SKILL_PASSIVE> list = new List<SKILL_PASSIVE>();
            foreach (string str in (data["PassiveEffect"] as List<string>))
            {
                if (Enum.TryParse(str, true, out SKILL_PASSIVE effect))
                {
                    list.Add(effect);
                }
            }
            skillData.SetEffect(list);
        }
        catch
        {
            if (Enum.TryParse(Convert.ToString(data["PassiveEffect"]), true, out SKILL_PASSIVE effect))
            {
                List<SKILL_PASSIVE> list = new List<SKILL_PASSIVE>()
                    {
                        effect,
                    };
                skillData.SetEffect(list);
            }
            else
            {
                skillData.SetEffect(new List<SKILL_PASSIVE>());
            }
        }

        //skillData.SetCoolTime(Convert.ToInt32(data["PassiveCoolTime"]));
        //coolTime = new WaitForSeconds(skillData.coolTime * 0.001f);
        try
        {
            skillData.SetCoolTime(Convert.ToInt32(data["PassiveCoolTime"]) / 1000.0f);
        }
        catch
        {
            skillData.SetCoolTime(0);
        }
        //finally
        //{
        //    coolTime = new WaitForSeconds(skillData.coolTime);
        //}

        try
        {
            List<CALC_MODE> list = new List<CALC_MODE>();
            foreach (string str in (data["CalcType"] as List<string>))
            {
                if (Enum.TryParse(str, true, out CALC_MODE mode))
                {
                    list.Add(mode);
                }
            }
            skillData.SetCalcMode(list);
        }
        catch
        {
            try
            {
                if (Enum.TryParse(Convert.ToString(data["CalcType"]), true, out CALC_MODE mode))
                {
                    List<CALC_MODE> list = new List<CALC_MODE>()
                    {
                        mode,
                    };
                    skillData.SetCalcMode(list);
                }
            }
            catch
            {
                skillData.SetCalcMode(new List<CALC_MODE>());
            }
        }

        try
        {
            List<float> list = new List<float>();
            foreach(string str in data["PassiveParam"] as List<string>)
            {
                list.Add(float.Parse(str));
            }
            skillData.SetEffectParam(list);
        }
        catch
        {
            try
            {
                List<float> list = new List<float>()
                {
                    float.Parse(Convert.ToString(data["PassiveParam"])),
                };
                skillData.SetEffectParam(list);
            }
            catch
            {
                skillData.SetEffectParam(new List<float>());
            }
        }

        skillData.SetPrefabPath(Convert.ToString(data["PassivePrefabPath"]));
    }
}
