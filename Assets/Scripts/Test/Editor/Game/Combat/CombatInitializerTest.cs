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

    // TODO
    [Test]
    public void TestStart()
    {

    }
}
