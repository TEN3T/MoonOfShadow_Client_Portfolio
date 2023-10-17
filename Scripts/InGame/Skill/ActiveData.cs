using SKILLCONSTANT;
using System.Collections.Generic;

public class ActiveData
{
    public int skillId { get; private set; }                        //스킬 id값
    public float coolTime { get; private set; }                       //스킬의 재사용 대기시간
    public List<string> skillEffectParam { get; private set; }               //스킬 이펙트 파라미터
    public bool skillCut { get; private set; }                      //필사기 연출 값
    public bool isEffect { get; private set; }                      //게임 시작 시 스킬 발동 여부
    public bool isUltimate { get; private set; }                    //스킬의 타입이 궁극기인지 여부
    public string name { get; private set; }                        //스킬 이름
    public string desc { get; private set; }                        //스킬 설명
    public string icon { get; private set; }                        //스킬 아이콘 데이터 연결값
    public string cutDire { get; private set; }                     //필살기 컷씬 연출 값
    public string skillImage { get; private set; }                  //스킬 발동 시 사용할 애니메이션 파일 이름
    public List<SKILL_EFFECT> skillEffect { get; private set; }           //스킬 이펙트
    public SKILL_TARGET skillTarget { get; private set; }           //스킬 발동 대상
    public string skillPrefabPath { get; private set; }
    public float intervalTime { get; private set; }                 //투사체간 발사 간격
    public float duration { get; private set; }                     //스킬 지속 시간
    public bool isPenetrate { get; private set; }                   //스킬의 관통 여부

    //투사체 개수
    public int projectileCount
    {
        get
        {
            return GameManager.Instance.player != null ? GameManager.Instance.player.playerManager.playerData.projectileAdd + _projectileCount : _projectileCount;
        }
        private set { }
    }                
    private int _projectileCount;

    //투사체 크기 배율
    public float projectileSizeMulti
    {
        get
        {
            if (GameManager.Instance.player != null)
            {
                return _projectileSizeMulti + PassiveEffect.CalcData(_projectileSizeMulti, GameManager.Instance.player.playerManager.playerData.projectileSize.param, GameManager.Instance.player.playerManager.playerData.projectileSize.calcMode);
            }

            return _projectileSizeMulti;
        }
        private set { }
    }
    private float _projectileSizeMulti;

    //투사체 속도
    public float speed
    {
        get
        {
            if (GameManager.Instance.player != null)
            {
                return _speed + PassiveEffect.CalcData(_speed, GameManager.Instance.player.playerManager.playerData.projectileSpeed.param, GameManager.Instance.player.playerManager.playerData.projectileSpeed.calcMode);
            }

            return _speed;
        }
        private set { }
    }
    private float _speed;

    //스플레쉬 범위 (원의 반지름)
    public float splashRange
    {
        get
        {
            if (GameManager.Instance.player != null)
            {
                return _splashRange + PassiveEffect.CalcData(_splashRange, GameManager.Instance.player.playerManager.playerData.projectileSplash.param, GameManager.Instance.player.playerManager.playerData.projectileSplash.calcMode);
            }

            return _splashRange;
        }
        private set { }
    }
    private float _splashRange;

    //스킬 사거리
    public float attackDistance
    {
        get
        {
            if (GameManager.Instance.player != null)
            {
                return _attackDistance + PassiveEffect.CalcData(_attackDistance, GameManager.Instance.player.playerManager.playerData.projectileDistance.param, GameManager.Instance.player.playerManager.playerData.projectileDistance.calcMode);
            }

            return _attackDistance;
        }
        private set { }
    }
    private float _attackDistance;

    //스킬 공격력
    public float damage
    {
        get
        {
            if (GameManager.Instance.player != null)
            {
                return _damage + PassiveEffect.CalcData(_damage, GameManager.Instance.player.playerManager.playerData.skillDamage.param, GameManager.Instance.player.playerManager.playerData.skillDamage.calcMode);
            }

            return _damage;
        }
        private set { }
    }
    private float _damage;

    public void SetSkillId(int skillId) { this.skillId = skillId; }
    public void SetCoolTime(float coolTime) { this.coolTime = coolTime; }
    public void SetAttackDistance(float attackDistance) { this._attackDistance = attackDistance; }
    public void SetProjectileCount(int projectileCount) { this._projectileCount = projectileCount; }
    public void SetIntervalTime(float intervalTime) { this.intervalTime = intervalTime; }
    public void SetDuration(float duration) { this.duration = duration; }
    public void SetDamage(float damage) { this._damage = damage; }
    public void SetSpeed(float speed) { this._speed = speed; }
    public void SetSplashRange(float splashRange) { this._splashRange = splashRange; }
    public void SetProjectileSizeMulti(float projectileSizeMulti) { this._projectileSizeMulti = projectileSizeMulti; }
    public void SetSkillEffectParam(List<string> skillEffectParam) { this.skillEffectParam = skillEffectParam; }
    public void SetSkillCut(bool skillCut) { this.skillCut = skillCut; }
    public void SetIsEffect(bool isEffect) { this.isEffect = isEffect; }
    public void SetIsUltimate(bool isUltimate) { this.isUltimate = isUltimate; }
    public void SetIsPenetrate(bool isPenetrate) { this.isPenetrate = isPenetrate; }
    public void SetName(string name) { this.name = name; }
    public void SetDesc(string desc) { this.desc = desc; }
    public void SetIcon(string icon) { this.icon = icon; }
    public void SetCutDire(string cutDire) { this.cutDire = cutDire; }
    public void SetSkillImage(string skillImage) { this.skillImage = skillImage; }
    public void SetSkillEffect(List<SKILL_EFFECT> skillEffect) { this.skillEffect = skillEffect; }
    public void SetSkillTarget(SKILL_TARGET skillTarget) { this.skillTarget = skillTarget; }
    public void SetSkillPrefabPath(string skillPrefabPath) { this.skillPrefabPath = skillPrefabPath; }

    //public void ModifyCoolTime(float coolTime) { this.coolTime += coolTime; }
    //public void ModifyAttackDistance(float attackDistance) { this.attackDistance += attackDistance; }
    //public void ModifyProjectileCount(int projectileCount) { this.projectileCount += projectileCount; }
    //public void ModifyDuration(float duration) { this.duration += duration; }
    //public void ModifyDamage(float damage) { this.damage += damage; }
    //public void ModifySpeed(float speed) { this.speed += speed; }
    //public void ModifySplashRange(float splashRange) { this.splashRange += splashRange; }
    //public void ModifyProjectileSizeMulti(float projectileSizeMulti) { this.projectileSizeMulti += projectileSizeMulti; }
}
