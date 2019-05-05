using NUnit.Framework;

public class PlayerTest
{
    [Test]
    public void TestCopyConfig()
    {
        Player player = new Player("test player", 84, 3);

        Player copy = player.CopyConfig() as Player;

        Assert.AreEqual(player.Name, copy.Name);
        Assert.AreEqual(player.MaxHealth, copy.Health);
        Assert.AreEqual(player.MaxHealth, copy.MaxHealth);
        Assert.AreEqual(player.Speed, copy.Speed);
    }
}
