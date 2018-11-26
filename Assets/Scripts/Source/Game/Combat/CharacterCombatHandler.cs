using System;

public class CharacterCombatHandler
{
    Action attackHandler, defendHandler, deathHandler;
    // (remainingHealth, maxHealth, damage)
    Action<int, int, int> damageHandler;

    public CharacterCombatHandler(){}

    public CharacterCombatHandler(Action onAttackHandler, 
                                  Action onDefendHandler, 
                                  Action<int, int, int> onDamageHandler,
                                  Action onDeathHandler)
    {
        this.attackHandler = onAttackHandler;
        this.defendHandler = onDefendHandler;
        this.damageHandler = onDamageHandler;
        this.deathHandler = onDeathHandler;
    }

    public void Subscribe(CharacterCombatHandler characterCombatHandler)
    {
        this.attackHandler += characterCombatHandler.attackHandler;
        this.defendHandler += characterCombatHandler.defendHandler;
        this.damageHandler += characterCombatHandler.damageHandler;
        this.deathHandler += characterCombatHandler.deathHandler;
    }

    public void OnAttack()
    {
        if(attackHandler != null)
        {
            attackHandler();
        }
    }

    public void OnDefend()
    {
        if(defendHandler != null)
        {
            defendHandler();
        }
    }

    public void OnDamage(int remainingHealth, int maxHealth, int damage)
    {
        if(damageHandler != null)
        {
            damageHandler(remainingHealth, maxHealth, damage);
        }
    }

    public void OnDeath()
    {
        if(deathHandler != null)
        {
            deathHandler();
        }
    }
}
