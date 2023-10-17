using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : InGameUI
{
    private Image hpBar;
    private Vector3 hpBarPos = new Vector3(0.0f, -0.6f, 0.0f);

    private void Awake()
    {
        hpBar = GetComponent<Image>();
        //hpBar.enabled = false;
    }

    //private void Update()
    //{
    //    transform.position = CameraManager.Instance.cam.WorldToScreenPoint(GameManager.Instance.player.transform.position + new Vector3(0, -0.6f, 0));
    //    hpBar.fillAmount = (float)GameManager.Instance.player.playerManager.playerData.currentHp / GameManager.Instance.player.playerManager.playerData.hp;
    //}

    public void HpBarSetting(Vector3 pos, float currentHp, float maxHp)
    {
        transform.position = CameraManager.Instance.cam.WorldToScreenPoint(pos);
        hpBar.fillAmount = currentHp / maxHp;
    }

    //public void HpBarSwitch(bool flag)
    //{
    //    hpBar.enabled = flag;
    //}
}
