using BFM;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : SingletonBehaviour<PlayerStatusUI>
{
    public TextMeshProUGUI levelText { get; private set; }
    public Image iconImage { get; private set; }
    
    private Transform nameBox;
    //private Transform skillBox;
    //private Transform[] icons;

    //private List<SkillBoxIcon> boxIcons = new List<SkillBoxIcon>();
    //public Dictionary<int, SkillBoxIcon> boxIcons { get; private set; } = new Dictionary<int, SkillBoxIcon>();

    protected override void Awake()
    {
        nameBox = transform.Find("NameBox");
        //skillBox = transform.Find("SkillBox");
        //SkillBoxSetting();

        levelText = nameBox.GetComponentInChildren<TextMeshProUGUI>();
        iconImage = nameBox.Find("CharacterIcon").GetComponent<Image>();
    }

    //0번 궁극기, 1번 기본스킬, 2~8번 추가스킬칸
    //private void SkillBoxSetting()
    //{
    //    icons = new Transform[9];
    //    for (int i = 0; i < icons.Length; i++)
    //    {
    //        icons[i] = skillBox.GetChild(i);
    //    }
    //}

    //public void SkillIconInit(string iconPath, int skillNum)
    //{
    //    SkillBoxIcon boxIcon = (SkillBoxIcon)UIPoolManager.Instance.SpawnUI("SkillBoxIcon", icons[skillNum]);
    //    boxIcon.UiSetting(iconPath);
    //    boxIcons.Add(skillNum, boxIcon);
    //}

    //public void DimmedColorChange(Color color)
    //{
    //    foreach (int key in boxIcons.Keys)
    //    {
    //        boxIcons[key].DimmedColorSetting(color);
    //    }
    //}
}
