using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santuary : FieldStructure
{
    private const string SANTUARY_CIRCLE_PATH = "Prefabs/InGame/FieldStructure/FieldStructure_SantuaryCircle";

    private bool isActive;
    private int diffCount;
    private int currentKillCount;

    private SantuaryCircle santuaryCircle;

    protected override void Awake()
    {
        base.Awake();

        isActive = false;
        diffCount = int.Parse(this.fieldStructureData.gimmickParam[0]);

        santuaryCircle = Instantiate(ResourcesManager.Load<SantuaryCircle>(SANTUARY_CIRCLE_PATH), transform);
        santuaryCircle.gameObject.SetActive(false);
    }

    private void Update()
    {
        //DebugManager.Instance.PrintWarning("KillCount: {0} / CurrentCount: {1} / Diffcount: {2}", GameManager.Instance.killcount, killCount, diffCount);
        //if (GameManager.Instance.killCount - diffCount * (processMaxCount - processCount) == 0 && !santuaryCircle.gameObject.activeInHierarchy)
        //{
        //    ++processCount;
        //    if (processCount > processMaxCount)
        //    {
        //        this.gameObject.SetActive(false);
        //    }
        //    santuaryCircle.gameObject.SetActive(true);
        //}

        if (!isActive)
        {
            StartCoroutine(Activation());
        }
    }

    private IEnumerator Activation()
    {
        isActive = true;
        WaitForFixedUpdate tick = new WaitForFixedUpdate();
        currentKillCount = GameManager.Instance.killCount;
        while (GameManager.Instance.killCount - currentKillCount < diffCount)
        {
            yield return tick;
        }

        santuaryCircle.gameObject.SetActive(true);
        while (santuaryCircle.gameObject.activeInHierarchy)
        {
            yield return tick;
        }

        isActive = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
