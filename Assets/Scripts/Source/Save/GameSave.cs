using System;

[Serializable]
public class GameSave
{
    public Random Random { get; private set; }
    public Player Player { get; private set; }
    public Tutorial[] Tutorials { get; private set; }

    readonly int seed;
    readonly Player originalPlayer;

    public GameSave(int seed, Player player, Tutorial[] tutorials)
    {
        Random = new Random(seed);
        Player = player;
        this.seed = seed;
        originalPlayer = player.CopyConfig() as Player;
        Tutorials = tutorials;
    }

    public void Reset()
    {
        Random = new Random(seed);
        Player = originalPlayer;
        foreach(Tutorial tut in Tutorials)
        {
            tut.Reset();
        }
    }
}
