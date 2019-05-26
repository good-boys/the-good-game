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
}
