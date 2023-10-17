using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Hyum : PassiveSkill
{
    public Hyum(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum)
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
