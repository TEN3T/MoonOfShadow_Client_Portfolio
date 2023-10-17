using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const string BOUNCE_PATH = "Prefabs/InGame/Skill/Bounce";

    [SerializeField] AudioClip shootAudioClip;
    [SerializeField] AudioClip hitAudioClip;
    [SerializeField] AudioClip destroyAudioClip;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    protected Rigidbody2D rigid;
    protected Collider2D projectileCollider;
    protected int bounceCount;
    protected bool isMetastasis = false;

    //public float totalDamage { get; private set; }
    public ActiveData skillData { get; private set; }

    private void Awake()
    {
        if (!TryGetComponent(out projectileCollider))
        {
            projectileCollider = GetComponentInChildren<Collider2D>();
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.GetComponent<Renderer>().sortingLayerName = LayerConstant.SPAWNOBJECT.ToString();
        animator = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        if (projectileCollider != null)
        {
            projectileCollider.enabled = true;
        }
        //발사 사운드
        if (shootAudioClip != null)
        {
            SoundManager.Instance.PlayAudioClip("Skill", shootAudioClip);
        }
    }

    public void SetAnimation(Sprite sprite, RuntimeAnimatorController controller)
    {
        spriteRenderer.sprite = sprite;
        animator.runtimeAnimatorController = controller;
    }

    public virtual void SetProjectile(ActiveData skillData)
    {
        this.skillData = skillData;
        if (projectileCollider != null)
        {
            projectileCollider.isTrigger = true;
        }
        
        for (int i = 0; i < this.skillData.skillEffect.Count; i++)
        {
            if (this.skillData.skillEffect[i] == SKILLCONSTANT.SKILL_EFFECT.BOUNCE)
            {
                rigid = GetComponent<Rigidbody2D>();
                projectileCollider.sharedMaterial = ResourcesManager.Load<PhysicsMaterial2D>(BOUNCE_PATH);
                bounceCount = Convert.ToInt32(this.skillData.skillEffectParam[i]);
                projectileCollider.isTrigger = false;
            }

            if (this.skillData.skillEffect[i] == SKILLCONSTANT.SKILL_EFFECT.METASTASIS)
            {
                rigid = GetComponent<Rigidbody2D>();
                bounceCount = Convert.ToInt32(this.skillData.skillEffectParam[i]);
                isMetastasis = true;
                projectileCollider.isTrigger = false;
            }
        }

        //transform.localScale *= this.skillData.projectileSizeMulti;
        //this.totalDamage = skillData.damage;
    }

    public void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    public void CollisionRadius(float radius)
    {
        if (projectileCollider != null)
        {
            ((CircleCollider2D)projectileCollider).radius = radius;
        }
    }

    public void CollisionPower(bool flag)
    {
        if (projectileCollider != null)
        {
            projectileCollider.enabled = flag;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Monster monster))
        {
            monster.Hit(skillData.damage);
            SkillEffect(monster);
            //충돌 사운드
            if (hitAudioClip != null)
            {
                SoundManager.Instance.PlayAudioClip("Skill", hitAudioClip);
            }

            if (!skillData.isPenetrate)
            {
                Remove();
            }
            DebugManager.Instance.PrintDebug("[TEST]: Hit");
        }
    }

    //private void OnBecameInvisible()
    //{
    //    Invoke("Remove", skillData.duration * 2);
    //}

    protected void Remove()
    {
        SkillManager.Instance.DeSpawnProjectile(this);
        //소멸 사운드
        if (destroyAudioClip != null)
        {
            SoundManager.Instance.PlayAudioClip("Skill", destroyAudioClip);
        }
    }

    protected void SkillEffect(Monster target)
    {
        int count = skillData.skillEffect.Count;
        for (int i = 0; i < count; i++)
        {
            float param = float.Parse(skillData.skillEffectParam[i]);
            switch (skillData.skillEffect[i])
            {
                case SKILL_EFFECT.EXPLORE:
                    Explore(param);
                    break;
                case SKILL_EFFECT.MOVEUP:
                    StartCoroutine(MoveUp(param));
                    break;
                case SKILL_EFFECT.DRAIN:
                    Drain(param);
                    break;
                case SKILL_EFFECT.STUN:
                case SKILL_EFFECT.SLOW:
                case SKILL_EFFECT.KNOCKBACK:
                case SKILL_EFFECT.EXECUTE:
                case SKILL_EFFECT.RESTRAINT:
                case SKILL_EFFECT.PULL:
                    target.SkillEffectActivation(skillData.skillEffect[i], param);
                    break;
                case SKILL_EFFECT.SPAWNMOB:
                    if (!target.gameObject.activeInHierarchy)
                    {
                        SkillManager.Instance.CoroutineStarter(SpawnMob(param));
                    }
                    break;
                default:
                    DebugManager.Instance.PrintDebug("[ERROR]: 없는 스킬 효과입니다");
                    break;
            }
        }
    }

    private void Explore(float n)
    {
        if (UnityEngine.Random.Range(0, 100) < n)
        {
            List<Transform> targets = Scanner.RangeTarget(transform, skillData.splashRange, (int)LayerConstant.MONSTER);
            foreach (Transform target in targets)
            {
                if (target.TryGetComponent(out Monster monster))
                {
                    monster.monsterData.SetCurrentHp(monster.monsterData.currentHp - (int)skillData.damage);
                }
            }
        }
    }

    private IEnumerator MoveUp(float n)
    {
        GameManager.Instance.player.playerManager.playerData.MoveSpeedModifier(n * 0.01f);
        yield return new WaitForSeconds(skillData.duration);
        GameManager.Instance.player.playerManager.playerData.MoveSpeedModifier(-n * 0.01f);
    }

    private void Drain(float n)
    {
        float hp = skillData.damage * n * 0.01f;
        GameManager.Instance.player.playerManager.playerData.CurrentHpModifier((int)hp);
    }

    private IEnumerator SpawnMob(float n)
    {
        float x = UnityEngine.Random.Range(-skillData.attackDistance, skillData.attackDistance);
        float y = UnityEngine.Random.Range(-skillData.attackDistance, skillData.attackDistance);
        Vector2 spawnPos = new Vector2(x, y);

        string path = CSVReader.Read("MonsterTable", n.ToString(), "MonsterPrefabPath").ToString();
        Monster summoner = Instantiate(ResourcesManager.Load<Monster>(path), GameManager.Instance.player.transform);
        summoner.gameObject.layer = (int)LayerConstant.SKILL;
        summoner.transform.localPosition = spawnPos;
        summoner.gameObject.SetActive(true);

        float duration = skillData.duration / 1000.0f;
        float time = 0.1f;
        DebugManager.Instance.PrintDebug("[SpawnMob]: " + duration);
        WaitForSeconds tick = new WaitForSeconds(time);
        while (duration > 0)
        {
            if (summoner.target == null || !summoner.target.gameObject.activeInHierarchy)
            {
                Transform target = Scanner.GetTargetTransform(SKILL_TARGET.MELEE, summoner.transform, skillData.attackDistance);
                summoner.SetTarget(target, false);
            }
            yield return tick;
            duration -= time;
        }

        summoner.gameObject.SetActive(false);
    }

}
