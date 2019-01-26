using UnityEngine;
using NUnit.Framework;
using Moq;

public class CombatInitializerTest
{
    Mock<CombatManager> mockCombatManager;
    Mock<AbstractCombatConfig> mockCombatConfig;
    Mock<AbstractCombatUI> mockCombatUI;
    Mock<CharacterManager> mockCharacterManager;
    Mock<TurnManager> mockTurnManager;
    Mock<GameFlowManager> mockGameFlowManager;
    GameObject gameObject;
    CombatInitializer combatInitializer;

    [SetUp]
    public void Setup()
    {
        mockCombatManager = new Mock<CombatManager>();
        mockCombatConfig = new Mock<AbstractCombatConfig>();
        mockCombatUI = new Mock<AbstractCombatUI>();
        mockCharacterManager = new Mock<CharacterManager>();
        mockTurnManager = new Mock<TurnManager>();
        mockGameFlowManager = new Mock<GameFlowManager>();

        gameObject = new GameObject();

        combatInitializer = gameObject.AddComponent<CombatInitializer>();
        combatInitializer.Init(mockCombatManager.Object,
                                mockCombatConfig.Object,
                                mockCombatUI.Object,
                                mockCharacterManager.Object,
                                mockTurnManager.Object,
                                mockGameFlowManager.Object);
    }
    
    [Test]
    public void TestStart()
    {
        Mock<Player> mockPlayer = new Mock<Player>("name", 100);
        Mock<Enemy> mockEnemy = new Mock<Enemy>("name", 100);
        Mock <CharacterCombatHandler> mockPlayerCombatHandler = new Mock<CharacterCombatHandler>();
        Mock<CharacterCombatHandler> mockEnemyCombatHandler = new Mock<CharacterCombatHandler>();
        mockCombatConfig.Setup(combatConfig => combatConfig.GetPlayer()).Returns(mockPlayer.Object);
        mockCharacterManager.Setup(characterManager => characterManager.RegisterCharacter(It.IsAny<Player>()));
        mockCombatConfig.Setup(combatConfig => combatConfig.GetEnemies()).Returns(new Enemy[]{mockEnemy.Object});
        mockCharacterManager.Setup(characterManager => characterManager.RegisterCharacter(It.IsAny<Enemy>()));
        mockPlayer.Setup(player => player.SubscribeCombatHandler(It.IsAny<CharacterCombatHandler>()));
        mockCombatUI.Setup(combatUI => combatUI.GetPlayerCombatHandler(It.IsAny<Player>())).Returns(mockPlayerCombatHandler.Object);
        mockCombatUI.Setup(combatUI => combatUI.GetEnemyCombatHandler(It.IsAny<Enemy>())).Returns(mockEnemyCombatHandler.Object);
        mockCombatManager.Setup(combatManager => combatManager.Init(It.IsAny<CharacterManager>(),
                                                                    It.IsAny<TurnManager>(),
                                                                    It.IsAny<GameFlowManager>()));

        combatInitializer.Start();

        mockCharacterManager.Verify(characterManager => characterManager.RegisterCharacter(mockPlayer.Object));
        mockCombatConfig.Verify(combatConfig => combatConfig.GetEnemies());
        mockCharacterManager.Verify(characterManager => characterManager.RegisterCharacter(mockEnemy.Object));
        mockCombatConfig.Verify(combatConfig => combatConfig.GetPlayer());
        mockPlayer.Verify(player => player.SubscribeCombatHandler(mockPlayerCombatHandler.Object));
        mockCombatConfig.Verify(combatConfig => combatConfig.GetEnemies());
        mockEnemy.Verify(enemy => enemy.SubscribeCombatHandler(mockEnemyCombatHandler.Object));
        mockCombatManager.Verify(combatManager => combatManager.Init(mockCharacterManager.Object,
                                                                     mockTurnManager.Object,
                                                                     mockGameFlowManager.Object));
    }
}
