using NUnit.Framework;
using Moq;

public class EnemyTest
{
    Enemy enemy;
    Player player;
    Mock<CharacterActionPattern> mockPattern;

    [SetUp]
    public void Setup()
    {
        mockPattern = new Mock<CharacterActionPattern>();
        enemy = new Enemy("Goblin", 43, 2);
        player = new Player("Knight", 200, 1);
    }

    [Test]
    public void TestHasPattern_NotSet()
    {
        Assert.False(enemy.HasPattern);
    }

    [Test]
    public void TestSetActionPattern()
    {
        enemy.SetActionPattern(mockPattern.Object);

        Assert.True(enemy.HasPattern);
    }

    [Test]
    public void TestNextActionFromPattern_Defend()
    {
        mockPattern.SetupGet(pattern => pattern[It.IsAny<int>()])
            .Returns(new Mock<DefendTemplate>().Object);
        mockPattern.SetupGet(pattern => pattern.Length).Returns(1);
        enemy.SetActionPattern(mockPattern.Object);

        CharacterAction action = enemy.NextActionFromPattern(player);

        Assert.True(action is Defend);
        Assert.AreSame(player, action.Targets[0]);
    }

    [Test]
    public void TestNextActionFromPattern_Attack()
    {
        AttackDirection attackDirection = AttackDirection.Down;
        Mock<AttackTemplate> mockAttackTemplate = new Mock<AttackTemplate>();
        mockAttackTemplate.SetupGet(attack => attack.Direction).Returns(attackDirection);
        mockPattern.SetupGet(pattern => pattern[It.IsAny<int>()])
            .Returns(mockAttackTemplate.Object);
        mockPattern.SetupGet(pattern => pattern.Length).Returns(1);
        enemy.SetActionPattern(mockPattern.Object);

        CharacterAction action = enemy.NextActionFromPattern(player);

        Assert.True(action is Attack);
        Assert.AreSame(player, action.Targets[0]);
        Assert.AreEqual(attackDirection, (action as Attack).Direction);
    }

    [Test]
    public void TestNextActionFromPattern_NoActions()
    {
        mockPattern.SetupGet(pattern => pattern.Length).Returns(0);
        enemy.SetActionPattern(mockPattern.Object);

        Assert.Throws<NoCharacterActionExistsException>(delegate ()
        {
            enemy.NextActionFromPattern(player);
        });
    }

    [Test]
    public void TestNextActionFromPattern_UnsupportedActionType()
    {
        mockPattern.SetupGet(pattern => pattern[It.IsAny<int>()])
            .Returns(new Mock<CharacterActionTemplate>().Object);
        mockPattern.SetupGet(pattern => pattern.Length).Returns(1);
        enemy.SetActionPattern(mockPattern.Object);

        Assert.Throws<NoCharacterActionExistsException>(delegate ()
        {
            enemy.NextActionFromPattern(player);
        });
    }

    [Test]
    public void TestCopyConfig()
    {
        Weapon weapon = new Weapon("test_weapon", 1, 1, 1, 1, 1f, 1f, 1f);
        enemy.SetActionPattern(mockPattern.Object);
        enemy.EquipWeapon(weapon);

        Enemy copy = enemy.CopyConfig() as Enemy;

        Assert.AreEqual(enemy.Name, copy.Name);
        Assert.AreEqual(enemy.MaxHealth, copy.Health);
        Assert.AreEqual(enemy.MaxHealth, copy.MaxHealth);
        Assert.AreEqual(enemy.Speed, copy.Speed);
        Assert.True(copy.HasPattern);
        Assert.AreEqual(weapon, copy.EquippedWeapon);
    }
}
