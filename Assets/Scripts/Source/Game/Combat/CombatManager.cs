using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour
{
    // TODO: Replace with permanent solution
    [SerializeField]
    float delayBetweenActions = 1f;

    [SerializeField]
    float defendTimer = 1f;

    [SerializeField]
    float inputStart = 0.5f;

    [SerializeField]
    float inputEnd = 1f;

    [SerializeField]
    float missTimer = 0.1f;

    [SerializeField]
    PlayAttackVFX playerHit;

    [SerializeField]
    PlayAttackVFX enemyHit;

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
        if(!turnManager.HasNextAction())
        {
            Debug.LogWarning("Unable to process. TurnManager has no remaining actions");
            return;
        }
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
        if (gameFlowManager != null)
        {
            gameFlowManager.combatUI.anim.Play("HideSide");
        }

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
                    gameFlowManager.combatUI.ShowAttackTimer(action.Actor.EquippedWeapon.GoalSize, action.Actor.EquippedWeapon.GoalPos, action.Actor.EquippedWeapon.TimerSpeed);
                    float delta = action.Actor.EquippedWeapon.GoalSize * action.Actor.EquippedWeapon.TimerSpeed;
                    float minTimer = -action.Actor.EquippedWeapon.GoalPos / 360 * action.Actor.EquippedWeapon.TimerSpeed;
                    float maxTimer = minTimer + delta;
                    Debug.Log(minTimer + " " + maxTimer);
                    while (timer < action.Actor.EquippedWeapon.TimerSpeed && !action.Actor.hitBonus && !missed)
                    {
                        timer += Time.deltaTime;
                        Debug.Log(timer);

                        if (timer > minTimer && timer < maxTimer)
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
                            

                        yield return null;
                    }
                    gameFlowManager.combatUI.EndAttackTimer();
                    enemyHit.PlayVFX("small_0002"); //ToDo: Make this a variable passed by the player weapon type
                    enemyHit.PlaySFX("sword_whoosh_01");
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
                        AttackDirection attackDir = enemyAction.Actor.GetAttackDirection(); //TODO: Make better system for choosing enemy attack direction
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
                                if (dir.x > 0 && attackDir == AttackDirection.Right)
                                {
                                    action.Actor.hitBonus = true;
                                }
                                else if (dir.x < 0 && attackDir == AttackDirection.Left)
                                {
                                    action.Actor.hitBonus = true;
                                }
                                else if (dir.y > 0 && attackDir == AttackDirection.Up)
                                {
                                    action.Actor.hitBonus = true;
                                }
                                else if (dir.y < 0 && attackDir == AttackDirection.Down)
                                {
                                    action.Actor.hitBonus = true;
                                }
                            }

                            yield return null;
                        }
                        playerHit.PlayVFX("small_0002"); //ToDo: Make this a variable passed by the enemy
                        if (action.Actor.hitBonus)
                        {
                            playerHit.PlaySFX("sword_strike_armor_chain_04");
                        }
                        else
                        {
                            playerHit.PlaySFX("sword_whoosh_06");
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
