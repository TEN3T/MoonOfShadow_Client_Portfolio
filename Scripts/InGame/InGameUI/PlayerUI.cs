using BFM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUI : SingletonBehaviour<PlayerUI>
{
    private GameOverUI gameOverUi;
    private PlayerStatusUI statusUi;
    private LevelUpUI levelUi;

    public ExpBar expBar { get; private set; }
    public SkillBoxUI skillBoxUi { get; private set; }

    [SerializeField] private Color color = new Color(0, 0, 0, 0.75f);

    public int skillCount
    {
        get { return activeSkillCount + passiveSkillCount; }
        private set { }
    }
    public int activeSkillCount { get; set; } = 0;
    public int passiveSkillCount { get; set; } = 0;

    protected override void Awake()
    {
        gameOverUi = GetComponentInChildren<GameOverUI>();
        statusUi = GetComponentInChildren<PlayerStatusUI>();
        levelUi = GetComponentInChildren<LevelUpUI>();
        skillBoxUi = GetComponentInChildren<SkillBoxUI>();
        expBar = GetComponentInChildren<ExpBar>();
    }

    private void Start()
    {
        gameOverUi.gameObject.SetActive(false);
        levelUi.gameObject.SetActive(false);
    }

    private void Update()
    {
        //PlayerStatusUI.Instance.DimmedColorChange(color);
        skillBoxUi.DimmedColorChange(color);
    }

    public void NameBoxSetting(string path)
    {
        statusUi.levelText.text = $"Lv.{1}";
        statusUi.iconImage.sprite = ResourcesManager.Load<Sprite>(path);
    }

    public void LevelTextChange(int level)
    {
        statusUi.levelText.text = $"Lv.{level}";
    }

    public IEnumerator SkillSelectWindowOpen()
    {
        levelUi.gameObject.SetActive(true);
        //Time.timeScale = 0f;
        GameManager.Instance.Pause();
        levelUi.skills.Clear();
        levelUi.SkillBoxInit(3);

        while (!levelUi.isSelect)
        {
            yield return null;
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverUi.gameObject.SetActive(true);
    }
}
