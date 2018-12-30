using NUnit.Framework;
using Moq;

public class CharacterActionTest
{
    [Test]
    public void TestInit()
    {
        Character mockActor = new Mock<Character>("name", 100).Object;
        Character mockTarget = new Mock<Character>("name", 100).Object;

        CharacterAction characterAction = new CharacterAction(mockActor, null, mockTarget);

        Assert.AreSame(mockActor, characterAction.Actor);
        Assert.AreSame(mockTarget, characterAction.Targets[0]);
    }

}
