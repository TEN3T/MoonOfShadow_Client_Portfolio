using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBoxIcon : InGameUI
{
    RectTransform rect;
    private WaitForFixedUpdate dimmedTime;

    private Image icon;
    private Image dimmed;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        icon = GetComponent<Image>();
        dimmed = transform.Find("Dimmed").GetComponent<Image>();
        dimmedTime = new WaitForFixedUpdate();
    }

    public void UiSetting(string path)
    {
        rect.sizeDelta = transform.parent.GetComponent<RectTransform>().sizeDelta;
        Sprite sprite = ResourcesManager.Load<Sprite>(path);
        icon.sprite = sprite;
        dimmed.sprite = sprite;
        DimmedColorSetting(0, 0, 0, 150);
    }

    public void DimmedColorSetting(byte r, byte g, byte b, byte a)
    {
        dimmed.color = new Color32(r, g, b, a);
    }

    public void DimmedColorSetting(Color color)
    {
        dimmed.color = color;
    }

    public IEnumerator Dimmed(float time)
    {
        time -= GameManager.Instance.player.playerManager.GetCoolDown(time);
        float cool = time;
        dimmed.fillAmount = time == 0.0f ? 0.0f : 1.0f;
        while (cool > 0.0f)
        {
            cool -= Time.fixedDeltaTime;
            dimmed.fillAmount -= Time.fixedDeltaTime / time;
            yield return dimmedTime;
        }
    }
}
