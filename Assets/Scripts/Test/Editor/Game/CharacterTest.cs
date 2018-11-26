using NUnit.Framework;

public class CharacterTest 
{
	[Test]
	public void TestInitCharacter() 
    {
        string testName = "test";
        int testHealth = 100;
        Character character = new Character(testName, testHealth);

        Assert.AreEqual(testName, character.Name);
        Assert.AreEqual(testHealth, character.Health);
        Assert.AreEqual(testHealth, character.MaxHealth);
    }
}
