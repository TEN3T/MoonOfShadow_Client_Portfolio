using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pok : ActiveSkill
{
    public Pok(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);

        for (int i = 0; i < skillData.projectileCount; i++)
        {
            Projectile projectile = SkillManager.Instance.SpawnProjectile<Projectile>(skillData, shooter);
            projectile.CollisionPower(false);
            projectile.transform.localPosition = Vector2.up * skillData.attackDistance;
            projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
            projectile.SetAlpha(1.0f);
            SkillManager.Instance.CoroutineStarter(Move(projectile));
            yield return intervalTime;
        }
    }

    private IEnumerator Move(Projectile projectile)
    {
        projectile.CollisionPower(true);
        Vector3 rotate = GameManager.Instance.player.lookDirection.x >= 0 ? Vector3.back : Vector3.forward;
        float angle = 0.0f;
        float weight = skillData.speed * Time.deltaTime;
        while (angle < 180.0f)
        {
            weight += 0.001f;
            angle += weight;
            projectile.transform.RotateAround(shooter.position, rotate, weight);
            yield return new WaitForFixedUpdate();
        }
        projectile.CollisionPower(false);
        projectile.SetAlpha(0.0f);
    }
}
