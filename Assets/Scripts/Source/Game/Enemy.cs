public class Enemy : Character 
{
    public Enemy(string name, int health, int speed) : base(name, health, speed) { }

    public Enemy(CharacterConfig config) : base(config) { }
}
