using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GwiGi : ActiveSkill
{
    public GwiGi(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        shooter = Scanner.GetTargetTransform(skillData.skillTarget, shooter, skillData.attackDistance);

        for (int i = 0; i < skillData.projectileCount; i++)
        {
            Projectile projectile = SkillManager.Instance.SpawnProjectile(skillData, shooter, false);
            projectile.transform.localPosition = Vector2.up * skillData.attackDistance;
            projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
            SkillManager.Instance.CoroutineStarter(Move(projectile));
            yield return intervalTime;
        }
    }

    private IEnumerator Move(Projectile projectile)
    {
        float angle = 0.0f;
        float weight = 0.0f;
        if (shooter.TryGetComponent(out Player player))
        {
            if (player.lookDirection.x >= 0)
            {
                do
                {
                    weight += 0.002f;
                    angle -= Time.fixedDeltaTime * skillData.speed + weight;
                    projectile.transform.RotateAround(shooter.position, Vector3.forward, angle);
                    yield return frame;
                } while (projectile.transform.localEulerAngles.z > 240.0f);
            }
            else
            {
                while (projectile.transform.localEulerAngles.z < 100.0f)
                {
                    weight += 0.001f;
                    angle += Time.fixedDeltaTime * skillData.speed + weight;
                    projectile.transform.RotateAround(shooter.position, Vector3.forward, angle);
                    yield return frame;
                }
            }
        }

        //if (Scanner.GetTarget(skillData.skillTarget, shooter, skillData.attackDistance).x >= 0)
        //{
        //    do
        //    {
        //        weight += 0.002f;
        //        angle -= Time.fixedDeltaTime * skillData.speed + weight;
        //        projectile.transform.RotateAround(shooter.position, Vector3.forward, angle);
        //        yield return frame;
        //    } while (projectile.transform.localEulerAngles.z > 240.0f);
        //}
        //else
        //{
        //    while (projectile.transform.localEulerAngles.z < 100.0f)
        //    {
        //        weight += 0.001f;
        //        angle += Time.fixedDeltaTime * skillData.speed + weight;
        //        projectile.transform.RotateAround(shooter.position, Vector3.forward, angle);
        //        yield return frame;
        //    }
        //}

        SkillManager.Instance.DeSpawnProjectile(projectile);
    }

}
