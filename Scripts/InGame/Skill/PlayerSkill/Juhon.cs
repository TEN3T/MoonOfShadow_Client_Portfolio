using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juhon : ActiveSkill
{
    public Juhon(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);

        Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData, shooter);
        projectile.CollisionRadius(skillData.attackDistance);
        for (int i = 0; i < skillData.projectileCount; i++)
        {
            //projectile.transform.localScale = Vector2.one * skillData.projectileSizeMulti;
            projectile.CollisionPower(true);
            yield return intervalTime;
            projectile.CollisionPower(false);
        }
        //projectile.transform.localScale = Vector2.zero;
        SkillManager.Instance.DeSpawnProjectile(projectile);
    }
    
}
