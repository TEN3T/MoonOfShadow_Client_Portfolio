using BFM;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : SingletonBehaviour<Timer>
{
    private TextMeshProUGUI timeText;

    public int currentTime { get; private set; } = 0;

    protected override void Awake()
    {
        timeText = GetComponent<TextMeshProUGUI>();
        //timeText.color = Color.white;
        //timeText.fontSize = 45;
    }

    public void TimerSwitch(bool btn)
    {
        if (btn)
        {
            StartCoroutine(TimerBtn());
        }
        else
        {
            StopCoroutine(TimerBtn());
        }
    }

    private IEnumerator TimerBtn()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            currentTime += 1000;
            timeText.text = $"{currentTime / 1000 / 60:00}  {currentTime / 1000 % 60:00}";
        }
    }
}
