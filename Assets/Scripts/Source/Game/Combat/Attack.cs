using System;

public enum AttackDirection
{
    Right,
    Left,
    Up,
    Down,
    None,
}

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

    public AttackDirection Direction { get; set; }

    public Attack(Character actor, int damage, int bonus, Action onAttackHandler = null, params Character[] targets) : base(actor, onAttackHandler, targets)
    {
        this.Damage = damage;
        this.Bonus = bonus;
        this.Direction = AttackDirection.None;
    }

    public override void DisableAttack()
    {
        this.Damage = 0;
        this.Bonus = 0;
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
