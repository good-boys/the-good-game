using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using UnityEngine.EventSystems;

public class CombatUI : AbstractCombatUI
{
    private const string FADE_IN_CLIP = "FadeIn";
    private const string FADE_OUT_CLIP = "FadeOut";
    private const float FADE_TRANSITION_DELAY = 0.5f;

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

    public GameObject winPanel;
    public GameObject winBtn;

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

    public void OnHealthChange(int health, int maxHealth)
    {
        damageCharacter(playerHealth, health, maxHealth, 0);
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
        GameFlowManager.instance.inBattle = true;
        GameFlowManager.instance.GameOver();
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

    public float DoFadeOut()
    {
        canvasGroup.alpha = 1;
        anim.Play(FADE_OUT_CLIP);
        return anim.runtimeAnimatorController.animationClips
                    .ToList()
                    .Find(clip => clip.name == FADE_OUT_CLIP).length;
    }

    public float GetFadeTransitionDelay()
    {
        return FADE_TRANSITION_DELAY;
    }

    public void DoFadeIn()
    {
        anim.Play(FADE_IN_CLIP);
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
        GameFlowManager.instance.inBattle = false;
        StartCoroutine(WinScreen());
    }

    IEnumerator WinScreen()
    {
        yield return new WaitForSeconds(1f);
        winPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(winBtn);
        anim.Play("WinPanel");
    }

    public void Rest()
    {
        GameFlowManager.instance.NextLevel();
    }

    public void Upgrade()
    {
        GameFlowManager.instance.NextLevel();
    }

    void damageCharacter(Image healthBar, int remainingHealth, int maxHealth, int dammage)
    {
        healthBar.fillAmount = (float) remainingHealth / (float) maxHealth;
    }
}
