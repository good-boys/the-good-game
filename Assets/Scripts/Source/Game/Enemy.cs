public class Enemy : Character
{
    public virtual bool HasPattern { get { return pattern != null; } }

    CharacterActionPattern pattern;
    int patternIndex = 0;

    public Enemy(string name, int health, int speed) : base(name, health, speed)
    {

    }

    public Enemy(CharacterConfig config) : base(config) { }

    public void SetActionPattern(CharacterActionPattern pattern)
    {
        this.pattern = pattern;
    }

    public CharacterActionPattern GetActionPattern()
    {
        return pattern;
    }

    public virtual CharacterAction NextActionFromPattern(params Character[] targets)
    {
        if(pattern.Length == 0)
        {
            throw new NoCharacterActionExistsException("Action pattern is empty");
        }

        CharacterActionTemplate templateAction = pattern[patternIndex++];
        patternIndex %= pattern.Length;
        if(templateAction is DefendTemplate)
        {
            return Defend(targets);
        }
        else if(templateAction is AttackTemplate)
        {
            Attack attack = Attack(targets);
            attack.Direction = (templateAction as AttackTemplate).Direction;
            return attack;
        }
        throw new NoCharacterActionExistsException(string.Format(
            "Unsupported template of type {0}",
            templateAction.GetType()));
    }

    private Enemy copyActionPattern(Enemy copy)
    {
        if(HasPattern)
        {
            copy.SetActionPattern(UnityEngine.Object.Instantiate(pattern));
        }
        return copy;
    }

    public new object CopyConfig()
    {
        Enemy copy = new Enemy(Config);
        copy = copyActionPattern(copy);
        return copyWeapon(copy);
    }
}
