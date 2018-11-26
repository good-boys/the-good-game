
public class Weapon : CharacterItem
{
    public int Damage
    {
        get;
        private set;
    }

    public int Defense
    {
        get;
        private set;
    }

    public Weapon(string name, int damage, int defense) : base(name)
    {
        this.Damage = damage;
        this.Defense = defense;
    }
}
