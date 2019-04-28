using UnityEngine;

public class CharacterActionTemplate : ScriptableObject
{

}

[CreateAssetMenu(fileName = "Attack", menuName = "Combat/Attack", order = 1)]
public class AttackTemplate : CharacterActionTemplate
{
    public virtual AttackDirection Direction { get { return direction; } }

    [SerializeField]
    AttackDirection direction;
}

[CreateAssetMenu(fileName = "Defend", menuName = "Combat/Defend", order = 1)]
public class DefendTemplate : CharacterActionTemplate
{

}
