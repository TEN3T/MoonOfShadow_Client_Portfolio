using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategorySoulData
{
    public string categoryId { get; private set; }
    public int order { get; private set; }
    public string categoryName { get; private set; }
    //언락 조건 텍스트 아이디
    public string image { get; private set; }
    //언락 조건 이넘 배열
    //언락 조건 파람 배열
    public int soulCount { get; private set; }

    public void SetCategoryId(string categoryId) { this.categoryId = categoryId; }
    public void SetOrder(int order) { this.order = order; }
    public void SetCategoryName(string categoryName) { this.categoryName = categoryName; }
    public void SetImage(string image) { this.image = image; }
    public void SetSoulCount(int soulCount) { this.soulCount = soulCount; }
}
