using UnityEngine;

public abstract class AbstractCombatUI : MonoBehaviour 
{
    public abstract CharacterCombatHandler GetPlayerCombatHandler(Player player);
    public abstract CharacterCombatHandler GetEnemyCombatHandler(Enemy enemy);
}
