public class CombatConfig : AbstractCombatConfig
{
    Player player;
    Enemy enemy;

    public void Initialize(Player savedPlayer, Enemy nextEnemy)
    {
        player = savedPlayer.CopyConfig() as Player;
        enemy = nextEnemy.CopyConfig() as Enemy;
    }

    public override Player GetPlayer()
    {
        return player;
    }

    public override Enemy GetEnemy()
    {
        return enemy;
    }
}
