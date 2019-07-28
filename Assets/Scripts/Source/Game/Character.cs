using System;

[Serializable]
public class Character
{
    [NonSerialized]
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

    public virtual int Speed
    {
        get
        {
            return _speed;
        }
        private set
        {
            if(value < 0)
            {
                _speed = 0;
                return;
            }
            _speed = value;
        }
    }

    public CharacterConfig Config
    {
        get { return new CharacterConfig(Name, MaxHealth, Speed); }
    }

    public Weapon EquippedWeapon
    {
        get;
        private set;
    }

    public virtual CharacterAction ActiveAction 
    {
        get { return _activeAction; }
        private set { _activeAction = value; }
    }

    public bool hitBonus;

    int _health;
    int _speed;
    [NonSerialized]
    CharacterAction _activeAction;
         
    public Character(CharacterConfig config) : this(config.Name, config.Health, config.Speed) {}

    public Character(string name, int health, int speed)
    {
        characterCombatHandler = new CharacterCombatHandler();
        this.Name = name;
        this.MaxHealth = health;
        this.Health = health;
        this.Speed = speed;
    }

    public Character(string name, int health, int speed, CharacterCombatHandler characterCombatHandler)
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
        return new Attack(this, 
                          EquippedWeapon == null ? GetDefaultAttack() : EquippedWeapon.Damage,
                          EquippedWeapon == null ? 0 : EquippedWeapon.BonusAttack,
                          executeAttack, 
                          targets);
    }

    void executeAttack()
    {
        characterCombatHandler.OnAttack();
    }

    public virtual Defend Defend(params Character[] targets)
    {
        return new Defend(this, 
                          EquippedWeapon == null ? GetDefaultDefense() : EquippedWeapon.Defense,
                          EquippedWeapon == null ? 0 : EquippedWeapon.BonusDefense, 
                          executeDefense, 
                          targets);
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

    public AttackDirection GetAttackDirection()
    {
        Random random = new Random();
        int attackDir = random.Next(0, 4);

        return (AttackDirection) attackDir;
    }

    public override string ToString()
    {
        return string.Format("([{0}] name:{1}, health:{2}, speed:{3})", GetType(), Name, Health, Speed);
    }

    public object CopyConfig()
    {
        Character copy = new Character(Config);
        return copyWeapon(copy);
    }

    protected Character copyWeapon(Character copy)
    {
        if(EquippedWeapon != null)
        {
            copy.EquipWeapon(EquippedWeapon.Copy());
        }
        return copy;
    }
}
