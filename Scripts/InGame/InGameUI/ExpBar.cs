using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    private Image expImage;
    private TextMeshProUGUI expText;

    private void Awake()
    {
        expImage = GetComponent<Image>();

        expText = GetComponentInChildren<TextMeshProUGUI>();
        expText.color = Color.white;
        expText.fontSize = 25;
    }

    public void SetExpBar(int exp, int needExp)
    {
        expImage.fillAmount = exp / (float)needExp;
        expText.text = $"{exp}/{needExp}";
    }
}
