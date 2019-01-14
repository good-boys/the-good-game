using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour 
{
    StatManager statManager;
    EnemyActionManager enemyActionManager;
    List<Player> players;
    List<Enemy> enemies;

    public virtual void Init(StatManager statManager, EnemyActionManager enemyActionManager)
    {
        this.statManager = statManager;
        this.enemyActionManager = enemyActionManager;
        players = new List<Player>();
        enemies = new List<Enemy>();
    }

    public virtual CharacterAction RequestNextAction(Character character)
    {
        if(players.Count == 0)
        {
            Debug.LogError("Cannot determine next action because there is no player");
            return null;
        }
        if(character is Enemy)
        {
            return enemyActionManager.RequestNextAction(character as Enemy, GetActivePlayer());
        }
        // TODO
        throw new NotImplementedException();
    }

    public virtual void RegisterCharacter(Character character)
    {
        if (character is Player)
        {
            players.Add(character as Player);
        }
        else if(character is Enemy)
        {
            enemies.Add(character as Enemy);
        }
    }

    public virtual void ProcessAction(CharacterAction action)
    {
        statManager.ProcessAction(action);
        if(action is Defend)
        {
            action.Actor.SetActiveAction(action);
        }
    }

    public virtual Player GetActivePlayer()
    {
        // TODO
        return players[0];
    }

    public virtual Enemy[] GetEnemies()
    {
        return enemies.ToArray();
    }
}
