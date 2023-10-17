using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bujung : ActiveSkill
{
    public Bujung(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        for (int i = 0; i < skillData.projectileCount; i++)
        {
            ProjectileStraight projectile = SkillManager.Instance.SpawnProjectile<ProjectileStraight>(skillData);
            projectile.transform.localPosition = shooter.position;

            Vector2 targetPos = Scanner.GetTarget(skillData.skillTarget, shooter, skillData.attackDistance);
            Vector2 vec = (targetPos - (Vector2)shooter.position).normalized;
            projectile.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg - 90.0f);
            yield return intervalTime;
        }
    }
}
