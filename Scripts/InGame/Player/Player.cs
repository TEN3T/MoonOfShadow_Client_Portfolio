using SKILLCONSTANT;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;
    private Vector2 playerDirection;
    private PlayerItem playerItem;
    private Transform shadow;
    private SpineManager spineManager;
    private WaitForSeconds invincibleTime;
    private StatusEffect statusEffect;

    private HpBar hpBar;
    private Vector3 hpBarPos = new Vector3(0.0f, -0.6f, 0.0f);

    public Transform character { get; private set; }
    public PlayerManager playerManager { get; private set; }
    public Vector2 lookDirection { get; private set; } //바라보는 방향
    public int exp { get; private set; }
    public int level { get; private set; }
    public int needExp { get; private set; }

    #region Mono
    private void Awake()
    {
        statusEffect = new StatusEffect();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerDirection = Vector3.zero;
        lookDirection = Vector3.left;
        character = transform.Find("Character");
        shadow = transform.Find("Shadow");
        spineManager = GetComponent<SpineManager>();
        playerManager = GetComponentInChildren<PlayerManager>();

        playerItem = GetComponentInChildren<PlayerItem>();
        gameObject.tag = "Player";
        invincibleTime = new WaitForSeconds(Convert.ToInt32(Convert.ToString(CSVReader.Read("BattleConfig", "InvincibleTime", "ConfigValue"))) / 1000.0f);
    }

    private void Start()
    {
        level = 1;
        needExp = Convert.ToInt32(CSVReader.Read("LevelUpTable", (level + 1).ToString(), "NeedExp"));
        hpBar = (HpBar)UIPoolManager.Instance.SpawnUI("HpBar", PlayerUI.Instance.transform.Find("HpBarUI"), transform.position);
        AudioSetting();
        //hpBar.HpBarSwitch(true);
    }

    /*
     *키보드 입력이랑 움직이는 부분은 안정성을 위해 분리시킴
     *Update -> 키보드 input
     *FixedUpdate -> movement
     */
    private void Update()
    {
        KeyDir();
        hpBar.HpBarSetting(transform.position + hpBarPos, playerManager.playerData.currentHp, playerManager.playerData.hp);
    }

    private void FixedUpdate()
    {
        Move();
    }

    #endregion

    #region Movement, Animation
    //키보드 입력을 받아 방향을 결정하는 함수
    private void KeyDir()
    {
        playerDirection.x = Input.GetAxisRaw("Horizontal");
        playerDirection.y = Input.GetAxisRaw("Vertical");

        if (playerDirection != Vector2.zero)
        {
            lookDirection = playerDirection; //쳐다보는 방향 저장
        }
    }

    private void Move()
    {
        spineManager.SetDirection(character, playerDirection);
        spineManager.SetDirection(shadow, playerDirection);
        playerRigidbody.velocity = playerDirection.normalized * playerManager.playerData.moveSpeed;

        if (playerRigidbody.velocity == Vector2.zero)
        {
            Vector3 pos = transform.localPosition;
            pos.y += 0.00005f;
            transform.localPosition = pos;
            spineManager.SetAnimation("Idle", true);
        }
        else
        {
            spineManager.SetAnimation("Run", true, 0, playerManager.playerData.moveSpeed);
        }
    }
    #endregion

    #region Level
    //public void GetExp(int exp)
    //{
    //    this.exp += playerManager.playerData.ExpBuff(exp);

    //    if (this.exp >= needExp)
    //    {
    //        LevelUp();
    //    }
    //}

    public void UpdateGetItemRange()
    {
        playerItem.UpdateItemRange();
    }

    //private void LevelUp()
    //{
    //    exp -= needExp;
    //    needExp = Convert.ToInt32(CSVReader.Read("LevelUpTable", (++level + 1).ToString(), "NeedExp"));
    //    GameManager.Instance.playerUi.LevelTextChange(level);
    //    GameManager.Instance.playerUi.SkillSelectWindowOpen();
    //}
    #endregion

    #region Collider
    public IEnumerator Invincible()
    {
        spineManager.SetColor(Color.red);
        yield return invincibleTime;
        spineManager.SetColor(Color.white);
    }
    #endregion

    #region Sound
    private void AudioSetting()
    {
        SoundManager.Instance.AddAudioSource("Skill", GetComponent<AudioSource>(), "EFFECT_SOUND");
    }
    #endregion

    #region STATUS_EFFECT
    public IEnumerator FireDot(float time, float dotDamage)
    {
        if (statusEffect.IsStatusEffect(STATUS_EFFECT.FIRE))
        {
            yield break;
        }

        statusEffect.AddStatusEffect(STATUS_EFFECT.FIRE);
        WaitForSeconds sec = new WaitForSeconds(1.0f);
        for (int i = 0; i < time; i++)
        {
            StartCoroutine(Invincible());
            this.playerManager.playerData.CurrentHpModifier(-(int)dotDamage);
            yield return sec;
        }
        statusEffect.RemoveStatusEffect(STATUS_EFFECT.FIRE);
    }

    public IEnumerator Slow(float time, float value)
    {
        if (statusEffect.IsStatusEffect(STATUS_EFFECT.SLOW))
        {
            yield break;
        }

        statusEffect.AddStatusEffect(STATUS_EFFECT.SLOW);
        float decreaseValue = value * this.playerManager.playerData.moveSpeed;
        this.playerManager.playerData.MoveSpeedModifier(-decreaseValue);
        yield return new WaitForSeconds(time);
        this.playerManager.playerData.MoveSpeedModifier(decreaseValue);
        statusEffect.RemoveStatusEffect(STATUS_EFFECT.SLOW);
    }
    #endregion
}
