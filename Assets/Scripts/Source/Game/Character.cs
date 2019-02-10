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

    public float Speed
    {
        get
        {
            return _speed;
        }
        private set
        {
            _speed = value;
        }
    }

    public Weapon EquippedWeapon
    {
        get;
        private set;
    }

    public virtual CharacterAction ActiveAction
    {
        get;
        private set;
    }

    int _health;
    float _speed;

    public Character(string name, int health, float speed)
    {
        characterCombatHandler = new CharacterCombatHandler();
        this.Name = name;
        this.MaxHealth = health;
        this.Health = health;
        this.Speed = speed;
    }

    public Character(string name, int health, float speed, CharacterCombatHandler characterCombatHandler)
    {
        this.characterCombatHandler = characterCombatHandler;
        this.Name = name;
        this.MaxHealth = health;
        this.Health = health;
        this.Speed = speed;
    }

    public virtual void Damage(int damage)
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

    public virtual Attack Attack(params Character[] targets)
    {
        return new Attack(this, EquippedWeapon == null ? GetDefaultAttack() : EquippedWeapon.Damage, executeAttack, targets);
    }

    void executeAttack()
    {
        characterCombatHandler.OnAttack();
    }

    public virtual Defend Defend(params Character[] targets)
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

    public virtual CharacterAction UseActiveAction()
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
