using NUnit.Framework;
using Moq;

public class CharacterTest
{
    Character character;
    Mock<CharacterCombatHandler> mockCombatHandler;

    [SetUp]
    public void Setup()
    {
        character = givenCharacter();
    }


   [Test]
	public void TestInitCharacter() 
    {
        string testName = "test";
        int testHealth = 100;
        int testSpeed = 1;
        character = new Character(testName, testHealth, testSpeed);

        Assert.AreEqual(testName, character.Name);
        Assert.AreEqual(testHealth, character.Health);
        Assert.AreEqual(testHealth, character.MaxHealth);
        Assert.AreEqual(testSpeed, character.Speed);
    }

    [Test]
    public void TestInitCharacter_SpeedLessThanZero()
    {
        string testName = "test";
        int testHealth = 100;
        int testSpeed = -1;
        character = new Character(testName, testHealth, testSpeed);

        Assert.AreEqual(testName, character.Name);
        Assert.AreEqual(testHealth, character.Health);
        Assert.AreEqual(testHealth, character.MaxHealth);
        Assert.AreEqual(0, character.Speed);
    }

    [Test]
    public void TestDamage()
    {
        int health = character.Health;
        int damage = 5;

        character.Damage(damage);

        Assert.AreEqual(health - damage, character.Health);
        mockCombatHandler.Verify(handler => handler.OnDamage(health - damage, character.MaxHealth, damage));
    }

    [Test]
    public void TestSubscribeCombatHandler()
    {
        CharacterCombatHandler mockHandler = new Mock<CharacterCombatHandler>().Object;

        character.SubscribeCombatHandler(mockHandler);

        mockCombatHandler.Verify(handler => handler.Subscribe(mockHandler));
    }

    [Test]
    public void TestEquipWeapon()
    {
        Weapon mockWeapon = new Mock<Weapon>("name", 0, 0).Object;

        character.EquipWeapon(mockWeapon);

        Assert.AreSame(mockWeapon, character.EquippedWeapon);
    }

    [Test]
    public void TestAttack()
    {
        Character mockTarget = new Mock<Character>("name", 100, 1).Object;
        Mock<Weapon> mockWeapon = new Mock<Weapon>("name", 0, 0);
        int weaponDamage = 5;
        mockWeapon.Setup(weapon => weapon.Damage).Returns(weaponDamage);
        character.EquipWeapon(mockWeapon.Object);

        Attack attack = character.Attack(mockTarget);
        attack.Use();

        Assert.AreSame(character, attack.Actor);
        Assert.AreSame(mockTarget, attack.Targets[0]);
        Assert.AreEqual(weaponDamage, attack.Damage);
        mockCombatHandler.Verify(handler => handler.OnAttack());
    }

    [Test]
    public void TestDefend()
    {
        Character mockTarget = new Mock<Character>("name", 100, 1).Object;
        Mock<Weapon> mockWeapon = new Mock<Weapon>("name", 0, 0);
        int weaponDefense = 5;
        mockWeapon.Setup(weapon => weapon.Defense).Returns(weaponDefense);
        character.EquipWeapon(mockWeapon.Object);

        Defend defend = character.Defend(mockTarget);
        defend.Use();

        Assert.AreSame(character, defend.Actor);
        Assert.AreSame(mockTarget, defend.Targets[0]);
        Assert.AreEqual(weaponDefense, defend.Defense);
        mockCombatHandler.Verify(handler => handler.OnDefend());
    }

    [Test]
    public void TestSetActiveAction()
    {
        CharacterAction mockAction = new Mock<CharacterAction>(character, null, null).Object;

        character.SetActiveAction(mockAction);

        Assert.AreSame(mockAction, character.ActiveAction);
    }

    [Test]
    public void TestUseActiveAction()
    {
        CharacterAction mockAction = new Mock<CharacterAction>(character, null, null).Object;
        character.SetActiveAction(mockAction);

        CharacterAction usedAction = character.UseActiveAction();

        Assert.IsNull(character.ActiveAction);
        Assert.AreSame(mockAction, usedAction);
    }

    Character givenCharacter()
    {
        string testName = "test";
        int testHealth = 100;
        int testSpeed = 1;
        mockCombatHandler = new Mock<CharacterCombatHandler>();
        mockCombatHandler.Setup(handler => handler.Subscribe(It.IsAny<CharacterCombatHandler>()));
        mockCombatHandler.Setup(handler => handler.OnAttack());
        mockCombatHandler.Setup(handler => handler.OnDefend());
        return new Character(testName, testHealth, testSpeed, mockCombatHandler.Object);
    }
}
