using System;

public class Attack : CharacterAction 
{
    public virtual int Damage
    {
        get;
        private set;
    }

    public virtual int Bonus
    {
        get;
        private set;
    }

    public Attack(Character actor, int damage, int bonus, Action onAttackHandler = null, params Character[] targets) : base(actor, onAttackHandler, targets)
    {
        this.Damage = damage;
        this.Bonus = bonus;
    }

    public override void AddBonus()
    {
        this.Damage += this.Bonus;
    }

    public override void ResetAction()
    {
        this.Damage -= this.Bonus;
    }
}
