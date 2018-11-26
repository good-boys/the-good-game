using System;

public class CharacterAction
{
    Action onUseHandler;

    public Character Actor
    {
        get;
        private set;
    }
	
    public Character[] Targets
    {
        get;
        private set;
    }

    public CharacterAction(Character actor, Action onUseHandler = null, params Character[] targets)
    {
        this.Actor = actor;
        this.onUseHandler = onUseHandler;
        this.Targets = targets;
    }

    public void Use()
    {
        if(onUseHandler != null)
        {
            onUseHandler();
        }
    }
}
