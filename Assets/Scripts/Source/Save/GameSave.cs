using System;

[Serializable]
public class GameSave
{
    Random random;

    public GameSave(int seed)
    {
        random = new Random(seed);
    }
}
