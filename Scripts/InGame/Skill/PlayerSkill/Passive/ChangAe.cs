using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangAe : PassiveSkill
{
    public ChangAe(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum)
    {
    }

    public override IEnumerator Activation()
    {
        for (int i = 0; i < skillData.skillEffect.Count; i++)
        {
            PassiveEffect.PassiveEffectActivation(skillData.skillEffectParam[i], skillData.skillEffect[i], skillData.calcMode[i]);
        }
        yield return new WaitForFixedUpdate();
    }
}
