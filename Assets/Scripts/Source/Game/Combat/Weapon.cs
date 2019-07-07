using System;

[Serializable]
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

    public virtual float GoalSize
    {
        get;
        private set;
    }

    public virtual float GoalPos
    {
        get;
        private set;
    }

    public virtual float TimerSpeed
    {
        get;
        private set;
    }

    public Weapon(string name, int damage, int defense, int bonusAttack, int bonusDefense, float goalSize, float goalPos, float timerSpeed) : base(name)
    {
        this.Damage = damage;
        this.Defense = defense;
        this.BonusAttack = bonusAttack;
        this.BonusDefense = bonusDefense;
        this.GoalSize = goalSize;
        this.GoalPos = goalPos;
        this.TimerSpeed = timerSpeed;
    }

    public Weapon Copy()
    {
        return new Weapon(
            Name, 
            Damage, 
            Defense, 
            BonusAttack, 
            BonusDefense, 
            GoalSize, 
            GoalPos, 
            TimerSpeed);
    }

    public override bool Equals(object obj)
    {
        if(!(obj is Weapon))
        {
            return ReferenceEquals(this, obj);
        }
        Weapon weapon = obj as Weapon;
        return Name.Equals(weapon.Name) &&
            Damage.Equals(weapon.Damage) &&
            Defense.Equals(weapon.Defense) &&
            BonusAttack.Equals(weapon.BonusAttack) &&
            BonusDefense.Equals(weapon.BonusDefense) &&
            GoalSize.Equals(weapon.GoalSize) &&
            GoalPos.Equals(weapon.GoalPos) &&
            TimerSpeed.Equals(weapon.TimerSpeed);
    }
}
