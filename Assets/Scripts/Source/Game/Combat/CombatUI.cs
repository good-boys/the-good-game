using UnityEngine;
using UnityEngine.UI;

public class CombatUI : AbstractCombatUI
{
    int actionIndex = 1;
    [SerializeField]
    Image playerHealth, enemyHealth;

    [SerializeField]
    Text actionBar;

    [SerializeField]
    Text infoBar;

    [SerializeField]
    Text damageNumber;

    [SerializeField]
    Text enemyDamageNumber;

    [SerializeField]
    Image upArrow;

    [SerializeField]
    Image leftArrow;

    [SerializeField]
    Image rightArrow;

    [SerializeField]
    Image downArrow;

    [SerializeField]
    RadialMarker radialMarker;

    public Animator anim;
    public CanvasGroup canvasGroup;

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

    public void Reset()
    {
        slain = false;
        enemyHealth.fillAmount = 100f;
    }

    void damagePlayer(int remainingHealth, int maxHealth, int damage)
    {
        damageNumber.text = damage.ToString();
        damageNumber.GetComponent<Animator>().SetTrigger("Hit");
        HideDirection();
        if (slain) return;
        infoBar.text = string.Format("{0} dealt {1} damage to {2}", enemyName, damage, playerName);
        damageCharacter(playerHealth, remainingHealth, maxHealth, damage);
    }

    void killPlayer()
    {
        infoBar.text = string.Format("{0} has been defeated", playerName);
        playerHealth.fillAmount = 0;
        slain = true;
        GameFlowManager.instance.RestartLevel();
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

    public void ShowAttackTimer(float goalSize, float goalPos, float speed)
    {
        radialMarker.Spin(goalSize, goalPos, speed);
    }

    public void EndAttackTimer()
    {
        radialMarker.EndTimer();
    }

    public void ShowEnemyDirection(AttackDirection dir)
    {
        switch(dir)
        {
            case AttackDirection.Right:
                rightArrow.enabled = true;
                break;
            case AttackDirection.Left:
                leftArrow.enabled = true;
                break;
            case AttackDirection.Up:
                upArrow.enabled = true;
                break;
            case AttackDirection.Down:
                downArrow.enabled = true;
                break;
        }
    }

    public void DoFadeOut()
    {
        canvasGroup.alpha = 1;
        anim.Play("FadeOut");
    }

    public void DoFadeIn()
    {
        anim.Play("FadeIn");
    }

    void HideDirection()
    {
        rightArrow.enabled = false;
        leftArrow.enabled = false;
        upArrow.enabled = false;
        downArrow.enabled = false;
    }

    void damageEnemy(int remainingHealth, int maxHealth, int damage)
    {
        enemyDamageNumber.text = damage.ToString();
        enemyDamageNumber.GetComponent<Animator>().Play("Hit");
        if (slain) return;
        infoBar.text = string.Format("{0} dealt {1} damage to {2}", playerName, damage, enemyName);
        damageCharacter(enemyHealth, remainingHealth, maxHealth, damage);
    }

    void killEnemy()
    {
        infoBar.text = string.Format("{0} is slain", enemyName);
        enemyHealth.fillAmount = 0;
        slain = true;
        GameFlowManager.instance.NextLevel();
    }

    void damageCharacter(Image healthBar, int remainingHealth, int maxHealth, int dammage)
    {
        healthBar.fillAmount = (float) remainingHealth / (float) maxHealth;
    }
}
