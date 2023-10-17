
using SKILLCONSTANT;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [field: SerializeField] public MonsterData monsterData { get; private set; }

    private CapsuleCollider2D monsterCollider2;
    private CapsuleCollider2D monsterCollider;
    private Rigidbody2D monsterRigidbody;
    private Vector2 monsterDirection;

    //private bool isSlow;
    private bool spineSwitch;
    private StatusEffect statusEffect;
    private bool isPlayer;
    private BehaviorTreeManager btManager;

    private float weightX;
    private float weightY;
    private WaitForSeconds delay;
    private WaitForSeconds tick;

    private HpBar hpBar;
    private float hpBarVisibleTime;
    private WaitForFixedUpdate fixedFrame;

    private MonsterCollider attackCollider;
    private SpineManager spineManager;
    private SoundRequester soundRequester;
    private SoundSituation.SOUNDSITUATION situation;

    public int monsterId { get; set; }
    public bool isHit { get; set; }
    public bool isAttack { get; private set; }
    public Transform target { get; private set; }
    public Vector2 lookDirection { get; private set; } //바라보는 방향


    private void Awake()
    {
        statusEffect = new StatusEffect();
        attackCollider = GetComponentInChildren<MonsterCollider>();
        monsterCollider2 = transform.Find("Collision").GetComponent<CapsuleCollider2D>();
        monsterCollider = GetComponent<CapsuleCollider2D>();
        spineManager = GetComponent<SpineManager>();
        soundRequester = GetComponent<SoundRequester>();
        monsterRigidbody = GetComponent<Rigidbody2D>();
        tick = new WaitForSeconds(0.4f);
        hpBarVisibleTime = Convert.ToInt32(CSVReader.Read("BattleConfig", "HpBarVisibleTime", "ConfigValue")) / 1000.0f;
        fixedFrame = new WaitForFixedUpdate();
    }

    private void Start()
    {
        btManager = new BehaviorTreeManager(SetAI(monsterData.attackType));
        spineManager.SetAnimation("Idle", true);
        attackCollider.SetAttackDistance(monsterData.atkDistance);

        if (monsterData.atkDistance <= 1.0f)    //근거리
        {
            weightX = attackCollider.attackCollider.size.x * 0.3f;
            weightY = monsterCollider.size.y * 0.3f;
        }
        else    //원거리
        {
            weightX = monsterData.atkDistance;
            weightY = monsterData.atkDistance;
        }
    }

    private void FixedUpdate()
    {
        if (spineSwitch)
        {
            btManager.Active();
        }

        //if (hpBar != null)
        //{
        //    hpBar.HpBarSetting(transform.position, monsterData.currentHp, monsterData.hp);
        //}
        //else
        //{
            
        //    hpBar = (HpBar)UIPoolManager.Instance.SpawnUI("HpBar", PlayerUI.Instance.transform.Find("HpBarUI"), transform.position);
        //}

        monsterRigidbody.velocity = Vector2.zero;
    }

    public void SpawnSet(float hpCoefficient, float attackCoefficient)
    {
        Physics2D.IgnoreCollision(monsterCollider, monsterCollider2);
        
        monsterCollider.enabled = true;
        monsterCollider.enabled = true;
        monsterDirection = Vector2.zero;
        lookDirection = Vector2.right;
        situation = SoundSituation.SOUNDSITUATION.IDLE;
        MonsterDataSetting(monsterId.ToString(), hpCoefficient, attackCoefficient);
        delay = new WaitForSeconds(1.0f / monsterData.atkSpeed);

        isAttack = false;
        isHit = false;
        spineSwitch = true;
        //isSlow = false;
    }

    #region AI
    private Node SetAI(AttackTypeConstant attackType)
    {
        switch (attackType)
        {
            case AttackTypeConstant.Bold:
                return BoldAI();
            case AttackTypeConstant.Shy:
                return ShyAI();
            default:
                return null;
        }
    }

    private Node BoldAI()
    {
        return new SelectorNode
                (new List<Node>()
                {
                    new SequenceNode
                    (new List<Node>()
                    {
                        new ActionNode(IsAttack),
                        new ActionNode(IsAttackable),
                        new ActionNode(Attack)
                    }),
                    new SequenceNode
                    (new List<Node>()
                    {
                        new ActionNode(IsVisible),
                        new ActionNode(Run)
                    }),
                    new ActionNode(Idle)
                });
    }

    private Node ShyAI()
    {
        return new SelectorNode
                (new List<Node>()
                {
                    new SequenceNode
                    (new List<Node>()
                    {
                        new ActionNode(IsHit),
                        new SelectorNode
                        (new List<Node>()
                        {
                            new SequenceNode
                            (new List<Node>()
                            {
                                new ActionNode(IsAttack),
                                new ActionNode(IsAttackable),
                                new ActionNode(Attack)
                            }),
                            new ActionNode(Run)
                        })
                    }),
                    new ActionNode(Idle),
                });
    }
    #endregion

    #region AI_Logic
    private NodeConstant IsAttack()
    {
        return spineManager.GetAnimationName().Equals("Attack") ? NodeConstant.RUNNING : NodeConstant.SUCCESS;
    }

    private NodeConstant IsAttackable()
    {
        Vector2 diff = target.position - transform.position;
        //float distance = diff.magnitude;
        //if (distance <= monsterData.atkDistance && ((Mathf.Abs(diff.y) <= Mathf.Abs(weightY))))
        //{
        //    return NodeConstant.SUCCESS;
        //}
        if (((Mathf.Abs(diff.x) <= Mathf.Abs(weightX))) && ((Mathf.Abs(diff.y) <= Mathf.Abs(weightY))))
        {
            return NodeConstant.SUCCESS;
        }
        return NodeConstant.FAILURE;
    }

    private NodeConstant Attack()
    {
        monsterRigidbody.velocity = Vector3.zero;
        if (!isAttack)
        {
            spineManager.SetAnimation("Attack", false);
            spineManager.AddAnimation("Idle", true);
            StartCoroutine("AttackDelay");
        }
        return NodeConstant.SUCCESS;
    }

    private NodeConstant IsVisible()
    {
        return (target.position - transform.position).magnitude <= monsterData.viewDistance ? NodeConstant.SUCCESS : NodeConstant.FAILURE;
    }

    private NodeConstant Run()
    {
        if (isAttack)
        {
            isAttack = false;
            StopCoroutine("AttackDelay");
        }

        Vector2 diff = target.position - transform.position;
        float distance = diff.magnitude;

        if (distance <= monsterData.atkDistance)
        {
            return NodeConstant.SUCCESS;
        }

        spineManager.SetAnimation("Run", true, 0, monsterData.moveSpeed);
        monsterDirection = diff.normalized;
        spineManager.SetDirection(transform, monsterDirection);
        monsterRigidbody.MovePosition(monsterRigidbody.position + (monsterDirection * monsterData.moveSpeed * Time.fixedDeltaTime));
        //monsterRigidbody.velocity = monsterDirection * monsterData.moveSpeed;
        return NodeConstant.RUNNING;
    }

    private NodeConstant Idle()
    {
        if (isAttack)
        {
            isAttack = false;
            StopCoroutine("AttackDelay");
        }

        isAttack = false;
        spineManager.SetAnimation("Idle", true);
        return NodeConstant.SUCCESS;
    }

    private NodeConstant IsHit()
    {
        return isHit ? NodeConstant.SUCCESS : NodeConstant.FAILURE;
    }

    private IEnumerator AttackDelay()
    {
        yield return tick;
        isAttack = true;
        attackCollider.AttackColliderSwitch(true);
        //yield return tick;
        yield return delay;
        attackCollider.AttackColliderSwitch(false);
        isAttack = false;
    }

    #endregion

    public void SetTarget(Transform target, bool isPlayer)
    {
        this.target = target;
        this.isPlayer = isPlayer;
    }

    public void MonsterDataSetting(string monsterId, float hpCoefficient, float attackCoefficient)
    {
        Dictionary<string, Dictionary<string, object>> monsterTable = CSVReader.Read("MonsterTable");
        if (monsterTable.ContainsKey(monsterId))
        {
            Dictionary<string, object> table = monsterTable[monsterId];
            monsterData.SetMonsterName(Convert.ToString(table["MonsterName"]));
            int hp = Convert.ToInt32(table["HP"]);
            hp += Mathf.FloorToInt(hp * hpCoefficient);
            monsterData.SetHp(hp);
            monsterData.SetCurrentHp(monsterData.hp);
            monsterData.SetSizeMultiple(float.Parse(Convert.ToString(table["SizeMultiple"])));
            int attack = Convert.ToInt32(table["Attack"]);
            attack += Mathf.FloorToInt(attack * attackCoefficient);
            monsterData.SetAttack(attack);
            monsterData.SetMoveSpeed(float.Parse(Convert.ToString(table["MoveSpeed"])));
            monsterData.SetAtkSpeed(float.Parse(Convert.ToString(table["AtkSpeed"])));
            monsterData.SetViewDistance(float.Parse(Convert.ToString(table["ViewDistance"])));
            monsterData.SetAtkDistance(float.Parse(Convert.ToString(table["AtkDistance"])));
            monsterData.SetSkillID(Convert.ToInt32(table["SkillID"]));
            monsterData.SetGroupSource(Convert.ToString(table["GroupSource"]));
            monsterData.SetGroupSourceRate(Convert.ToInt32(table["GroupSourceRate"]));
            monsterData.SetMonsterPrefabPath(Convert.ToString(table["MonsterPrefabPath"]));
            monsterData.SetAttackType((AttackTypeConstant)Enum.Parse(typeof(AttackTypeConstant), Convert.ToString(table["AttackType"])));
        }
    }

    private void DropItem()
    {
        if (UnityEngine.Random.Range(0, 10001) <= monsterData.groupSourceRate)
        {
            ItemManager.Instance.DropItems(monsterData.groupSource, transform);
        }
    }

    public void Hit(float totalDamage)
    {
        //StopCoroutine(HpBarControl());
        if (hpBar == null)
        {
            StartCoroutine(HpBarControl());
        }

        isHit = true;
        monsterData.SetCurrentHp(monsterData.currentHp - (int)(totalDamage * GameManager.Instance.player.playerManager.playerData.attack));
        if (monsterData.currentHp <= 0)
        {
            Die(true);
        }
    }

    private IEnumerator HpBarControl()
    {
        //hpBar.HpBarSwitch(true);
        //yield return hpBarVisibleTime;
        //hpBar.HpBarSwitch(false);
        hpBar = (HpBar)UIPoolManager.Instance.SpawnUI("HpBar", PlayerUI.Instance.transform.Find("HpBarUI"), transform.position);
        float time = 0.0f;
        do
        {
            hpBar.HpBarSetting(transform.position, monsterData.currentHp, monsterData.hp);
            time += Time.fixedDeltaTime;
            yield return fixedFrame;
        } while (time < hpBarVisibleTime && monsterData.currentHp > 0);
        UIPoolManager.Instance.DeSpawnUI("HpBar", hpBar);
        hpBar = null;
    }

    public void Die(bool isDrop)
    {
        soundRequester.ChangeSituation(SoundSituation.SOUNDSITUATION.DIE);
        monsterCollider.enabled = false;
        monsterCollider2.enabled = false;
        StartCoroutine(DieAnimation());

        if (isDrop)
        {
            DropItem();
        }
        //UIPoolManager.Instance.DeSpawnUI("HpBar", hpBar);
        //hpBar = null;

        GameManager.Instance.killCount++;
    }

    private IEnumerator DieAnimation()
    {
        spineSwitch = false;
        try
        {
            spineManager.SetAnimation("Death", false);
        }
        catch
        {
            DebugManager.Instance.PrintDebug("[ERROR]: 스파인에 죽는 애니메이션이 없는 몬스터입니다");
        }
        yield return new WaitForSeconds(1.0f);
        MonsterSpawner.Instance.DeSpawnMonster(this);
    }

    #region SKILL_EFFECT
    public void SkillEffectActivation(SKILL_EFFECT effect, float param)
    {
        this.SkillEffectActivation(effect, param, 1.0f);
    }

    public void SkillEffectActivation(SKILL_EFFECT effect, float param, float sec)
    {
        isHit = true;
        if (gameObject.activeInHierarchy)
        {
            switch (effect)
            {
                case SKILL_EFFECT.STUN:
                    StartCoroutine(Stun(param));
                    break;
                case SKILL_EFFECT.SLOW:
                    StartCoroutine(Slow(param, sec));
                    break;
                case SKILL_EFFECT.KNOCKBACK:
                    StartCoroutine(KnockBack(param));
                    break;
                case SKILL_EFFECT.EXECUTE:
                    Execute(param);
                    break;
                case SKILL_EFFECT.RESTRAINT:
                    StartCoroutine(Restraint(param));
                    break;
                case SKILL_EFFECT.PULL:
                    StartCoroutine(Pull(param));
                    break;
                default:
                    DebugManager.Instance.PrintDebug("[ERROR]: 없는 스킬 효과입니다");
                    break;
            }
        }
    }

    private IEnumerator Stun(float n)
    {
        if (spineSwitch)
        {
            spineSwitch = false;
            float originSpeed = monsterData.moveSpeed;
            monsterData.SetMoveSpeed(0.0f);
            spineManager.SetAnimation("Idle", true);
            yield return new WaitForSeconds(n);
            monsterData.SetMoveSpeed(originSpeed);
            spineSwitch = true;
        }
    }

    private IEnumerator Slow(float n, float sec)
    {
        if (statusEffect.IsStatusEffect(STATUS_EFFECT.SLOW))
        {
            statusEffect.AddStatusEffect(STATUS_EFFECT.SLOW);
            float originSpeed = monsterData.moveSpeed;
            monsterData.SetMoveSpeed(originSpeed * n * 0.01f);
            yield return new WaitForSeconds(sec);
            monsterData.SetMoveSpeed(originSpeed);
            statusEffect.RemoveStatusEffect(STATUS_EFFECT.SLOW);
        }

        //if (!isSlow)
        //{
        //    isSlow = true;
        //    float originSpeed = monsterData.moveSpeed;
        //    monsterData.SetMoveSpeed(originSpeed * n * 0.01f);
        //    yield return new WaitForSeconds(sec);
        //    monsterData.SetMoveSpeed(originSpeed);
        //    isSlow = false;
        //}
    }

    private IEnumerator Slow(float n)
    {
        yield return this.Slow(n, 1.0f);
    }

    private IEnumerator KnockBack(float n)
    {
        if (spineSwitch)
        {
            spineSwitch = false;
            Vector2 diff = transform.position - target.position;
            monsterRigidbody.AddRelativeForce(diff.normalized * n * 0.0002f, ForceMode2D.Impulse);
            yield return tick;
            spineSwitch = true;
        }
    }

    private void Execute(float n)
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) < n)
        {
            Die(true);
        }
    }

    private IEnumerator Restraint(float n)
    {
        if (statusEffect.IsStatusEffect(STATUS_EFFECT.RESTRAINT))
        {
            statusEffect.AddStatusEffect(STATUS_EFFECT.RESTRAINT);
            float originSpeed = monsterData.moveSpeed;
            monsterData.SetMoveSpeed(0.0f);
            yield return new WaitForSeconds(n);
            monsterData.SetMoveSpeed(originSpeed);
            statusEffect.RemoveStatusEffect(STATUS_EFFECT.RESTRAINT);
        }
    }

    private IEnumerator Pull(float n)
    {
        if (spineSwitch)
        {
            spineSwitch = false;
            Vector2 diff = target.position - transform.position;
            monsterRigidbody.AddRelativeForce(diff.normalized * n * 0.0002f, ForceMode2D.Impulse);
            yield return tick;
            spineSwitch = true;
        }
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
            this.Hit(-(int)dotDamage);
            yield return sec;
        }
        statusEffect.RemoveStatusEffect(STATUS_EFFECT.FIRE);
    }
    #endregion
}