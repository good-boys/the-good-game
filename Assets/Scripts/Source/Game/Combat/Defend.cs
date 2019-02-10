using System;

public class Defend : CharacterAction 
{
    public virtual int Defense
    {
        get;
        private set;
    }

    public virtual int Bonus
    {
        get;
        private set;
    }

    public Defend(Character actor, int defense, int bonus, Action onDefendHandler = null, params Character[] targets) : base(actor, onDefendHandler, targets)
    {
        this.Defense = defense;
        this.Bonus = bonus;
    }

    public override void AddBonus()
    {
        this.Defense += this.Bonus;
    }

    public override void ResetAction()
    {
        this.Defense -= this.Bonus;
    }
}
