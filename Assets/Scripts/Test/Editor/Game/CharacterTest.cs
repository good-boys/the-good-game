using NUnit.Framework;

public class CharacterTest
{
    Character character;

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

    Character givenCharacter()
    {
        string testName = "test";
        int testHealth = 100;
        return new Character(testName, testHealth);
    }
}
