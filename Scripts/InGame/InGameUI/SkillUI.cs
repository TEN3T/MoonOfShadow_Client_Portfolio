using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : InGameUI
{
    private Image skillIcon;
    private TextMeshProUGUI skillName;
    private TextMeshProUGUI skillInfo;

    public Button btn { get; private set; }
    public int skillId { get; private set; }

    private void Awake()
    {
        btn = GetComponent<Button>();

        skillIcon = transform.Find("Icon").GetComponent<Image>();
        skillName = transform.Find("SkillName").GetComponent<TextMeshProUGUI>();
        skillInfo = transform.Find("SkillInfo").GetComponent<TextMeshProUGUI>();
    }

    public void UISetting(Dictionary<string, object> skillData)
    {
        skillIcon.sprite = ResourcesManager.Load<Sprite>(skillData["Icon"].ToString());
        skillName.text = LocalizeManager.Instance.GetText(skillData["Name"].ToString());
        skillInfo.text = LocalizeManager.Instance.GetText(skillData["Desc"].ToString());
    }

}
