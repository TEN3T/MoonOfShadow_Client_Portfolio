
using System;
using System.Collections;

public class PlayerData
{
    public string characterName { get; private set; }
    public string iconImage { get; private set; }

    public int hp
    {
        get { return originHp + modifyHp; }
        private set { }
    }

    public int currentHp { get; private set; }

    public float attack
    {
        get { return originAttack + modifyAttack; }
        private set { }
    }

    public int criRatio
    {
        get { return originCriRatio + modifyCriRatio; }
        private set { }
    }

    public float criDamage
    {
        get { return originCriDamage + modifyCriDamage; }
        private set { }
    }

    public int coolDown
    {
        get { return originCoolDown + modifyCoolDown; }
        private set { }
    }

    public int hpRegen
    {
        get { return originHpRegen + modifyHpRegen; }
        private set { }
    }

    public int shield { get; private set; }

    public int projectileAdd { get; private set; }

    public float moveSpeed
    {
        get { return originMoveSpeed + modifyMoveSpeed; }
        private set { }
    }

    public float getItemRange
    {
        get { return originGetItemRange + modifyGetItemRange; }
        private set { }
    }

    public string characterPrefabPath { get; private set; }

    public float expBuff { get; private set; }

    public float armor { get; private set; }

    public int level { get; private set; }

    public int exp { get; private set; }

    public int needExp { get; private set; }

    #region PASSIVE
    public PassiveSet projectileSize { get; private set; } = new PassiveSet(0.0f, 0);
    public PassiveSet projectileSpeed { get; private set; } = new PassiveSet(0.0f, 0);
    public PassiveSet projectileSplash { get; private set; } = new PassiveSet(0.0f, 0);
    public PassiveSet projectileDistance { get; private set; } = new PassiveSet(0.0f, 0);
    public PassiveSet skillDamage { get; private set; } = new PassiveSet(0.0f, 0);

    public void SetProjectileSize(float param, SKILLCONSTANT.CALC_MODE mode)
    {
        this.projectileSize = new PassiveSet(param + this.projectileSize.param, mode);
    }

    public void SetProjectileSpeed(float param, SKILLCONSTANT.CALC_MODE mode)
    {
        this.projectileSpeed = new PassiveSet(param + this.projectileSpeed.param, mode);
    }
    public void SetProjectileSplash(float param, SKILLCONSTANT.CALC_MODE mode)
    {
        this.projectileSplash = new PassiveSet(param + this.projectileSplash.param, mode);
    }
    public void SetProjectileDistance(float param, SKILLCONSTANT.CALC_MODE mode)
    {
        this.projectileDistance = new PassiveSet(param + this.projectileDistance.param, mode);
    }
    public void SetSkillDamage(float param, SKILLCONSTANT.CALC_MODE mode)
    {
        this.skillDamage = new PassiveSet(param + this.skillDamage.param, mode);
    }

    #endregion

    private int originHp;
    private int modifyHp;
    private float originAttack;
    private float modifyAttack;
    private int originCriRatio;
    private int modifyCriRatio;
    private float originCriDamage;
    private float modifyCriDamage;
    private int originCoolDown;
    private int modifyCoolDown;
    private int originHpRegen;
    private int modifyHpRegen;
    private float originMoveSpeed;
    private float modifyMoveSpeed;
    private float originGetItemRange;
    private float modifyGetItemRange;

    //Setter - Origin Data
    #region Setter
    public void SetCharacterName(string characterName)
    {
        this.characterName = characterName;
    }

    public void SetIconImage(string iconImage)
    {
        this.iconImage = iconImage;
    }

    public void SetHp(int hp)
    {
        this.originHp = hp;
        this.modifyHp = 0;
    }

    public void SetCurrentHp(int currentHp)
    {
        this.currentHp = currentHp;
    }

    public void SetAttack(float attack)
    {
        this.originAttack = attack/100;
        this.modifyAttack = 0.0f;
    }

    public void SetCriRatio(int criRatio)
    {
        this.originCriRatio = criRatio;
        this.modifyCriRatio = 0;
    }

    public void SetCriDamage(float criDamage)
    {
        this.originCriDamage = criDamage;
        this.modifyCriDamage = 0.0f;
    }

    public void SetCoolDown(int coolDown)
    {
        this.originCoolDown = coolDown;
        this.modifyCoolDown = 0;
    }

    public void SetHpRegen(int hpRegen)
    {
        this.originHpRegen = hpRegen;
        this.modifyHpRegen = 0;
    }

    public void SetShield(int shield)
    {
        this.shield = shield;
    }

    public void SetProjectileAdd(int projectileAdd)
    {
        this.projectileAdd = projectileAdd;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.originMoveSpeed = moveSpeed;
        this.modifyMoveSpeed = 0;
    }

    public void SetGetItemRange(float getItemRange)
    {
        this.originGetItemRange = getItemRange;
        this.modifyGetItemRange = 0;
    }

    public void SetExpBuff(float expBuff)
    {
        this.expBuff = expBuff;
    }

    public void SetArmor(float armor)
    {
        this.armor = armor;
    }

    public IEnumerator ExpUp(int exp)
    {
        this.exp += this.ExpBuff(exp);
        while (this.exp >= this.needExp)
        {
            ++level;
            this.exp -= this.needExp;
            this.needExp = Convert.ToInt32(CSVReader.Read("LevelUpTable", (level + 1).ToString(), "NeedExp"));
            yield return GameManager.Instance.playerUi.SkillSelectWindowOpen();
        }
        GameManager.Instance.playerUi.expBar.SetExpBar(this.exp, this.needExp);
        GameManager.Instance.playerUi.LevelTextChange(level);
        yield return null;
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public void SetNeedExp(int needExp)
    {
        this.needExp = needExp;
    }

    #endregion

    //Modifier - Modify Data
    #region Modifier
    public void HpModifier(int hp)
    {
        this.modifyHp += hp;
        this.CurrentHpModifier(hp);
    }

    public void CurrentHpModifier(int currentHp)
    {
        this.currentHp += currentHp;
        if (this.currentHp > this.hp)
        {
            this.currentHp = this.hp;
        }
    }

    public void AttackModifier(float attack)
    {
        this.modifyAttack += attack;
    }

    public void CriRatioModifier(int criRatio)
    {
        this.modifyCriRatio += criRatio;
    }

    public void CriDamageModifier(float criDamage)
    {
        this.modifyCriDamage += criDamage;
    }

    public void CoolDownModifier(int coolDown)
    {
        this.modifyCoolDown += coolDown;
    }

    public void HpRegenModifier(int hpRegen)
    {
        this.modifyHpRegen += hpRegen;
    }

    public void ShieldModifier(int shield)
    {
        this.shield += shield;
    }

    public void ProjectileAddModifier(int projectileAdd)
    {
        this.projectileAdd += projectileAdd;
    }

    public void MoveSpeedModifier(float moveSpeed)
    {
        this.modifyMoveSpeed += moveSpeed;
    }

    public void GetItemRangeModifier(float getItemRange)
    {
        this.modifyGetItemRange += getItemRange;
    }

    public void ExpBuffModifier(float expBuff)
    {
        this.expBuff += expBuff;
    }

    public void ArmorModifier(float armor)
    {
        this.armor += armor;
    }
    #endregion

    public void HpRegen()
    {
        this.currentHp += this.hpRegen;
        if (this.currentHp > this.hp)
        {
            this.currentHp = this.hp;
        }
    }

    private int ExpBuff(int exp)
    {
        return (int)(exp * (this.expBuff + 1.0f));
    }

    public int Armor(int damage)
    {
        return damage - (int)(damage * this.armor * 0.01f);
    }

}
