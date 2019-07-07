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

    public void Init(CombatManager combatManager,
                        AbstractCombatConfig combatConfig,
                        AbstractCombatUI combatUI,
                        CharacterManager characterManager,
                        TurnManager turnManager,
                        GameFlowManager gameFlowManager)
    {
        this.combatManager = combatManager;
        this.combatConfig = combatConfig;
        this.combatUI = combatUI;
        this.characterManager = characterManager;
        this.turnManager = turnManager;
        this.gameFlowManager = gameFlowManager;
    }

    public void Initialize()
    {
        setupCharacterManager();
        setupCombatUI();
        combatManager.Init(characterManager, turnManager, gameFlowManager);
    }

    void setupCharacterManager()
    {
        characterManager.RegisterCharacter(combatConfig.GetPlayer());
        characterManager.RegisterCharacter(combatConfig.GetEnemy());
    }

    void setupCombatUI()
    {
        Player player = combatConfig.GetPlayer();
        Enemy enemy = combatConfig.GetEnemy();
        player.SubscribeCombatHandler(combatUI.GetPlayerCombatHandler(player));
        enemy.SubscribeCombatHandler(combatUI.GetEnemyCombatHandler(enemy));
    }
}
