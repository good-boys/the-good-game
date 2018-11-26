using UnityEngine;

public abstract class AbstractCombatConfig : MonoBehaviour
{
    public abstract Player GetPlayer();
    public abstract Enemy[] GetEnemies();
}
