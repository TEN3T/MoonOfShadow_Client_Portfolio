using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Irons : ActiveSkill
{
    private List<int> angles = new List<int>();

    public Irons(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        for (int i = 0; i < skillData.projectileCount; i++)
        {
            angles.Add(i);
        }
        angles.FisherYateShuffle();

        for (int i = 0; i < skillData.projectileCount; i++)
        {
            ProjectileStraight projectile = SkillManager.Instance.SpawnProjectile<ProjectileStraight>(skillData);
            projectile.transform.localPosition = shooter.position;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angles[i] * 360.0f / skillData.projectileCount);
            yield return intervalTime;
        }

        angles.Clear();
    }
}
