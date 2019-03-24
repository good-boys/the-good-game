using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    Action onActionQueueUpdated = delegate { };
    List<Queue<CharacterAction>> characterActionsByRound = new List<Queue<CharacterAction>>();
    List<Dictionary<Character, int>> turnCountByRound = new List<Dictionary<Character, int>>();

    // Used to test functionality
    public void Init(List<Queue<CharacterAction>> characterActions,
                     List<Dictionary<Character, int>> turnsInRound)
    {
        this.characterActionsByRound = characterActions;
        this.turnCountByRound = turnsInRound;
        addEmptyRound();
    }

    public virtual void Awake()
    {
        addEmptyRound();
    }

    void addEmptyRound()
    {
        characterActionsByRound.Add(new Queue<CharacterAction>());
        turnCountByRound.Add(new Dictionary<Character, int>());
    }

    public virtual void RegisterAction(CharacterAction characterAction)
    {
        Character actor = characterAction.Actor;
        int turnIndex = getFirstRoundForAction(actor);
        characterActionsByRound[turnIndex].Enqueue(characterAction);
        turnCountByRound[turnIndex][actor]++;
        onActionQueueUpdated();
    }

    public virtual void HandleActionsUpdated(Action action)
    {
        onActionQueueUpdated += action;
    }

    int getFirstRoundForAction(Character character)
    {
        if(character.Speed == 0)
        {
            throw new UnableToRegisterActionException("getFirstRoundForAction: Character has speed 0");
        }

        int roundIndex = -1;
        int characterTurnsInRound;
        do
        {
            roundIndex++;
            if(roundIndex >= turnCountByRound.Count)
            {
                addEmptyRound();
            }
            Dictionary<Character, int> turnsInRound = turnCountByRound[roundIndex];
            if(!turnsInRound.ContainsKey(character))
            {
                turnsInRound[character] = 0;
            }
            characterTurnsInRound = turnsInRound[character];
        }while(characterTurnsInRound >= character.Speed);
        return roundIndex;
    }

    public virtual bool HasNextAction()
    {
        return characterActionsByRound.Count > 0 &&
                characterActionsByRound.First().Count > 0;
    }

    public virtual CharacterAction GetNextAction()
    {
        cleanupQueue();
        if(characterActionsByRound.Count == 0)
        {
            throw new NoCharacterActionExistsException("GetNextAction: no actions remaining in queue");
        }
        CharacterAction nextAction = characterActionsByRound.First().Dequeue();
        cleanupQueue();
        onActionQueueUpdated();
        return nextAction;
    }

    void cleanupQueue()
    {
        while(characterActionsByRound.Count > 0 && characterActionsByRound.First().Count == 0)
        {
            characterActionsByRound.RemoveAt(0);
            turnCountByRound.RemoveAt(0);
        }
    }

    public virtual Queue<CharacterAction> GetQueue()
    {
        if(characterActionsByRound.Count == 0)
        {
            return new Queue<CharacterAction>();
        }
        return new Queue<CharacterAction>(characterActionsByRound.SelectMany(action => action));
    }

    public virtual List<CharacterAction> PredictActions(int count)
    {
        List<CharacterAction> futureActions = GetQueue().ToList();
        if(count <= futureActions.Count)
        {
            return futureActions.GetRange(0, count);
        }
        Character dummyPlayer = new Player("DummyPlayer", 1, 1);
        Character dummyEnemy = new Enemy("DummyEnemy", 1, 1);
        if(futureActions.Count == 0)
        {
            futureActions.Add(new CharacterAction(dummyPlayer));
            futureActions.Add(new CharacterAction(dummyEnemy));
        }
        else if(futureActions.Count == 1)
        {
            futureActions.Add(new CharacterAction(futureActions[0].Actor is Player ? dummyEnemy : dummyPlayer));
        }
        List<CharacterAction> prediction = new List<CharacterAction>();
        while(count > 0)
        {
            int sampleSize = Math.Min(futureActions.Count, count);
            prediction.AddRange(futureActions.GetRange(0, sampleSize));
            count -= sampleSize;
        }
        return prediction;
    }

    public virtual CharacterAction Peek()
    {
        return characterActionsByRound.First().Peek();
    }

    public virtual bool ShouldWaitForPlayerAction(Player player)
    {
        if(player.Speed == 0)
        {
            return false;
        }
        if(characterActionsByRound.Count == 0)
        {
            return true;
        }

        Queue<CharacterAction> roundActions = characterActionsByRound.Last();
        Dictionary<Character, int> countsInRound = turnCountByRound.Last();
        if(roundActions.Count == 0)
        {
            return true;
        }
        // CASE: Player hasn't taken a turn yet
        if(!countsInRound.ContainsKey(player))
        {
            return true;
        }
        // CASE: Player has used all their turns for the round
        if(countsInRound[player] >= player.Speed)
        {
            return false;
        }
        return true;
    }
}

public class NoCharacterActionExistsException : Exception
{
    public NoCharacterActionExistsException()
    {
    }

    public NoCharacterActionExistsException(string message)
        : base(message)
    {
    }

    public NoCharacterActionExistsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

public class UnableToRegisterActionException : Exception
{
    public UnableToRegisterActionException()
    {
    }

    public UnableToRegisterActionException(string message)
        : base(message)
    {
    }

    public UnableToRegisterActionException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
