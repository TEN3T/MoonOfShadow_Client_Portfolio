using BFM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class MonsterSpawner : SingletonBehaviour<MonsterSpawner>
{
    private const string WALL_NAME = "Wall";
    private const float RADIUS = 10.0f;
    private const int ROUND_AMOUNT = 50;

    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private int stageId = 10101;

    private int spawnAmount;
    private int spawnCount;
    private Dictionary<int, ObjectPool<Monster>> spawner;
    private Queue<RemainMonster> remainMonsters;
    //private WaitForSeconds tick = new WaitForSeconds(0.05f);
    private WaitForFixedUpdate tick = new WaitForFixedUpdate();
    private float monsterHpCoefficient;
    private float monsterAttackCoefficient;

    public List<Monster> monsters;
    
    private struct RemainMonster
    {
        public int id;
        public string location;

        public RemainMonster(int id, string location)
        {
            this.id = id;
            this.location = location;
        }
    }

    //private void Update()
    //{
    //    DebugManager.Instance.PrintWarning("[SpawnTest {0}s] MobCount: {1}/{2}", Timer.Instance.currentTime * 0.001, spawnCount, spawnAmount);
    //}

    protected override void Awake()
    {
        spawnAmount = Convert.ToInt32(CSVReader.Read("StageTable", stageId.ToString(), "LimitAmount"));
        spawnCount = 0;
        spawner = new Dictionary<int, ObjectPool<Monster>>();
        monsterHpCoefficient = float.Parse(CSVReader.Read("BattleConfig", "StatIncreaseValueHP", "ConfigValue").ToString());
        monsterAttackCoefficient = float.Parse(CSVReader.Read("BattleConfig", "StatIncreaseValueAttack", "ConfigValue").ToString());
    }

    public Monster SpawnMonster(int monsterId, Vector2 pos)
    {
        Monster monster = spawner[monsterId].GetObject();
        monster.monsterId = monsterId;
        monster.gameObject.layer = (int)LayerConstant.MONSTER;
        monster.GetComponentInChildren<MeshRenderer>().sortingLayerName = LayerConstant.SPAWNOBJECT.ToString();
        float weight = Timer.Instance.currentTime * 0.001f;
        monster.SpawnSet(monsterHpCoefficient * weight, monsterAttackCoefficient * weight);
        monster.transform.localScale = Vector3.one * monster.monsterData.sizeMultiple;
        monster.transform.localPosition = new Vector3(pos.x, pos.y, (int)LayerConstant.MONSTER);
        monster.SetTarget(GameManager.Instance.player.transform, true);
        monster.gameObject.SetActive(true);
        monsters.Add(monster);
        ++spawnCount;
        return monster;
    }

    public void DeSpawnMonster(Monster monster)
    {
        spawner[monster.monsterId].ReleaseObject(monster);
        monsters.Remove(monster);
        --spawnCount;
    }

    private Queue<MonsterSpawnData> spawnQueue;

    private void SpawnerInit()
    {
        monsters = new List<Monster>();
        remainMonsters = new Queue<RemainMonster>();
        spawnQueue = new Queue<MonsterSpawnData>();

        Dictionary<string, Dictionary<string, object>> stageData = CSVReader.Read(CSVReader.Read("StageTable", stageId.ToString(), "MonsterSpawnID").ToString());
        foreach (string spawnId in stageData.Keys)
        {
            try
            {
                int spawnMobId = Convert.ToInt32(stageData[spawnId]["SpawnMobID"]);
                if (!spawner.ContainsKey(spawnMobId))
                {
                    try
                    {
                        string prefabPath = CSVReader.Read("MonsterTable", spawnMobId.ToString(), "MonsterPrefabPath").ToString();
                        spawner.Add(spawnMobId, new ObjectPool<Monster>(ResourcesManager.Load<Monster>(prefabPath), transform));
                    }
                    catch
                    {
                        DebugManager.Instance.PrintDebug("[ERROR] 현재 존재하지 않는 몬스터입니다 MonsterID: " + spawnMobId);
                    }
                }
                spawnQueue.Enqueue(new MonsterSpawnData(Convert.ToInt32(spawnId), Convert.ToInt32(stageData[spawnId]["SpawnTime"]), spawnMobId, Convert.ToInt32(stageData[spawnId]["SpawnMobAmount"]), Convert.ToString(stageData[spawnId]["SpawnMobLocation"])));
            }
            catch
            {
                DebugManager.Instance.PrintDebug("[ERROR] 빈 줄이 삽입되어 있습니다: " + spawnId);
            }
        }
    }

    public IEnumerator Spawn()
    {
        WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();
        SpawnerInit();

        MonsterSpawnData spawnData = spawnQueue.Dequeue();
        while (true)
        {
            yield return fixedUpdate;

            if (remainMonsters.Count != 0)
            {
                RemainMonster remainMonster = remainMonsters.Dequeue();
                Monster monster;
                if (Enum.TryParse(remainMonster.location, true, out SpawnMobLocation spawnMobLocation))
                {
                    monster = SpawnMonster(remainMonster.id, CameraManager.Instance.RandomPosInGrid(spawnMobLocation));
                }
                else
                {
                    Vector2 spawnPos = GameManager.Instance.map.transform.Find("SpawnPoint").Find(remainMonster.location).position;
                    monster = SpawnMonster(remainMonster.id, spawnPos);
                }
                yield return tick;
            }

            if (Timer.Instance.currentTime < spawnData.spawnTime)
            {
                continue;   //아직 스폰타임이 아니면 스킵
            }
            if (spawnData.spawnTime != -1)
            {
                Monster monster;
                if (Enum.TryParse(spawnData.spawnLocation, true, out SpawnMobLocation spawnLocation))
                {
                    if (spawnLocation == SpawnMobLocation.ROUND)
                    {
                        StartCoroutine(Round(spawnData.spawnMobAmount, spawnData.spawnMobId));
                    }
                    else
                    {
                        for (int i = 0; i < spawnData.spawnMobAmount; i++)
                        {
                            SpawnMobLocation location = spawnLocation;
                            if (spawnLocation == SpawnMobLocation.RANDOMROUND)
                            {
                                location = (SpawnMobLocation)(i % (System.Enum.GetValues(typeof(SpawnMobLocation)).Length - 1));
                            }

                            if (location == SpawnMobLocation.FACE)
                            {
                                if (GameManager.Instance.player.lookDirection.x < 0)
                                {
                                    location = SpawnMobLocation.LEFT;
                                }
                                else
                                {
                                    location = SpawnMobLocation.RIGHT;
                                }
                            }
                            else if (location == SpawnMobLocation.BACK)
                            {
                                if (GameManager.Instance.player.lookDirection.x < 0)
                                {
                                    location = SpawnMobLocation.RIGHT;
                                }
                                else
                                {
                                    location = SpawnMobLocation.LEFT;
                                }
                            }
                            monster = SpawnMonster(spawnData.spawnMobId, CameraManager.Instance.RandomPosInGrid(location));
                            yield return tick;
                        }
                    }
                }
                else
                {
                    Vector2 spawnPos = GameManager.Instance.map.transform.Find("SpawnPoint").Find(spawnData.spawnLocation).position;
                    for (int i = 0; i < spawnData.spawnMobAmount; i++)
                    {
                        if (spawnCount < spawnAmount)
                        {
                            monster = SpawnMonster(spawnData.spawnMobId, spawnPos);
                        }
                        else
                        {
                            remainMonsters.Enqueue(new RemainMonster(spawnData.spawnMobId, spawnData.spawnLocation));
                        }
                        yield return tick;
                    }
                }

            }

            if (spawnQueue.Count == 0)
            {
                if (remainMonsters.Count == 0)
                {
                    DebugManager.Instance.PrintDebug("[SpawnerTest]: End");
                    yield break;  //더이상 스폰할 몬스터가 없을 경우 종료
                }
            }
            else
            {
                spawnData = spawnQueue.Dequeue();
            }
        }
        
    }

    private IEnumerator Round(int amount, int monsterId)
    {
        float weight = 10.0f;
        float angle = amount == 0 ? 0 : 360 / amount;
        float radius = RADIUS;

        if (amount > ROUND_AMOUNT)
        {
            radius = RADIUS * 1.25f;
        }
        Monster monster;
        for (int i = 0; i < amount; i++)
        {
            yield return tick;
            Vector2 pos = new Vector2(Mathf.Cos(i * angle * Mathf.Deg2Rad), Mathf.Sin(i * angle * Mathf.Deg2Rad)) * radius + (Vector2)GameManager.Instance.player.transform.position;
            try
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 1.5f, 1 << (int)LayerConstant.MAP);
                int j;
                for (j = 0; j < colliders.Length; j++)
                {
                    if (colliders[j].name.Equals(WALL_NAME))
                    {
                        break;
                    }
                }

                if (j == colliders.Length)
                {
                    if (spawnCount < spawnAmount)
                    {
                        monster = SpawnMonster(monsterId, pos);
                    }
                    else
                    {
                        remainMonsters.Enqueue(new RemainMonster(monsterId, "top"));
                    }
                }
                else
                {
                    --i;
                    angle += weight;
                }
            }
            catch
            {
                --i;
                angle += weight;
            }
        }
    }

}


