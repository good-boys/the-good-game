using System;

[Serializable]
public class Player : Character
{
    public Player(string name, int health, int speed) : base(name, health, speed) { }

    public Player(CharacterConfig config) : base(config) { }

    public new object CopyConfig()
    {
        Player copy = new Player(Config);
        return copyWeapon(copy);
    }
}
