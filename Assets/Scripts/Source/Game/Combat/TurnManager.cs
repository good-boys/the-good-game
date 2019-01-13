using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour 
{
    Queue<CharacterAction> characterActions = new Queue<CharacterAction>();

    public void RegisterAction(CharacterAction characterAction)
    {
        characterActions.Enqueue(characterAction);
    }

    public CharacterAction GetNextAction()
    {
        return characterActions.Dequeue();
    }

    public bool ShouldWaitForPlayerAction()
    {
        // TODO
        return characterActions.Count == 0;
    }
}
