using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum CutSceneType
{
    BLINK,
    FADEOUT,
}

public class CutScenes : MonoBehaviour
{
    [SerializeField] private CutSceneType type = CutSceneType.BLINK;
    [SerializeField] private float stopPosX = 550.0f;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float animationSpeed = 0.01f;
    [SerializeField] private float animationTime = 2.5f;

    private RectTransform rect;
    private Image cutImage;

    private bool slow;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        cutImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (rect.anchoredPosition.x >= stopPosX && slow)
        {
            slow = false;
            StartCoroutine(CutScenesAnimation());
        }

        if (slow)
        {
            transform.Translate(Vector2.right * moveSpeed * 10000.0f * Time.unscaledDeltaTime);
            moveSpeed *= 0.99f;
        }
    }

    private void Start()
    {
        rect.anchoredPosition = new Vector2(-Screen.width, rect.anchoredPosition.y);
        slow = true;
        Time.timeScale = 0.0f;
    }

    private IEnumerator CutScenesAnimation()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        switch (type)
        {
            case CutSceneType.BLINK:
                WaitForSecondsRealtime blinkTime = new WaitForSecondsRealtime(animationSpeed);
                float intervalTime = animationTime;

                for (int i = 0; i < 3; i++)
                {
                    cutImage.fillAmount = 0.0f;
                    yield return blinkTime;
                    cutImage.fillAmount = 1.0f;
                    yield return new WaitForSecondsRealtime(intervalTime);
                    intervalTime *= 0.25f;
                }
                break;
            case CutSceneType.FADEOUT:
                while (cutImage.fillAmount > 0)
                {
                    cutImage.fillAmount -= Time.unscaledDeltaTime * animationSpeed;
                    yield return null;
                }
                yield return new WaitForSecondsRealtime(animationTime);
                break;
            default:
                DebugManager.Instance.PrintError("[CutScenes]: 존재하지 않는 컷씬입니다");
                break;
        }

        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync(gameObject.scene.name);
    }

}
