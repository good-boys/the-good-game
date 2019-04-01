using System;

[Serializable]
public class Player : Character
{
    public Player(string name, int health, int speed) : base(name, health, speed) { }

    public Player(CharacterConfig config) : base(config) { }

    public new object CopyConfig()
    {
        return new Player(Config);
    }
}
