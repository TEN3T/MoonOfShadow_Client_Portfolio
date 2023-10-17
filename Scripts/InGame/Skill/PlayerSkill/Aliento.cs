using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aliento : ActiveSkill
{
    public Aliento(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    //public override void Init()
    //{
    //}

    public override IEnumerator Activation()
    {
        //if (!skillData.isEffect)
        //{
        //    yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        //}

        //do
        //{
        //    for (int i = 0; i < skillData.projectileCount; i++)
        //    {
        //        ProjectileStraight projectile = SkillManager.Instance.SpawnProjectile<ProjectileStraight>(skillData);
        //        projectile.transform.localPosition = shooter.position;
        //        Vector2 pos = Scanner.GetTarget(skillData.skillTarget, shooter, skillData.attackDistance);
        //        pos -= (Vector2)shooter.position;
        //        projectile.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - 90.0f);
        //        yield return intervalTime;
        //    }

        //    yield return PlayerUI.Instance.skillBoxUi.boxIcons[skillNum].Dimmed(skillData.coolTime);
        //} while (skillData.coolTime > 0.0f);
        for (int i = 0; i < skillData.projectileCount; i++)
        {
            ProjectileStraight projectile = SkillManager.Instance.SpawnProjectile<ProjectileStraight>(skillData);
            projectile.transform.localPosition = shooter.position;
            Vector2 pos = Scanner.GetTarget(skillData.skillTarget, shooter, skillData.attackDistance);
            pos -= (Vector2)shooter.position;
            projectile.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - 90.0f);
            yield return intervalTime;
        }
    }
}
