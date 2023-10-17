using BFM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonBehaviour<ItemManager>
{
    private Dictionary<int, ObjectPool<Item>> itemPools;

    private Dictionary<string, Dictionary<string, object>> itemTable;
    private Dictionary<string, List<ItemGroup>> groupSource;
    private Dictionary<string, List<ItemPackage>> packageSource;

    protected override void Awake()
    {
        itemPools = new Dictionary<int, ObjectPool<Item>>();
        //ItemPoolInit();
        itemTable = CSVReader.Read("ItemTable");
        ItemSourceInit();
    }

    #region Init
    //private void ItemPoolInit()
    //{
    //    foreach (Transform child in transform)
    //    {
    //        itemPools.Add(int.Parse(child.name), child.GetComponent<ItemPool>());
    //    }
    //}

    private void ItemSourceInit()
    {
        groupSource = new Dictionary<string, List<ItemGroup>>();
        packageSource = new Dictionary<string, List<ItemPackage>>();

        Dictionary<string, Dictionary<string, object>> packageTable = CSVReader.Read("PackageTable");

        foreach (KeyValuePair<string, Dictionary<string, object>> data in packageTable)
        {
            if (data.Key.Equals(""))
            {
                continue;
            }
            ItemPackage package = new ItemPackage(Convert.ToInt32(data.Key), Convert.ToBoolean(data.Value["IsMultiple"].ToString().ToLower()), Convert.ToInt32(data.Value["ItemID"]), Convert.ToInt32(data.Value["ItemAmount"]), Convert.ToInt32(data.Value["ItemProbability"]));
            string packageSourceName = data.Value["PackageSource"].ToString();
            if (!packageSource.ContainsKey(packageSourceName))
            {
                packageSource[packageSourceName] = new List<ItemPackage>();
            }
            packageSource[packageSourceName].Add(package);
        }

        Dictionary<string, Dictionary<string, object>> groupTable = CSVReader.Read("GroupTable");
        foreach (KeyValuePair<string, Dictionary<string, object>> data in groupTable)
        {
            if (data.Key.Equals(""))
            {
                continue;
            }
            ItemGroup group = new ItemGroup(Convert.ToInt32(data.Key), Convert.ToBoolean(data.Value["IsMultiple"].ToString().ToLower()), data.Value["PackageID"].ToString(), Convert.ToInt32(data.Value["PackageProbability"]));
            string groupSourceName = data.Value["GroupSource"].ToString();
            if (!groupSource.ContainsKey(groupSourceName))
            {
                groupSource[groupSourceName] = new List<ItemGroup>();
            }
            groupSource[groupSourceName].Add(group);
        }
    }
    #endregion

    #region Item Spawn
    public void DropItems(string groupSourceName, Transform spawner)
    {
        List<int> items = SearchGroupSource(groupSourceName);
        
        foreach (int itemId in items)
        {
            SpawnItem(itemId, spawner.position);
        }
    }

    private List<int> SearchGroupSource(string groupSourceName)
    {
        List<int> items = new List<int>();

        int probability = 10000;
        foreach (ItemGroup group in groupSource[groupSourceName])
        {
            if (UnityEngine.Random.Range(0, probability + 1) <= group.packageProbability)
            {
                SearchPackageSource(group.packageId, items);

                if (!group.isMultiple)
                {
                    break;
                }
                
            }
            else
            {
                if (!group.isMultiple)
                {
                    probability -= group.packageProbability;
                }
            }
            
        }

        return items;
    }

    private void SearchPackageSource(string packageSourceName, List<int> items)
    {
        int probability = 10000;
        foreach (ItemPackage package in packageSource[packageSourceName])
        {
            if (UnityEngine.Random.Range(0, probability + 1) <= package.itemProbability)
            {
                items.Add(package.itemId);

                if (!package.isMultiple)
                {
                    break;
                }
            }
            else
            {
                if (!package.isMultiple)
                {
                    probability -= package.itemProbability;
                }
            }
        }
    }

    public Item SpawnItem(int itemId, Vector3 pos)
    {
        if (!itemPools.ContainsKey(itemId))
        {
            string prefabPath = itemTable[itemId.ToString()]["PrefabPath"].ToString();
            itemPools.Add(itemId, new ObjectPool<Item>(ResourcesManager.Load<Item>(prefabPath), transform));
        }
        Item item = itemPools[itemId].GetObject();
        item.gameObject.layer = (int)LayerConstant.ITEM;
        item.transform.position = pos;
        item.gameObject.SetActive(true);
        return item;
    }

    public void DeSpawnItem(Item item)
    {
        itemPools[item.itemId].ReleaseObject(item);
    }
    #endregion

    #region Struct
    private struct ItemGroup
    {
        public int groupId { get; private set; }
        public bool isMultiple { get; private set; }
        public string packageId { get; private set; }
        public int packageProbability { get; private set; }

        public ItemGroup(int groupId, bool isMultiple, string packageId, int packageProbability)
        {
            this.groupId = groupId;
            this.isMultiple = isMultiple;
            this.packageId = packageId;
            this.packageProbability = packageProbability;
        }
    }

    private struct ItemPackage
    {
        public int packageId { get; private set; }
        public bool isMultiple { get; private set; }
        public int itemId { get; private set; }
        public int itemAmount { get; private set; }
        public int itemProbability { get; private set; }

        public ItemPackage(int packageId, bool isMultiple, int itemId, int itemAmount, int itemProbability)
        {
            this.packageId = packageId;
            this.isMultiple = isMultiple;
            this.itemId = itemId;
            this.itemAmount = itemAmount;
            this.itemProbability = itemProbability;
        }
    }
    #endregion
}
