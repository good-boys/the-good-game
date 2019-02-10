using UnityEngine;
using UnityEngine.UI;

public class PlaceholderCombatUI : AbstractCombatUI
{
    int actionIndex = 1;
    [SerializeField]
    Image playerHealth, enemyHealth;

    [SerializeField]
    Text actionBar;

    [SerializeField]
    Text infoBar;

    string playerName, enemyName;

    bool slain;

    public override CharacterCombatHandler GetPlayerCombatHandler(Player player)
    {
        playerName = player.Name;
        return new CharacterCombatHandler(
            ()=>{actionBar.text = string.Format("({2}) {0} is attacking with {1}", player.Name, player.EquippedWeapon.Name, actionIndex++);},
            ()=>{actionBar.text = string.Format("({2}) {0} is defending with {1}", player.Name, player.EquippedWeapon.Name, actionIndex++);},
            damagePlayer,
            killPlayer
        );
    }

    void damagePlayer(int remainingHealth, int maxHealth, int damage)
    {
        if(slain) return;
        infoBar.text = string.Format("{0} dealt {1} damage to {2}", enemyName, damage, playerName);
        damageCharacter(playerHealth, remainingHealth, maxHealth, damage);
    }

    void killPlayer() 
    {
        infoBar.text = string.Format("{0} has been defeated", playerName);
        playerHealth.fillAmount = 0;
        slain = true;
    }

    public override CharacterCombatHandler GetEnemyCombatHandler(Enemy enemy)
    {
        enemyName = enemy.Name;
        return new CharacterCombatHandler(
            ()=>{actionBar.text = string.Format("({2}) {0} is attacking with {1}", enemy.Name, enemy.EquippedWeapon.Name, actionIndex++);},
            ()=>{actionBar.text = string.Format("({2}) {0} is defending with {1}", enemy.Name, enemy.EquippedWeapon.Name, actionIndex++);},
            damageEnemy,
            killEnemy
        );
    }

    void damageEnemy(int remainingHealth, int maxHealth, int damage)
    {
        if(slain) return;
        infoBar.text = string.Format("{0} dealt {1} damage to {2}", playerName, damage, enemyName);
        damageCharacter(enemyHealth, remainingHealth, maxHealth, damage);
    }

    void killEnemy()
    {
        infoBar.text = string.Format("{0} is slain", enemyName);
        enemyHealth.fillAmount = 0;
        slain = true;
    }

    void damageCharacter(Image healthBar, int remainingHealth, int maxHealth, int dammage)
    {
        healthBar.fillAmount = (float) remainingHealth / (float) maxHealth;
    }
}
