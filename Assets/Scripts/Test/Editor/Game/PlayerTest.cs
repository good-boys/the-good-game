using NUnit.Framework;

public class PlayerTest
{
    [Test]
    public void TestCopyConfig()
    {
        Weapon weapon = new Weapon("test_weapon", 1, 1, 1, 1, 1f, 1f, 1f);
        Player player = new Player("test player", 84, 3);
        player.EquipWeapon(weapon);

        Player copy = player.CopyConfig() as Player;

        Assert.AreEqual(player.Name, copy.Name);
        Assert.AreEqual(player.MaxHealth, copy.Health);
        Assert.AreEqual(player.MaxHealth, copy.MaxHealth);
        Assert.AreEqual(player.Speed, copy.Speed);
        Assert.AreEqual(weapon, copy.EquippedWeapon);
    }
}
