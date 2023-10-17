using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MonsterData
{
    
    [SerializeField] public string monsterName ;
    [SerializeField] public float sizeMultiple ;
    [SerializeField] public int hp ;
    [SerializeField] public int currentHp;
    [SerializeField] public float attack ;
    [SerializeField] public float moveSpeed ;
    [SerializeField] public float atkSpeed ;
    [SerializeField] public float viewDistance ;
    [SerializeField] public float atkDistance ;
    [SerializeField] public int skillID ;
    [SerializeField] public string groupSource ;
    [SerializeField] public int groupSourceRate ;
    [SerializeField] public string monsterPrefabPath ;
    [SerializeField] public AttackTypeConstant attackType ;

    public void SetMonsterName(string monsterName) { this.monsterName = monsterName; }
    public void SetSizeMultiple(float sizeMultiple) { this.sizeMultiple = sizeMultiple; }
    public void SetHp(int hp) { this.hp = hp; }
    public void SetCurrentHp(int currentHp) { this.currentHp = currentHp; }
    public void SetAttack(float attack) { this.attack = attack; }
    public void SetMoveSpeed(float moveSpeed) { this.moveSpeed = moveSpeed; }
    public void SetAtkSpeed(float atkSpeed) { this.atkSpeed = atkSpeed; }
    public void SetViewDistance(float viewDistance) { this.viewDistance = viewDistance; }
    public void SetAtkDistance(float atkDistance) { this.atkDistance = atkDistance; }
    public void SetSkillID(int skillID) { this.skillID = skillID; }
    public void SetGroupSource(string groupSource) { this.groupSource = groupSource; }
    public void SetGroupSourceRate(int groupSourceRate) { this.groupSourceRate = groupSourceRate; }
    public void SetMonsterPrefabPath(string monsterPrefabPath) { this.monsterPrefabPath = monsterPrefabPath; }
    public void SetAttackType(AttackTypeConstant attackType) { this.attackType = attackType; }
}