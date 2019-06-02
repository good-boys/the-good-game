using System;

[Serializable]
public class GameSave
{
    public Random Random { get; private set; }
    public Player Player { get; private set; }
    public Tutorial CombatTutorial { get; private set; }

    readonly int seed;
    readonly Player originalPlayer;

    public GameSave(int seed, Player player, Tutorial combatTutorial)
    {
        Random = new Random(seed);
        Player = player;
        this.seed = seed;
        originalPlayer = player.CopyConfig() as Player;
        CombatTutorial = combatTutorial;
    }

    public void Reset()
    {
        Random = new Random(seed);
        Player = originalPlayer;
        CombatTutorial.Reset();
    }
}
