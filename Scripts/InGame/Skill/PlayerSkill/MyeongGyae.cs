using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MyeongGyae : ActiveSkill
{
    private Monster prefab;
    private List<Monster> summoners = new List<Monster>();

    public MyeongGyae(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        string path = CSVReader.Read("MonsterTable", skillData.skillEffectParam[0], "MonsterPrefabPath").ToString();
        float timeVariable = 1.0f;
        WaitForSeconds timeCount = new WaitForSeconds(timeVariable);
        for (int i = 0; i < skillData.projectileCount - summoners.Count; i++)
        {
            if (prefab == null)
            {
                prefab = ResourcesManager.Load<Monster>(path);
            }
            Monster summoner = Object.Instantiate(prefab, shooter);
            summoner.gameObject.layer = (int)LayerConstant.SKILL;
            summoner.gameObject.SetActive(false);
            summoners.Add(summoner);
        }

        foreach (Monster summoner in summoners)
        {
            summoner.monsterData.SetAttack(skillData.damage);
            summoner.monsterData.SetMoveSpeed(skillData.speed);
            summoner.transform.localPosition = Vector2.one;
            summoner.SetTarget(Scanner.GetTargetTransform(skillData.skillTarget, summoner.transform, skillData.attackDistance), false);
            summoner.gameObject.SetActive(true);
            yield return intervalTime;
        }

        float time = skillData.duration;
        while (time > 0)
        {
            foreach (Monster summoner in summoners)
            {
                if (summoner.target == null || !summoner.target.gameObject.activeInHierarchy)
                {
                    Transform target = Scanner.GetTargetTransform(skillData.skillTarget, summoner.transform, skillData.attackDistance);
                    summoner.SetTarget(target, false);
                }
            }
            yield return timeCount;
            time -= timeVariable;
        }

        foreach (Monster summoner in summoners)
        {
            summoner.gameObject.SetActive(false);
        }
    }
}
