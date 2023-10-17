using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crepitus : ActiveSkill
{
    private WaitForSeconds tick;
    private float size;

    public Crepitus(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        tick = new WaitForSeconds(0.25f);
        size = skillData.attackDistance >= 10 ? 10 : skillData.attackDistance;

        for (int i = 0; i < skillData.projectileCount; i++)
        {
            Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData);
            SkillManager.Instance.CoroutineStarter(Boom(projectile));
            yield return intervalTime;
        }
    }

    private IEnumerator Boom(Projectile projectile)
    {
        projectile.transform.localPosition = Scanner.GetTarget(SKILLCONSTANT.SKILL_TARGET.RANDOM, shooter, size);
        yield return tick;
        SkillManager.Instance.DeSpawnProjectile(projectile);
    }
}
