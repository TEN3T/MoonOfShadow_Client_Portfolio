
using System.Collections.Generic;

public class FieldStructureData
{
    public int structureId { get; private set; }
    public string structureName { get; private set; }
    public string frontPath { get; private set; }
    public string topPath { get; private set; }
    public List<string> gimmickParam { get; private set; }
    public bool topIsPassable { get; private set; }
    public bool frontIsPassable { get; private set; }
    public float castTime { get; private set; }
    public int layerOrder { get; private set; }
    public float coolTime { get; private set; }
    public bool isAct { get; private set; }

    public void SetStructureId(int structureId) { this.structureId = structureId; }
    public void SetStructureName(string structureName) { this.structureName = structureName; }
    public void SetFrontPath(string frontPath) { this.frontPath = frontPath; }
    public void SetTopPath(string topPath) { this.topPath = topPath; }
    public void SetGimmickParam(List<string> gimmickParam) { this.gimmickParam = gimmickParam; }
    public void SetTopIsPassable(bool topIsPassable) { this.topIsPassable = topIsPassable; }
    public void SetFrontIsPassable(bool frontIsPassable) { this.frontIsPassable = frontIsPassable; }
    public void SetCastTime(float castTime) { this.castTime = castTime; }
    public void SetLayerOrder(int layerOrder) { this.layerOrder = layerOrder; }
    public void SetCoolTime(float coolTime) { this.coolTime = coolTime; }
    public void SetIsAct(bool isAct) { this.isAct = isAct; }
}
