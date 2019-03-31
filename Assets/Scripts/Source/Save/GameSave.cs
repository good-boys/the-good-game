using System;

[Serializable]
public class GameSave
{
    public Random Random { get; private set; }

    public GameSave(int seed)
    {
        Random = new Random(seed);
    }
}
