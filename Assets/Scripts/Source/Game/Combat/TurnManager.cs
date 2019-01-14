using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    Queue<CharacterAction> characterActions = new Queue<CharacterAction>();

    public virtual void RegisterAction(CharacterAction characterAction)
    {
        characterActions.Enqueue(characterAction);
    }

    public virtual CharacterAction GetNextAction()
    {
        return characterActions.Dequeue();
    }

    public virtual bool ShouldWaitForPlayerAction()
    {
        // TODO
        return characterActions.Count == 0;
    }
}
