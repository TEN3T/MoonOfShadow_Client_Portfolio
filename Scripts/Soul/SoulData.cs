using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulData
{
    public int soulId { get; private set; }
    public string soulName { get; private set; }
    public string soulExplain { get; private set; }
    public string image { get; private set; }
    //혼 효과 이넘 배열
    //혼 효과 파람 배열
    public string categorySoul { get; private set; }
    public int colGroup { get; private set; }
    public int orderInCol { get; private set; }
    //언락 조건 이넘 배열
    //언락 조건 파람 배열

    public void SetSoulId(int soulId) { this.soulId = soulId; }
    public void SetSoulName(string soulName) { this.soulName = soulName; }
    public void SetSoulExplain(string soulExplain) { this.soulExplain = soulExplain; }
    public void SetSoulImage(string image) { this.image = image; }
    public void SetCategorySoul(string categorySoul) { this.categorySoul = categorySoul; }
    public void SetColGroup(int colGroup) { this.colGroup = colGroup; }
    public void SetOrderInCol(int orderInCol) { this.orderInCol = orderInCol; }
}
