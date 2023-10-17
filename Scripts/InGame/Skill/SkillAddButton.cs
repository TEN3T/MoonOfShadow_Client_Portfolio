using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillAddButton : MonoBehaviour
{
    private TMP_InputField inputField;
    private Button button;

    private void Start()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        button = GetComponentInChildren<Button>();

        button.onClick.AddListener(test);
    }


    private void test()
    {
        int skillId = int.Parse(inputField.text);
        int skillNum = skillId / 10000 == 1 ? PlayerUI.Instance.activeSkillCount : PlayerUI.Instance.passiveSkillCount;
        SkillManager.Instance.SkillAdd(skillId, GameManager.Instance.player.transform, skillNum);
    }
}
