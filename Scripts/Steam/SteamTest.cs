using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!SteamManager.Initialized) { return;}
        string name = SteamFriends.GetPersonaName();

        SettingManager.Instance.ReadSettingFile();
        SettingManager.Instance.GetSettingValue("test");
        SettingManager.Instance.SetSettingValue("test",5);
        SettingManager.Instance.WriteSettingFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
