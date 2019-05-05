using NUnit.Framework;

public class DefendTest
{
    [Test]
    public void TestInit()
    {
        int defense = 5;
        int bonus = 5;

        Defend defend = new Defend(null, defense, bonus, null, null);

        Assert.AreEqual(defense, defend.Defense);
    }
}
