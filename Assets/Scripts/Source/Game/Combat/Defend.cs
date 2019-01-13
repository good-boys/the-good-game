using System;

public class Defend : CharacterAction 
{
    public int Defense
    {
        get;
        private set;
    }

    public Defend(Character actor, int defense, Action onDefendHandler = null, params Character[] targets) : base(actor, onDefendHandler, targets)
    {
        this.Defense = defense;
    }
}
