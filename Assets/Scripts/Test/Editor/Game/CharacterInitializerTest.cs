using UnityEngine;
using NUnit.Framework;
using Moq;

public class CharacterInitializerTest 
{
    CharacterInitializer characterInitializer;
    GameObject gameObject;
    Mock<CharacterManager> mockCharacterManager;
    Mock<StatManager> mockStatManager;
    Mock<EnemyActionManager> mockEnemyActionManager;

    [SetUp]
    public void Setup()
    {
        mockCharacterManager = new Mock<CharacterManager>();
        mockStatManager = new Mock<StatManager>();
        mockEnemyActionManager = new Mock<EnemyActionManager>();
        mockEnemyActionManager.Setup(enemyActionManager => enemyActionManager.Init(It.IsAny<RandomGenerator>()));
        mockCharacterManager.Setup(manager => manager.Init(mockStatManager.Object,
                                                           mockEnemyActionManager.Object));
        gameObject = new GameObject();
        characterInitializer = gameObject.AddComponent<CharacterInitializer>();
        
        characterInitializer.Init(mockCharacterManager.Object,
                                  mockStatManager.Object,
                                  mockEnemyActionManager.Object);
    }

    [Test]
    public void TestAwake()
    {
        characterInitializer.Awake();

        mockEnemyActionManager.Verify(manager => manager.Init(It.IsAny<RandomGenerator>()));
        mockCharacterManager.Verify(manager => manager.Init(mockStatManager.Object,
                                                           mockEnemyActionManager.Object));
    }
}
