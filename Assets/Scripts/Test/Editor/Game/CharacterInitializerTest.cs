using NUnit.Framework;
using Moq;

public class CharacterInitializerTest 
{
    CharacterInitializer characterInitializer;
    Mock<CharacterManager> mockCharacterManager;
    Mock<StatManager> mockStatManager;
    Mock<EnemyActionManager> mockEnemyActionManager;

    [SetUp]
    public void Setup()
    {
        mockCharacterManager = new Mock<CharacterManager>();
        mockStatManager = new Mock<StatManager>();
        mockEnemyActionManager = new Mock<EnemyActionManager>();
        mockCharacterManager.Setup(manager => manager.Init(mockStatManager.Object,
                                                           mockEnemyActionManager.Object));
        characterInitializer = new CharacterInitializer(mockCharacterManager.Object,
                                                        mockStatManager.Object,
                                                        mockEnemyActionManager.Object);
    }

    [Test]
    public void TestAwake()
    {
        characterInitializer.Awake();

        mockCharacterManager.Verify(manager => manager.Init(mockStatManager.Object,
                                                           mockEnemyActionManager.Object));
    }
}
