public class Enemy : Character 
{
    public bool HasPattern { get { return pattern != null; } }

    CharacterActionPattern pattern;
    int patternIndex = 0;

    public Enemy(string name, int health, int speed) : base(name, health, speed)
    {

    }

    public void SetActionPattern(CharacterActionPattern pattern)
    {
        this.pattern = pattern;
    }

    public CharacterAction NextActionFromPattern(params Character[] targets)
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
}
