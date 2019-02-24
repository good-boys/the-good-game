
public class Weapon : CharacterItem
{
    public virtual int Damage
    {
        get;
        private set;
    }

    public virtual int Defense
    {
        get;
        private set;
    }

    public virtual int BonusAttack
    {
        get;
        private set;
    }

    public virtual int BonusDefense
    {
        get;
        private set;
    }

    public Weapon(string name, int damage, int defense, int bonusAttack, int bonusDefense) : base(name)
    {
        this.Damage = damage;
        this.Defense = defense;
        this.BonusAttack = bonusAttack;
        this.BonusDefense = bonusDefense;
    }
}
