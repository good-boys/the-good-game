using UnityEngine;

[CreateAssetMenu(fileName = "CharacterActionPattern", menuName = "Combat/Pattern", order = 1)]
public class CharacterActionPattern : ScriptableObject
{
    public CharacterActionTemplate[] Pattern { get { return actions; } }

    public virtual CharacterActionTemplate this[int index]
    {
        get { return actions[index]; }
    }

    public virtual int Length { get { return actions.Length; } }

    [SerializeField]
    CharacterActionTemplate[] actions;

    public CharacterActionPattern Init(CharacterActionTemplate[] actions)
    {
        this.actions = actions;
        return this;
    }
}
