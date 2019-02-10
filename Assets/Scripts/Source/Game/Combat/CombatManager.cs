using UnityEngine;
using System.Collections;

public class CombatManager : MonoBehaviour
{
    // TODO: Replace with permanent solution
    [SerializeField]
    float delayBetweenActions = 1f;

    CharacterManager characterManager;
    TurnManager turnManager;
    GameFlowManager gameFlowManager;


    public virtual void Init(CharacterManager characterManager, TurnManager turnManager, GameFlowManager gameFlowManager)
    {
        this.characterManager = characterManager;
        this.turnManager = turnManager;
        this.gameFlowManager = gameFlowManager;
    }

    public void PlayerAttack()
    {
        Player player = characterManager.GetActivePlayer();
        turnManager.RegisterAction(player.Attack(characterManager.GetEnemies()));
        onPlayerActionReceived();
    }

    public void PlayerDefend()
    {
        Player player = characterManager.GetActivePlayer();
        turnManager.RegisterAction(player.Defend(characterManager.GetEnemies()));
        onPlayerActionReceived();
    }

    public void ProcessNextAction()
    {
        CharacterAction characterAction = turnManager.GetNextAction();
        characterManager.ProcessAction(characterAction);
        characterAction.Use();
    }

    void onPlayerActionReceived()
    {
        generateEnemyActions();
        StartCoroutine(executeActions());
    }

    void generateEnemyActions()
    {
        foreach (Enemy enemy in characterManager.GetEnemies())
        {
            turnManager.RegisterAction(characterManager.RequestNextAction(enemy));
        }
    }

    IEnumerator executeActions()
    {   
        while(!turnManager.ShouldWaitForPlayerAction(characterManager.GetActivePlayer()))
        {
            ProcessNextAction();
            yield return new WaitForSeconds(delayBetweenActions);
        }
    }
}
