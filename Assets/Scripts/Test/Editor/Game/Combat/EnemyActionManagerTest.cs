using UnityEngine;
using NUnit.Framework;
using Moq;

public class EnemyActionManagerTest
{
    GameObject gameObject;
    EnemyActionManager enemyActionManager;
    Mock<RandomGenerator> mockRandomGenerator;
    Mock<Enemy> mockEnemy;
    Mock<Player> mockPlayer;
    Mock<Attack> mockAttack;
    Mock<Defend> mockDefend;

    [SetUp]
    public void Setup()
    {
        gameObject = new GameObject();

        enemyActionManager = gameObject.AddComponent<EnemyActionManager>();

        mockRandomGenerator = new Mock<RandomGenerator>();
        mockPlayer = new Mock<Player>("name", 100, 1.0f);
        mockEnemy = new Mock<Enemy>("name", 100, 1.0f);
        mockAttack = new Mock<Attack>(null, 5, null, null);
        mockDefend = new Mock<Defend>(null, 5, null, null);

        mockEnemy.Setup(enemy => enemy.Attack(mockPlayer.Object)).Returns(mockAttack.Object);
        mockEnemy.Setup(enemy => enemy.Defend(mockPlayer.Object)).Returns(mockDefend.Object);

        enemyActionManager.Init(mockRandomGenerator.Object);
    }

    [Test]
    public void TestRequestNextAction_Attack()
    {
        mockRandomGenerator.Setup(generator => generator.Generate(0, 1f)).Returns(1f);

        CharacterAction attack = enemyActionManager.RequestNextAction(mockEnemy.Object, mockPlayer.Object);

        Assert.AreSame(mockAttack.Object, attack);
        mockEnemy.Verify(enemy => enemy.Attack(mockPlayer.Object));
    }

    [Test]
    public void TestRequestNextAction_Defend()
    {
        mockRandomGenerator.Setup(generator => generator.Generate(0, 1f)).Returns(0);

        CharacterAction defend = enemyActionManager.RequestNextAction(mockEnemy.Object, mockPlayer.Object);

        Assert.AreSame(mockDefend.Object, defend);
        mockEnemy.Verify(enemy => enemy.Defend(mockPlayer.Object));
    }
}
