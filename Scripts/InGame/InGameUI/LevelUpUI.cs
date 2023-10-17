using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    //private const int SKILL_MAX_LEVEL = 8;
    //private const int ACTIVE_SKILL_MAX_COUNT = 6;
    //private const int PASSIVE_SKILL_MAX_COUNT = 5;
    //private const int SKILL_MAX_COUNT = ACTIVE_SKILL_MAX_COUNT + PASSIVE_SKILL_MAX_COUNT;

    private Dictionary<string, Dictionary<string, object>> skillTable;
    private Dictionary<string, Dictionary<string, object>> passiveTable;

    private Transform body;
    private RectTransform bodyRect;
    private List<int> skillBenList;
    //private List<int> skillNums = new List<int>();
    private Dictionary<int, int> skillNums = new Dictionary<int, int>();

    public bool isSelect { get; private set; }
    public List<SkillUI> skillUis { get; private set; } = new List<SkillUI>();
    //public int skillCount { get; private set; } = 0;
    public List<int> skills { get; private set; } = new List<int>();   //가진 스킬이 아니라 ui에 올라온 스킬 목록 (중복 방지)

    private void Awake()
    {
        try
        {
            skillBenList = (CSVReader.Read("BattleConfig", "SkillBenList", "ConfigValue") as List<string>).Select(int.Parse).ToList();
        }
        catch
        {
            skillBenList = new List<int>
            {
                Convert.ToInt32(CSVReader.Read("BattleConfig", "SkillBenList", "ConfigValue"))
            };
        }

        skillTable = CSVReader.Read("SkillTable");
        passiveTable = CSVReader.Read("PassiveTable");

        SkillNumRead();
        body = transform.Find("Body");
        bodyRect = body.GetComponent<RectTransform>();
    }

    private void CloseBox(int skillId)
    {
        int skillNum = skillId / 10000 == 1 ? PlayerUI.Instance.activeSkillCount : PlayerUI.Instance.passiveSkillCount;
        SkillManager.Instance.SkillAdd(skillId, GameManager.Instance.player.transform, skillNum);

        foreach (SkillUI ui in skillUis)
        {
            UIPoolManager.Instance.DeSpawnUI("SkillUI", ui);
        }

        //Time.timeScale = 1f;
        GameManager.Instance.Pause();
        gameObject.SetActive(false);
        isSelect = true;
    }

    public void SkillBoxInit(int num)
    {
        isSelect = false;
        float height = 175 + 120 * num;
        bodyRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        for (int i = 0; i < num; i++)
        {
            int skillId = RandomSkillId();
            if (skillId == 99999)
            {
                continue;
            }
            Vector2 pos = new Vector2(0, height * 0.5f - 175 - 120 * i);
            SkillUI skillUi = (SkillUI)UIPoolManager.Instance.SpawnUI("SkillUI", body.transform, pos);
            if (skillId / 10000 == 1)
            {
                skillUi.UISetting(skillTable[skillId.ToString()]);
            }
            else if (skillId / 10000 == 2)
            {
                skillUi.UISetting(passiveTable[skillId.ToString()]);
            }
            skillUi.btn.onClick.RemoveAllListeners();
            skillUi.btn.onClick.AddListener(() => CloseBox(skillId));
            skillUis.Add(skillUi);
        }
    }

    /*
     * 1. 벤 리스트 체크
     * 2. 이미 스킬 선택창에 올라간 스킬인가 체크
     * 3. 이미 가지고 있는 스킬인가 -> 스킬 레벨업
     */
    private int RandomSkillId()
    {
        //Dictionary<int, SkillInfo> skillList = SkillManager.Instance.skillList;
        Dictionary<int, Skill> skillList = SkillManager.Instance.skillList;
        int skillId = 0;
        int c = skillNums.Count;
        if (PlayerUI.Instance.skillCount < SkillManager.SKILL_MAX_COUNT) //스킬칸이 남은 경우
        {
            while (c-- != 0)
            {
                //skillId = skillNums[UnityEngine.Random.Range(0, skillNums.Count)];
                skillId = skillNums.ElementAt(UnityEngine.Random.Range(0, skillNums.Count)).Key;
                if (skillBenList.Contains(skillId))
                {
                    continue;
                }

                if (skills.Contains(skillId))
                {
                    continue;
                }

                skillId = skillId * 100 + 1;

                foreach (int id in skillList.Keys)
                {
                    if (id / 100 == skillId / 100) //가지고 있는 스킬일 때
                    {
                        if (id % 100 != skillNums[id / 100] /*SkillManager.SKILL_MAX_LEVEL*/) //만렙이 아니라면
                        {
                            skillId = id + 1;
                            skills.Add(skillId / 100);
                            return skillId;
                        }
                    }
                }

                if (skillId / 10000 == 1 && PlayerUI.Instance.activeSkillCount == SkillManager.ACTIVE_SKILL_MAX_COUNT)
                {
                    continue;
                }
                if (skillId / 10000 == 2 && PlayerUI.Instance.passiveSkillCount == SkillManager.PASSIVE_SKILL_MAX_COUNT)
                {
                    continue;
                }

                skills.Add(skillId / 100);
                return skillId;
            }
        }
        else
        {
            int index = UnityEngine.Random.Range(0, SkillManager.SKILL_MAX_COUNT);
            for (int i = 0; i < SkillManager.SKILL_MAX_COUNT; i++)
            {
                skillId = skillList.Keys.ElementAt((i + index) % SkillManager.SKILL_MAX_COUNT);
                if (skills.Contains(skillId / 100))
                {
                    continue;
                }
                if (skillId % 100 != skillNums[skillId / 100] /*SkillManager.SKILL_MAX_LEVEL*/)
                {
                    skills.Add(skillId / 100);
                    return skillId + 1;
                }
            }
        }

        return 99999;
    }

    private void SkillNumRead()
    {
        foreach (string id in skillTable.Keys)
        {
            try
            {
                int i = Convert.ToInt32(id) / 100;
                //if (!skillNums.Contains(i))
                //{
                //    skillNums.Add(i);
                //}
                if (!skillNums.ContainsKey(i))
                {
                    skillNums.Add(i, 1);
                }
                else
                {
                    skillNums[i]++;
                }
            }
            catch
            {
                continue;
            }
        }

        foreach (string id in passiveTable.Keys)
        {
            try
            {
                int i = Convert.ToInt32(id) / 100;
                //if (!skillNums.Contains(i))
                //{
                //    skillNums.Add(i);
                //}
                if (!skillNums.ContainsKey(i))
                {
                    skillNums.Add(i, 1);
                }
                else
                {
                    skillNums[i]++;
                }
            }
            catch
            {
                continue;
            }
        }
    }
}
