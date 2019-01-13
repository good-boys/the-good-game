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

    public virtual void Subscribe(CharacterCombatHandler characterCombatHandler)
    {
        this.attackHandler += characterCombatHandler.attackHandler;
        this.defendHandler += characterCombatHandler.defendHandler;
        this.damageHandler += characterCombatHandler.damageHandler;
        this.deathHandler += characterCombatHandler.deathHandler;
    }

    public virtual void OnAttack()
    {
        if(attackHandler != null)
        {
            attackHandler();
        }
    }

    public virtual void OnDefend()
    {
        if(defendHandler != null)
        {
            defendHandler();
        }
    }

    public virtual void OnDamage(int remainingHealth, int maxHealth, int damage)
    {
        if(damageHandler != null)
        {
            damageHandler(remainingHealth, maxHealth, damage);
        }
    }

    public virtual void OnDeath()
    {
        if(deathHandler != null)
        {
            deathHandler();
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == this)
        {
            return true;
        }
        if (obj is CharacterCombatHandler)
        {
            CharacterCombatHandler characterCombatHandler = obj as CharacterCombatHandler;
            return Object.Equals(characterCombatHandler.attackHandler, attackHandler) &&
                            Object.Equals(characterCombatHandler.defendHandler, defendHandler) &&
                            Object.Equals(characterCombatHandler.damageHandler, damageHandler) &&
                            Object.Equals(characterCombatHandler.deathHandler, deathHandler);
        }
        return false;
    }
}
