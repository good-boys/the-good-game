using System;

public class Character
{
    CharacterCombatHandler characterCombatHandler;

    public string Name
    {
        get;
        private set;
    }

    public int MaxHealth
    {
        get;
        private set;
    }

    public int Health
    {
        get
        {
            return _health;
        }
        private set
        {
            _health = Math.Max(value, 0);
            if(_health == 0)
            {
                characterCombatHandler.OnDeath();
            }
        }
    }

    public Weapon EquippedWeapon
    {
        get;
        private set;
    }

    public CharacterAction ActiveAction
    {
        get;
        private set;
    }

    int _health;

    public Character(string name, int health)
    {
        characterCombatHandler = new CharacterCombatHandler();
        this.Name = name;
        this.MaxHealth = health;
        this.Health = health;
    }

    public Character(string name, int health, CharacterCombatHandler characterCombatHandler)
    {
        this.characterCombatHandler = characterCombatHandler;
        this.Name = name;
        this.MaxHealth = health;
        this.Health = health;
    }

    public void Damage(int damage)
    {
        this.Health -= damage;
        characterCombatHandler.OnDamage(Health, MaxHealth, damage);
    }

    public virtual void SubscribeCombatHandler(CharacterCombatHandler characterCombatHandler)
    {
        this.characterCombatHandler.Subscribe(characterCombatHandler);
    }

    public void EquipWeapon(Weapon weapon)
    {
        EquippedWeapon = weapon;
    }

    public Attack Attack(params Character[] targets)
    {
        return new Attack(this, EquippedWeapon == null ? GetDefaultAttack() : EquippedWeapon.Damage, executeAttack, targets);
    }

    void executeAttack()
    {
        characterCombatHandler.OnAttack();
    }

    public Defend Defend(params Character[] targets)
    {
        return new Defend(this, EquippedWeapon == null ? GetDefaultDefense() : EquippedWeapon.Defense, executeDefense, targets);
    }

    void executeDefense()
    {
        characterCombatHandler.OnDefend();
    }

    public virtual void SetActiveAction(CharacterAction action)
    {
        ActiveAction = action;
    }

    public CharacterAction UseActiveAction()
    {
        CharacterAction activeAction = ActiveAction;
        ActiveAction = null;
        return activeAction;
    }

    protected virtual int GetDefaultAttack()
    {
        return 0;
    }

    protected virtual int GetDefaultDefense()
    {
        return 0;
    }
}
