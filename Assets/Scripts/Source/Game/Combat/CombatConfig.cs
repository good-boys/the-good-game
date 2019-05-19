using UnityEngine;
using System;

public class CombatConfig : AbstractCombatConfig
{
    Player player;
    Enemy enemy;

    public void Initialize(Player savedPlayer, Enemy nextEnemy)
    {
        player = savedPlayer;
        player.EquipWeapon(savedPlayer.EquippedWeapon);
        enemy = nextEnemy;
        enemy.EquipWeapon(nextEnemy.EquippedWeapon);
        enemy.SetActionPattern(nextEnemy.GetActionPattern());
    }

    public override Player GetPlayer()
    {
        return player;
    }

    public override Enemy GetEnemy()
    {
        return enemy;
    }
}
