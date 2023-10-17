using BFM;
using SKILLCONSTANT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TempStatus
{
    public int hp;
    public int currentHp;
    public int hpRegen;
    public int shield;
    public float attack;
    public float criRatio;
    public float criDamage;
    public float coolDown;
    public float moveSpeed;
    public float getItemRange;
    public float expBuff;
    public float armor;

    public int projectileAdd;
    public float projectileSize;
    public float projectileSplash;
    public float projectileSpeed;

    public void DataSetter(
        int hp, int currentHp, int hpRegen, int shield,
        float attack, float criRatio, float criDamage, float coolDown, float moveSpeed, float getItemRange, float expBuff, float armor,
        int projectileAdd, float projectileSize, float projectileSplash, float projectileSpeed)
    {
        this.hp = hp;
        this.currentHp = currentHp;
        this.hpRegen = hpRegen;
        this.shield = shield;
        this.attack = attack;
        this.criRatio = criRatio;
        this.coolDown = coolDown;
        this.moveSpeed = moveSpeed;
        this.getItemRange = getItemRange;
        this.expBuff = expBuff;
        this.armor = armor;
        this.projectileAdd = projectileAdd;
        this.projectileSize = projectileSize;
        this.projectileSplash = projectileSplash;
        this.projectileSpeed = projectileSpeed;
    }
}

//싱글톤 사용
public class PlayerManager : MonoBehaviour
{
    private const float HP_REGEN_PER_SECOND = 1f;

    private float fraction;             //분율
    private int coolTimeConstant;     //재사용대기시간감소상수
    private int coolTimeCoefficient;  //재사용대기시간감소최대치조절계수

    private Player player;
    private Collider2D playerCollider;

    public PlayerData playerData { get; private set; } = new PlayerData(); //플레이어의 데이터를 가지는 객체

    //임시 스탯 확인용
    [field: SerializeField] private TempStatus tempStatusWindow = new TempStatus();

    private void Update()
    {
        tempStatusWindow.DataSetter(
            playerData.hp, playerData.currentHp, playerData.hpRegen, playerData.shield, playerData.attack, playerData.criRatio,
            playerData.criDamage, playerData.coolDown, playerData.moveSpeed, playerData.getItemRange, playerData.expBuff, playerData.armor,
            playerData.projectileAdd, playerData.projectileSize.param, playerData.projectileSplash.param, playerData.projectileSpeed.param);
    }

    #region
    private void Awake()
    {


        ConfigSetting();
        player = GetComponentInParent<Player>();
        playerCollider = GetComponent<Collider2D>();
        playerCollider.isTrigger = true;
    }

    private void OnEnable()
    {
        PlayerSetting(FindCharacter(Convert.ToString(GameManager.Instance.GetPlayerId())));
        StartCoroutine(HpRegeneration());
    }

    private void Start()
    {
        Dictionary<string, object> characterData = FindCharacter(Convert.ToString(GameManager.Instance.GetPlayerId()));

        try
        {
            int skillId = Convert.ToInt32(characterData["SkillID_01"]);
            SkillManager.Instance.SkillAdd(skillId, player.transform, 0);
        }
        catch
        {
            DebugManager.Instance.PrintDebug("[ERROR]: 테이블에 유효한 데이터가 들어있는지 체크해주세요. (SkillID_01)");
        }

        try
        {
            int skillId = Convert.ToInt32(characterData["SkillID_02"]);
            SkillManager.Instance.SkillAdd(skillId, player.transform, 1);
        }
        catch
        {
            DebugManager.Instance.PrintDebug("[ERROR]: 테이블에 유효한 데이터가 들어있는지 체크해주세요. (SkillID_02)");
        }
        //int skillId2 = Convert.ToInt32(characterData["SkillID_02"]);
        //SkillManager.Instance.SkillAdd(skillId2, player.transform, 1);
    }

    private void ConfigSetting()
    {
        fraction = 1.0f / Convert.ToInt32(CSVReader.Read("BattleConfig", "Fraction", "ConfigValue"));
        coolTimeConstant = Convert.ToInt32(CSVReader.Read("BattleConfig", "CoolTimeOffset", "ConfigValue"));
        coolTimeCoefficient = Convert.ToInt32(CSVReader.Read("BattleConfig", "CoolTimeMax", "ConfigValue"));
    }

    //캐릭터에 스탯 부여
    public void PlayerSetting(Dictionary<string, object> characterData)
    {
        if (characterData == null)
        {
            //없는 캐릭터일 경우 에러메시지 송출
            Debug.LogError("존재하지 않는 캐릭터입니다");
            return;
        }

        playerData.SetCharacterName(Convert.ToString(characterData["CharacterName"]));
        playerData.SetIconImage(Convert.ToString(characterData["IconImage"]));
        playerData.SetHp(Convert.ToInt32(characterData["HP"]));
        playerData.SetCurrentHp(playerData.hp);
        playerData.SetAttack(Convert.ToInt32(characterData["Attack"]));
        playerData.SetCriRatio(Convert.ToInt32(characterData["CriRatio"]));
        playerData.SetCriDamage(float.Parse(Convert.ToString(characterData["CriDamage"])));
        playerData.SetCoolDown(Convert.ToInt32(characterData["CoolDown"]));
        playerData.SetHpRegen(Convert.ToInt32(characterData["HPRegen"]));
        playerData.SetShield(Convert.ToInt32(characterData["Shield"]));
        playerData.SetProjectileAdd(Convert.ToInt32(characterData["ProjectileAdd"]));
        playerData.SetMoveSpeed(float.Parse(Convert.ToString(characterData["MoveSpeed"])));
        playerData.SetGetItemRange(float.Parse(Convert.ToString(characterData["GetItemRange"])));
        playerData.SetExpBuff(0);
        playerData.SetArmor(0);

        playerData.SetLevel(1);
        playerData.SetNeedExp(Convert.ToInt32(CSVReader.Read("LevelUpTable", "2", "NeedExp")));
        GameManager.Instance.playerUi.expBar.SetExpBar(playerData.exp, playerData.needExp);
        GameManager.Instance.playerUi.LevelTextChange(playerData.level);
    }

    //캐릭터 id와 일치하는 행(Dictionary)을 리턴
    private Dictionary<string, object> FindCharacter(string characterId)
    {
        Dictionary<string, Dictionary<string, object>> playerStatusTable = CSVReader.Read("CharacterTable");

        //foreach -> ContainsKey로 변경
        //가독성을 위함, 유의미한 성능차이 없음
        if (playerStatusTable.ContainsKey(characterId))
        {
            return playerStatusTable[characterId];
        }
        //찾는 캐릭터가 없을 경우 null 리턴
        return null;
    }
    #endregion


    /*캐릭터 로직 관련*/
    #region Character Logic
    public float GetCoolDown(float coolTime)
    {
        return coolTime * ((float)Math.Pow(playerData.coolDown, 2) / ((float)Math.Pow(playerData.coolDown, 2) + coolTimeConstant)) * coolTimeCoefficient * fraction;
    }

    //체젠 함수
    //기본적으로 가지고 있는(get으로 가져올 수 있는) hp를 최대 체력이라 지정
    //현재 체력은 currentHp로 따로 빼서 사용
    private IEnumerator HpRegeneration()
    {
        while (true)
        {
            //정해진 초(HP_REGEN_PER_SECOND)마다 실행
            yield return new WaitForSeconds(HP_REGEN_PER_SECOND);

            //최대 체력보다 현재 체력이 낮을 때 체젠량만큼 회복
            if (playerData.currentHp < playerData.hp && playerData.currentHp >= 0)
            {
                playerData.HpRegen();
            }
        }
    }

    //크리티컬 판별 함수
    private bool IsCritical()
    {
        return UnityEngine.Random.Range(0.0f, 1.0f) <= (playerData.criRatio * fraction);
    }

    //최종적으로 몬스터에게 가하는 데미지 계산 함수
    //스킬 데미지 계산 방식에 따라 따로 계산
    //오리지널데미지 = 공격력 + or * 스킬피해
    //크리티컬데미지 = 오리지널데미지 * 크리티컬데미지
    //일단 스킬피해 제외하고 구현
    public float TotalDamage(float skillDamage)
    {
        if (IsCritical())
        {
            return playerData.criDamage * skillDamage;
        }
        return playerData.attack * skillDamage;
    }

    //쉴드 사용 함수
    //쉴드가 존재할경우 1감소시키고 데미지를 1로 반환
    //쉴드가 없을 경우 받은 데미지 그대로 리턴
    public float IsHit(float monsterDamage)
    {
        if (playerData.shield > 0)
        {
            playerData.ShieldModifier(-1);
            return 1;
        }

        return monsterDamage * (1.0f - playerData.armor);
    }
    #endregion

    /*몬스터 충돌*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Skill skill))
        {
            //StartCoroutine(player.Invincible());
            //playerData.CurrentHpModifier(-IsShield(/*스킬데미지*/));
        }
        else
        {
            try
            {
                Monster monster = collision.GetComponentInParent<Monster>();
                StartCoroutine(player.Invincible());
                DebugManager.Instance.PrintDebug("[충돌테스트]: 윽!");
                playerData.CurrentHpModifier((int)-IsHit(monster.monsterData.attack));
            }
            catch
            {
                DebugManager.Instance.PrintDebug("[충돌테스트]: 윽아님");
            }
        }
    }


}
