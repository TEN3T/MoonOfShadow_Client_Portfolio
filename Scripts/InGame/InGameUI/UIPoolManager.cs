using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPoolManager : SingletonBehaviour<UIPoolManager>
{
    private const string SKILL_BUTTON_PATH = "Prefabs/UI/SkillButton";
    private const string SKILL_BOX_ICON_PATH = "Prefabs/UI/SkillIcon";
    private const string HP_BAR_PATH = "Prefabs/UI/HpBar";

    private Dictionary<string, ObjectPool<InGameUI>> pools;
    //private ObjectPool<SkillBoxIcon> skillBoxIconPool;
    //private ObjectPool<SkillUI> skillUIPool;

    protected override void Awake()
    {
        pools = new Dictionary<string, ObjectPool<InGameUI>>
        {
            { "SkillUI", new ObjectPool<InGameUI>(ResourcesManager.Load<SkillUI>(SKILL_BUTTON_PATH), transform) },
            { "SkillBoxIcon", new ObjectPool<InGameUI>(ResourcesManager.Load<SkillBoxIcon>(SKILL_BOX_ICON_PATH), transform) },
            { "HpBar", new ObjectPool<InGameUI>(ResourcesManager.Load<HpBar>(HP_BAR_PATH), transform) },
        };
    }

    public InGameUI SpawnUI(string type, Transform transform, Vector2 pos)
    {
        InGameUI ui = pools[type].GetObject();
        ui.transform.SetParent(transform);
        ui.transform.localPosition = pos;
        ui.transform.localScale = Vector3.one;
        ui.gameObject.SetActive(true);
        return ui;
    }

    public InGameUI SpawnUI(string type, Transform transform)
    {
        return SpawnUI(type, transform, Vector2.zero);
    }

    public void DeSpawnUI(string type, InGameUI ui)
    {
        pools[type].ReleaseObject(ui);
        ui.transform.SetParent(transform);
    }

    //public SkillUI SpawnButton(Transform transform, Vector2 pos)
    //{
    //    SkillUI skillUi = skillUIPool.GetObject();
    //    skillUi.transform.SetParent(transform);
    //    skillUi.transform.localPosition = pos;
    //    skillUi.transform.localScale = Vector3.one;
    //    //skillUi.SkillDataInit();
    //    skillUi.gameObject.SetActive(true);
    //    return skillUi;
    //}

    //public void DeSpawnButton(SkillUI skillUi)
    //{
    //    skillUIPool.ReleaseObject(skillUi);
    //    skillUi.transform.SetParent(transform);
    //}

}
