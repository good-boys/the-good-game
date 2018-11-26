using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {
    StatManager statManager;
    EnemyActionManager enemyActionManager;
    List<Player> players;
    List<Enemy> enemies;

    public void Init(StatManager statManager, EnemyActionManager enemyActionManager)
    {
        this.statManager = statManager;
        this.enemyActionManager = enemyActionManager;
        players = new List<Player>();
        enemies = new List<Enemy>();
    }

    public CharacterAction RequestNextAction(Character character)
    {
        if(character is Enemy)
        {
            return enemyActionManager.RequestNextAction(character as Enemy, GetActivePlayer());
        }
        // TODO
        throw new NotImplementedException();
    }

    public void RegisterCharacter(Character character)
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

    public void ProcessAction(CharacterAction action)
    {
        statManager.ProcessAction(action);
        if(action is Defend)
        {
            action.Actor.SetActiveAction(action);
        }
    }

    public Player GetActivePlayer()
    {
        // TODO
        return players[0];
    }

    public Enemy[] GetEnemies()
    {
        return enemies.ToArray();
    }
}
