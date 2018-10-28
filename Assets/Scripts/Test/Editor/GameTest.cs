using NUnit.Framework;

public class GameTest {

	[Test]
	public void TestSimplePasses() {
        Game game = new Game();
        Assert.IsTrue(game.Running);
	}
}
