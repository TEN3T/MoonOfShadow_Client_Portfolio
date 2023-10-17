
public class ItemData
{
    public int itemId { get; private set; }
    public string itemName { get; private set; }
    public string itemImage { get; private set; }
    public ItemConstant itemType { get; private set; }
    public int itemTypeParam { get; private set; }
    public string imagePath { get; private set; }

    public void SetItemId(int itemId) { this.itemId = itemId; }
    public void SetItemName(string itemName) { this.itemName = itemName; }
    public void SetItemImage(string itemImage) { this.itemImage = itemImage; }
    public void SetItemType(ItemConstant itemType) { this.itemType = itemType; }
    public void SetItemTypeParam(int itemTypeParam) { this.itemTypeParam = itemTypeParam; }
    public void SetImagePath(string imagePath) { this.imagePath = imagePath; }
}
