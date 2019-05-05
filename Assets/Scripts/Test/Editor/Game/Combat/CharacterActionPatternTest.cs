using NUnit.Framework;
using Moq;

public class CharacterActionPatternTest
{
    CharacterActionPattern pattern;

    [SetUp]
    public void Setup()
    {
        pattern = new CharacterActionPattern();
    }

    [Test]
    public void TestGetPattern()
    {
        CharacterActionTemplate[] actions = { new Mock<CharacterActionTemplate>().Object };

        pattern.Init(actions);

        Assert.AreSame(actions, pattern.Pattern);
    }

    [Test]
    public void TestGetLength()
    {
        CharacterActionTemplate[] actions = { new Mock<CharacterActionTemplate>().Object };

        pattern.Init(actions);

        Assert.AreEqual(actions.Length, pattern.Length);
    }
}
