using UnityEngine;

using System;
using BFM;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    [SerializeField] private int mapId;
    [SerializeField] private int playerId;
    //private int mapId;
    //private int playerId;

    private bool gameOver = true;
    private float defaultScale;
    private float defaultCharScale;
    private UI_ESCPopup esc;

    public int killCount { get; set; }
    public PlayerUI playerUi { get; private set; }
    public Player player { get; private set; }
    public GameObject map { get; private set; }

    public int boxA { get; set; }
    public int boxB { get; set; }
    public int boxC { get; set; }
    public int key { get; set; }

    protected override void Awake()
    {
        defaultScale = float.Parse(Convert.ToString(CSVReader.Read("BattleConfig", "ImageMultiple", "ConfigValue")));
        defaultCharScale = float.Parse(Convert.ToString(CSVReader.Read("BattleConfig", "CharImageMultiple", "ConfigValue")));
        playerUi = GameObject.FindWithTag("PlayerUI").GetComponent<PlayerUI>();
        SoundManager.Instance.CreateSoundManager();
        LocalizeManager.Instance.SetLocalizeManager();
        //mapId = UIManager.Instance.selectStageID;
        //playerId = UIManager.Instance.selectCharacterID;
        //playerPoolManager.playerId = playerId;
    }

    private void Start()
    {
        Spawn();
        StartCoroutine(MonsterSpawner.Instance.Spawn());
        Timer.Instance.TimerSwitch(true);
        playerUi.NameBoxSetting(player.playerManager.playerData.iconImage);
        killCount = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            this.ExpUp(500);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ESC();
        }
        

        if (player.playerManager.playerData.currentHp <= 0 && gameOver)
        {
            gameOver = false;
            StopAllCoroutines();
            playerUi.GameOver();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("CutScenes", LoadSceneMode.Additive);
        }
    }

    public void ExpUp(int exp)
    {
        StartCoroutine(player.playerManager.playerData.ExpUp(exp));
    }

    public int GetPlayerId()
    {
        return playerId;
    }

    #region Player&MapCreate
    private void Spawn()
    {
        MapLoad(mapId);
        PlayerLoad(playerId);
        AssignLayerAndZ();
    }

    private void MapLoad(int mapId)
    {
        string mapName = CSVReader.Read("StageTable", mapId.ToString(), "MapID").ToString();
        GameObject map = ResourcesManager.Load<GameObject>("Prefabs/Map/" + mapName);
        this.map = Instantiate(map, transform);
        this.map.transform.localScale = Vector3.one * defaultScale;
        this.map.SetActive(true);
    }

    private void PlayerLoad(int playerId)
    {
        player = Instantiate(ResourcesManager.Load<Player>(CSVReader.Read("CharacterTable", playerId.ToString(), "CharacterPrefabPath").ToString()), transform);
        player.transform.localScale = Vector3.one * defaultCharScale;
        player.transform.localPosition = this.map.transform.Find("SpawnPoint").Find("PlayerPoint").localPosition;
        player.gameObject.SetActive(true);
    }

    private void AssignLayerAndZ()
    {
        RecursiveChild(player.transform, LayerConstant.SPAWNOBJECT);
        RecursiveChild(map.transform, LayerConstant.MAP);
    }

    private void RecursiveChild(Transform trans, LayerConstant layer)
    {
        if (trans.name.Equals("Character"))
        {
            trans.tag = "Player";
        }
        trans.gameObject.layer = (int)layer;
        if (trans.TryGetComponent(out Renderer render))
        {
            render.sortingLayerName = layer.ToString();
        }
        else if (trans.TryGetComponent(out MeshRenderer meshRender))
        {
            meshRender.sortingLayerName = layer.ToString();
        }
        trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, (int)layer);

        foreach (Transform child in trans)
        {
            switch (child.name)
            {
                case "Camera":
                    RecursiveChild(child, LayerConstant.POISONFOG);
                    break;
                case "FieldStructure":
                    //RecursiveChild(child, LayerConstant.OBSTACLE);
                    break;
                case "ItemCollider":
                    RecursiveChild(child, LayerConstant.ITEM);
                    break;
                //case "Top":
                //    RecursiveChild(child, LayerConstant.OBSTACLE - 2);
                //    break;
                case "PlayerManager":
                    RecursiveChild(child, LayerConstant.HIT);
                    break;
                default:
                    RecursiveChild(child, layer);
                    break;
            }
        }
    }
    #endregion

    #region Game State
    public void Pause()
    {
        if (Time.timeScale == 1f)
        {
            SoundManager.Instance.PauseType(AudioSourceSetter.EAudioType.EFFECT);
            SoundManager.Instance.PauseType(AudioSourceSetter.EAudioType.VOICE);
            Time.timeScale = 0f;
        }
        else
        {
            SoundManager.Instance.UnPauseType(AudioSourceSetter.EAudioType.EFFECT);
            SoundManager.Instance.UnPauseType(AudioSourceSetter.EAudioType.VOICE);
            Time.timeScale = 1f;
        }
    }

    private void ESC()
    {
        if (Time.timeScale == 1f)
        {
            SoundManager.Instance.PauseType(AudioSourceSetter.EAudioType.EFFECT);
            SoundManager.Instance.PauseType(AudioSourceSetter.EAudioType.VOICE);
            Time.timeScale = 0f;
            //esc = Instantiate(ResourcesManager.Load<UI_ESCPopup>("Prefabs/UI/UI_ESCPopup"));
        }
        else
        {
            SoundManager.Instance.UnPauseType(AudioSourceSetter.EAudioType.EFFECT);
            SoundManager.Instance.UnPauseType(AudioSourceSetter.EAudioType.VOICE);
            Time.timeScale = 1f;
            //Destroy(esc.gameObject);
        }
    }

    #endregion
}
