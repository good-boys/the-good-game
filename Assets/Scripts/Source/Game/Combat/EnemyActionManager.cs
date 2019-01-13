using UnityEngine;

public class EnemyActionManager : MonoBehaviour
{
    public virtual CharacterAction RequestNextAction(Enemy enemy, Player opponent)
    {
        // TODO: implement actual AI logic
        return Random.Range(0f, 1f) > 0.5f ? enemy.Attack(opponent) : (CharacterAction) enemy.Defend(opponent);
    }
}
