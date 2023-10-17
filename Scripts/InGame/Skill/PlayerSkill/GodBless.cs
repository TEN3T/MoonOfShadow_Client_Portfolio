using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodBless : ActiveSkill
{
    public GodBless(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);

        if (skillData.splashRange < 10)
        {
            for (int i = 0; i < skillData.projectileCount; i++)
            {
                Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData, shooter);
                projectile.transform.localScale = Vector2.zero;
                projectile.CollisionRadius(skillData.splashRange);
                projectile.transform.localScale = Vector2.one;
                projectile.CollisionPower(true);
                yield return duration;
                projectile.CollisionPower(false);
                projectile.transform.localScale = Vector2.zero;
            }
        }
        else    //일정범위 초과시 맵 전체 타격
        {
            foreach (Monster monster in MonsterSpawner.Instance.monsters)
            {
                //데미지 처리
                monster.Hit(skillData.damage);
            }
        }
    }
}
