using System;

[Serializable]
public class GameSave
{
    public Random Random { get; private set; }
    public Player Player { get; private set; }
    public int WeaponLevel { get; private set; }
    public int EnemyLevel { get; private set; }

    readonly int seed;
    readonly Player originalPlayer;

    public GameSave(int seed, Player player, int weaponLevel, int enemyLevel)
    {
        EnemyLevel = enemyLevel;
        WeaponLevel = weaponLevel;
        Random = new Random(seed);
        Player = player;
        this.seed = seed;
        originalPlayer = player.CopyConfig() as Player;
    }

    public void Reset()
    {
        Random = new Random(seed);
        Player = originalPlayer;
        WeaponLevel = 0;
        EnemyLevel = 0;
    }
}
