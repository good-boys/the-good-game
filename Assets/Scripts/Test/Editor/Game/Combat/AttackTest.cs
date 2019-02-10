using NUnit.Framework;

public class AttackTest
{
    [Test]
    public void TestInit()
    {
        int damage = 5;

        Attack attack = new Attack(null, damage, null, null);

        Assert.AreEqual(damage, attack.Damage);
    }

    // TODO: Remove after verifying fail terminates Travis build
    [Test]
    public void AlwaysFail()
    {
        Assert.True(false);
    }
}
