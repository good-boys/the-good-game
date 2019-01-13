using System;

public class Attack : CharacterAction 
{
    public virtual int Damage
    {
        get;
        private set;
    }

    public Attack(Character actor, int damage, Action onAttackHandler = null, params Character[] targets) : base(actor, onAttackHandler, targets)
    {
        this.Damage = damage;
    }
}
