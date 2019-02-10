using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    Queue<CharacterAction> characterActions = new Queue<CharacterAction>();

    // Used to test functionality
    public void Init(Queue<CharacterAction> characterActions)
    {
        this.characterActions = characterActions;
    }

    public virtual void RegisterAction(CharacterAction characterAction)
    {
        characterActions.Enqueue(characterAction);
    }

    public virtual CharacterAction GetNextAction()
    {
        return characterActions.Dequeue();
    }

    public virtual Queue<CharacterAction> GetQueue()
    {
        return characterActions;
    }

    public virtual void ReplaceQueue (Queue<CharacterAction> queue)
    {
        characterActions = queue;
    }

    public virtual CharacterAction Peek()
    {
        return characterActions.Peek();
    }

    public virtual bool ShouldWaitForPlayerAction()
    {
        // TODO
        return characterActions.Count == 0;
    }
}
