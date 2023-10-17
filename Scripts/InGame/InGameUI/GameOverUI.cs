using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private Button reStartBtn;
    private Image fadeBox;

    private void Awake()
    {
        fadeBox = transform.Find("FadeBox").GetComponent<Image>();
        fadeBox.gameObject.SetActive(false);

        reStartBtn = GetComponentInChildren<Button>();
        reStartBtn.onClick.AddListener(ReStart);
    }

    private void ReStart()
    {
        fadeBox.gameObject.SetActive(true);
        reStartBtn.interactable = false;
        StartCoroutine(SceneFadeOut());
    }

    private IEnumerator SceneFadeOut()
    {
        float alpha = 0f;
        WaitForSecondsRealtime sec = new WaitForSecondsRealtime(0.01f);
        while (alpha < 1.0f)
        {
            alpha += 0.01f;
            yield return sec;
            Color color = fadeBox.color;
            color.a = alpha;
            fadeBox.color = color;
        }
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("UI", LoadSceneMode.Single);
    }
}
