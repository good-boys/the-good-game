using UnityEngine;

public class EnemyActionManager : MonoBehaviour
{
    RandomGenerator random;

    public virtual void Init(RandomGenerator random)
    {
        this.random = random;
    }

    public virtual CharacterAction RequestNextAction(Enemy enemy, Player opponent)
    {
        // TODO: implement actual AI logic
        return random.Generate(0f, 1f) > 0.5f ? enemy.Attack(opponent) : (CharacterAction) enemy.Defend(opponent);
    }
}
