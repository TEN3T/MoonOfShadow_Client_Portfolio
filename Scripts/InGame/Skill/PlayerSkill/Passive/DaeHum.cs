using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaeHum : PassiveSkill
{
    private int prevShield;

    public DaeHum(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum)
    {
    }

    public override IEnumerator Activation()
    {
        for (int i = 0; i < skillData.skillEffect.Count; i++)
        {
            PassiveEffect.PassiveEffectActivation(skillData.skillEffectParam[i], skillData.skillEffect[i], skillData.calcMode[i]);
        }
        yield return Crash();
    }

    private IEnumerator Crash()
    {
        do
        {
            prevShield = GameManager.Instance.player.playerManager.playerData.shield;
            yield return frame;
        } while (prevShield <= GameManager.Instance.player.playerManager.playerData.shield);
    }
}
