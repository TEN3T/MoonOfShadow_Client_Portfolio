using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : ActiveSkill
{
    private const int CHANGE_ID = 0;
    private const string ANIMATOR_PATH = "";
    private const string SKELETONDATA_ASSET_PATH = "";

    public Possession(int skillId, Transform shooter, int skillNum) : base(skillId, shooter, skillNum) { }

    public override IEnumerator Activation()
    {
        Player player = shooter.GetComponent<Player>();
        Dictionary<string, Dictionary<string, object>> table = CSVReader.Read("CharacterTable");

        //변신
        yield return duration;
        //원래대로
    }
}
