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
        character = new Character(testName, testHealth);

        Assert.AreEqual(testName, character.Name);
        Assert.AreEqual(testHealth, character.Health);
        Assert.AreEqual(testHealth, character.MaxHealth);
    }

    [Test]
    public void TestDamage()
    {
        int health = character.Health;
        int damage = 5;

        character.Damage(damage);

        Assert.AreEqual(health - damage, character.Health);
    }

    [Test]
    public void TestSubscribeCombatHandler()
    {
        CharacterCombatHandler mockHandler = new Mock<CharacterCombatHandler>().Object;

        character.SubscribeCombatHandler(mockHandler);

        mockCombatHandler.Verify(handler => handler.Subscribe(mockHandler));
    }

    Character givenCharacter()
    {
        string testName = "test";
        int testHealth = 100;
        mockCombatHandler = new Mock<CharacterCombatHandler>();
        mockCombatHandler.Setup(handler => handler.Subscribe(It.IsAny<CharacterCombatHandler>()));
        return new Character(testName, testHealth, mockCombatHandler.Object);
    }
}
