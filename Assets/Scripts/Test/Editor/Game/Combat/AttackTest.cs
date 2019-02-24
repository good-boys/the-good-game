using NUnit.Framework;

public class AttackTest
{
    [Test]
    public void TestInit()
    {
        int damage = 5;
        int bonus = 5;

        Attack attack = new Attack(null, damage, bonus, null, null);

        Assert.AreEqual(damage, attack.Damage);
    }
}
