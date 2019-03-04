using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour
{
    // TODO: Replace with permanent solution
    [SerializeField]
    float delayBetweenActions = 1f;

    [SerializeField]
    float attackTimer = 1f;

    [SerializeField]
    float defendTimer = 1f;

    [SerializeField]
    float inputStart = 0.5f;

    [SerializeField]
    float inputEnd = 1f;

    [SerializeField]
    float missTimer = 0.1f;

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
        if (!turnManager.ShouldWaitForPlayerAction(characterManager.GetActivePlayer()))
            return;

        Player player = characterManager.GetActivePlayer();
        player.hitBonus = false;
        turnManager.RegisterAction(player.Attack(characterManager.GetEnemies()));
        onPlayerActionReceived();
    }

    public void PlayerDefend()
    {
        if (!turnManager.ShouldWaitForPlayerAction(characterManager.GetActivePlayer()))
            return;

        Player player = characterManager.GetActivePlayer();
        player.hitBonus = false;
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
        if(turnManager.ShouldWaitForPlayerAction(characterManager.GetActivePlayer()))
        {
            return;
        }
        generateEnemyActions();
        StartCoroutine(executeActions());
    }

    void generateEnemyActions()
    {
        foreach(Enemy enemy in characterManager.GetEnemies())
        {
            turnManager.RegisterAction(characterManager.RequestNextAction(enemy));
        }
    }

    IEnumerator executeActions()
    {
        gameFlowManager.combatUI.anim.Play("HideSide");

        while (!turnManager.ShouldWaitForPlayerAction(characterManager.GetActivePlayer()))
        {
            float timer = 0.0f;
            float missDelay = 0.0f;
            bool missed = false;
            CharacterAction action = turnManager.Peek();
            CharacterAction enemyAction = action.Targets[0].ActiveAction; //TODO: Maybe add target enemy incase of multiple enemies
            
            if (action.Actor is Player)
            {
                if (action is Attack)
                {
                    //Debug.Log("Player turn attack");
                    while (timer < attackTimer && !action.Actor.hitBonus)
                    {
                        timer += Time.deltaTime;
                        if (missed)
                        {
                            missDelay += Time.deltaTime;

                            if (Input.GetButton("Fire1"))
                            {
                                missDelay = 0.0f;
                            }
                        }
                        else
                        {
                            if (timer > inputStart && timer < inputEnd)
                            {
                                if (Input.GetButton("Fire1"))
                                {
                                    action.Actor.hitBonus = true;
                                }
                            }
                            else
                            {
                                if (Input.GetButton("Fire1"))
                                {
                                    missed = true;
                                }
                            }
                        }

                        if (missDelay > missTimer)
                        {
                            missed = false;
                            missDelay = 0f;
                        }

                        yield return null;
                    }
                }
                else
                {
                    //Debug.Log("Player turn defend");
                }
            }
            else
            {
                if (action is Attack)
                {
                    //Debug.Log("Enemy turn attack");
                    if (enemyAction.Actor.ActiveAction is Defend)
                    {
                        action = enemyAction;
                        int attackDir = enemyAction.Actor.GetAttackDirection(); //TODO: Make better system for choosing enemy attack direction
                        gameFlowManager.combatUI.ShowEnemyDirection(attackDir);
                        //Debug.Log("Enemy turn player defended");
                        bool hitDirection = false;
                        while (timer < defendTimer && !hitDirection)
                        {
                            timer += Time.deltaTime;

                            Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

                            if (dir.magnitude > 0)
                            {
                                hitDirection = true;
                            }

                            if (timer > inputStart && timer < inputEnd)
                            {
                                if (dir.x > 0 && attackDir == 0)
                                {
                                    action.Actor.hitBonus = true;
                                }
                                else if (dir.x < 0 && attackDir == 1)
                                {
                                    action.Actor.hitBonus = true;
                                }
                                else if (dir.y > 0 && attackDir == 2)
                                {
                                    action.Actor.hitBonus = true;
                                }
                                else if (dir.y < 0 && attackDir == 3)
                                {
                                    action.Actor.hitBonus = true;
                                }
                            }

                            yield return null;
                        }
                    }
                }
                else
                {
                    //Debug.Log("Enemy turn defend");
                }
            }

            if (action.Actor.hitBonus)
            {
                action.AddBonus();
            }

            ProcessNextAction();
            yield return new WaitForSeconds(delayBetweenActions);
        }

        gameFlowManager.combatUI.anim.Play("ReturnSide");
    }
}
