using SKILLCONSTANT;
using System.Collections.Generic;

public class PassiveData
{
    public int skillId { get; private set; }
    public string name { get; private set; }
    public string explain { get; private set; }
    public string iconPath { get; private set; }
    public string imagePath { get; private set; }
    public List<SKILL_PASSIVE> skillEffect { get; private set; }
    public float coolTime { get; private set; }
    public List<CALC_MODE> calcMode { get; private set; }
    public List<float> skillEffectParam { get; private set; }
    public string prefabPath { get; private set; }

    public void SetSkillId(int skillId) { this.skillId = skillId; }
    public void SetName(string name) { this.name = name; }
    public void SetExplain(string explain) { this.explain = explain; }
    public void SetIconPath(string iconPath) { this.iconPath = iconPath; }
    public void SetImagePath(string imagePath) { this.imagePath = imagePath; }
    public void SetEffect(List<SKILL_PASSIVE> skillEffect) { this.skillEffect = skillEffect; }
    public void SetCoolTime(float coolTime) { this.coolTime = coolTime; }
    public void SetCalcMode(List<CALC_MODE> calcMode) { this.calcMode = calcMode; }
    public void SetEffectParam(List<float> skillEffectParam) { this.skillEffectParam = skillEffectParam; }
    public void SetPrefabPath(string prefabPath) { this.prefabPath = prefabPath; }

    public void ModifyCoolTime(float coolTime) { this.coolTime += coolTime; }
}
