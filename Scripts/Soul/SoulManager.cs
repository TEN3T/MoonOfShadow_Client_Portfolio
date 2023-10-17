using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulManager : SingletonBehaviour<SoulManager>
{
    List<Soul> soulList = new List<Soul>();

    protected override void Awake()
    {
    }
}
