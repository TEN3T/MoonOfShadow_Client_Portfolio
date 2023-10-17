

public class MonsterSpawnData
{
    public MonsterSpawnData(int spawnId, int spawnTime, int spawnMobId, int spawnMobAmount, string spawnLocation)
    {
        this.spawnId = spawnId;
        this.spawnTime = spawnTime;
        this.spawnMobId = spawnMobId;
        this.spawnMobAmount = spawnMobAmount;
        this.spawnLocation = spawnLocation;
    }

    public int spawnId { get; private set; }
    public int spawnTime { get; private set; }
    public int spawnMobId { get; private set; }
    public int spawnMobAmount { get; private set; }
    public string spawnLocation { get; private set; } //string, enum 둘다 가져서 TryParse 꼭 사용하기

    //public void SetSponeTime(int spawnTime) { this.spawnTime = spawnTime; }
    //public void SetSpawnId(int spawnId) { this.spawnId = spawnId; }
    //public void SetSpawnMobID(int spawnMobId) { this.spawnMobId = spawnMobId; }
    //public void SetSpawnMobAmount(int spawnMobAmount) { this.spawnMobAmount = spawnMobAmount; }
    //public void SetSpawnLocation(string spawnLocation) { this.spawnLocation = spawnLocation; }
}
