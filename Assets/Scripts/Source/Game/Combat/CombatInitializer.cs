using UnityEngine;

public class CombatInitializer : MonoBehaviour 
{
    [SerializeField]
    CombatManager combatManager;

    [SerializeField]
    AbstractCombatConfig combatConfig;

    [SerializeField]
    AbstractCombatUI combatUI;

    [SerializeField]
    CharacterManager characterManager;

    [SerializeField]
    TurnManager turnManager;

    [SerializeField]
    GameFlowManager gameFlowManager;

    void Start()
    {
        setupCharacterManager();
        setupCombatUI();
        combatManager.Init(characterManager, turnManager, gameFlowManager);
    }

    void setupCharacterManager()
    {
        characterManager.RegisterCharacter(combatConfig.GetPlayer());
        foreach (Enemy enemy in combatConfig.GetEnemies())
        {
            characterManager.RegisterCharacter(enemy);
        }
    }

    void setupCombatUI()
    {
        Player player = combatConfig.GetPlayer();
        player.SubscribeCombatHandler(combatUI.GetPlayerCombatHandler(player));
        foreach (Enemy enemy in combatConfig.GetEnemies())
        {
            enemy.SubscribeCombatHandler(combatUI.GetEnemyCombatHandler(enemy));
        }
    }
}
