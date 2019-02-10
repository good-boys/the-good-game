using UnityEngine;
using NUnit.Framework;
using Moq;

public class CharacterManagerTest
{
    CharacterManager characterManager;
    GameObject gameObject;
    Mock<StatManager> mockStatManager;
    Mock<EnemyActionManager> mockEnemyActionManager;

    [SetUp]
    public void Setup()
    {
        mockStatManager = new Mock<StatManager>();
        mockEnemyActionManager = new Mock<EnemyActionManager>();
        mockStatManager.Setup(manager => manager.ProcessAction(It.IsAny<CharacterAction>()));
        gameObject = new GameObject();
        characterManager = gameObject.AddComponent<CharacterManager>();
        characterManager.Init(mockStatManager.Object, mockEnemyActionManager.Object);
    }

    [Test]
    public void TestRequestNextAction()
    {
        Player mockPlayer = new Mock<Player>("name", 100, 1.0f).Object;
        Enemy mockEnemy = new Mock<Enemy>("name", 100, 1.0f).Object;
        CharacterAction mockCharacterAction = new Mock<CharacterAction>(mockEnemy, null, null).Object;
        mockEnemyActionManager.Setup(manager => manager.RequestNextAction(It.IsAny<Enemy>(), It.IsAny<Player>())).Returns(mockCharacterAction);
        characterManager.RegisterCharacter(mockPlayer);

        CharacterAction characterAction = characterManager.RequestNextAction(mockEnemy);

        Assert.AreSame(mockCharacterAction, characterAction);
        mockEnemyActionManager.Verify(manager => manager.RequestNextAction(mockEnemy, mockPlayer));
    }

    [Test]
    public void TestRegisterCharacter_Player()
    {
        Player mockPlayer = new Mock<Player>("name", 100, 1.0f).Object;

        characterManager.RegisterCharacter(mockPlayer);

        Assert.AreSame(mockPlayer, characterManager.GetActivePlayer());
    }

    [Test]
    public void TestRegisterCharacter_Enemy()
    {
        Enemy mockEnemy = new Mock<Enemy>("name", 100, 1.0f).Object;

        characterManager.RegisterCharacter(mockEnemy);

        Assert.AreSame(mockEnemy, characterManager.GetEnemies()[0]);
    }

    [Test]
    public void TestProcessAction_Attack()
    {
        Attack mockAttack = new Mock<Attack>(null, 5, null, null).Object;

        characterManager.ProcessAction(mockAttack);

        mockStatManager.Verify(manager => manager.ProcessAction(mockAttack));
    }

    [Test]
    public void TestProcessAction_Defend()
    {
        Mock<Defend> mockDefend = new Mock<Defend>(null, 5, null, null);
        Mock<Character> mockActor = new Mock<Character>("name", 100, 1.0f);
        mockDefend.Setup(defend => defend.Actor).Returns(mockActor.Object);
        mockActor.Setup(actor => actor.SetActiveAction(It.IsAny<CharacterAction>()));

        characterManager.ProcessAction(mockDefend.Object);

        mockStatManager.Verify(manager => manager.ProcessAction(mockDefend.Object));
        mockDefend.Verify(defend => defend.Actor);
        mockActor.Verify(actor => actor.SetActiveAction(mockDefend.Object));
    }
}
