using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBoxUI : MonoBehaviour
{
    private Transform[] icons;

    public Dictionary<int, SkillBoxIcon> boxIcons { get; private set; } = new Dictionary<int, SkillBoxIcon>();

    private void Awake()
    {
        icons = new Transform[SkillManager.SKILL_MAX_COUNT];
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i] = transform.GetChild(i);
        }
    }

    public void SkillIconInit(string iconPath, int skillNum)
    {
        SkillBoxIcon boxIcon = (SkillBoxIcon)UIPoolManager.Instance.SpawnUI("SkillBoxIcon", icons[skillNum]);
        boxIcon.UiSetting(iconPath);
        boxIcons.Add(skillNum, boxIcon);
    }

    public void DimmedColorChange(Color color)
    {
        foreach (int key in boxIcons.Keys)
        {
            boxIcons[key].DimmedColorSetting(color);
        }
    }
}
