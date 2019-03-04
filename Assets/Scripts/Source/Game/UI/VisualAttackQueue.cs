using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualAttackQueue : MonoBehaviour
{
    [SerializeField]
    Color playerColor = Color.green;

    [SerializeField]
    Color enemyColor = Color.red;

    [SerializeField]
    Image[] attackBlocks;

    [SerializeField]
    TurnManager turnManager;

    public virtual void Awake()
    {
        hideAllBlocks();
        if (turnManager)
        {
            turnManager.HandleActionsUpdated(UpdateDisplay);
        }
    }

    public void Init(Image[] attackBlocks,
                     TurnManager turnManager,
                     Color playerColor,
                     Color enemyColor)
    {
        this.attackBlocks = attackBlocks;
        this.turnManager = turnManager;
        turnManager.HandleActionsUpdated(UpdateDisplay);
        this.playerColor = playerColor;
        this.enemyColor = enemyColor;
    }

    public void UpdateDisplay()
    {
        List<CharacterAction> upcomingActions = turnManager.GetQueue().ToList();
        int blocksToFill = Mathf.Min(attackBlocks.Length, upcomingActions.Count);
        for(int i = 0; i < blocksToFill; i++)
        {
            attackBlocks[i].enabled = true;
            attackBlocks[i].color = getActionColor(upcomingActions[i]);
        }
        for(int j = blocksToFill; j < attackBlocks.Length; j++)
        {
            attackBlocks[j].enabled = false;
        }
    }

    Color getActionColor(CharacterAction action)
    {
        if(action.Actor is Player)
        {
             return playerColor;
        }
        else if(action.Actor is Enemy)
        {
             return enemyColor;
        }
        return Color.white;
    }

    void hideAllBlocks()
    {
        for (int i = 0; i < attackBlocks.Length; i++)
        {
            attackBlocks[i].enabled = false;
        }
    }
}
