using System;
using NUnit.Framework;
using Moq;

public class CharacterActionTest
{
    [Test]
    public void TestInit()
    {
        Character mockActor = new Mock<Character>("name", 100, 1).Object;
        Character mockTarget = new Mock<Character>("name", 100, 1).Object;

        CharacterAction characterAction = new CharacterAction(mockActor, null, mockTarget);

        Assert.AreSame(mockActor, characterAction.Actor);
        Assert.AreSame(mockTarget, characterAction.Targets[0]);
    }

    [Test]
    public void TestUse()
    {
        bool actionRun = false;
        Action testAction =()=> 
        {
            actionRun = true;
        };
        Character mockActor = new Mock<Character>("name", 100, 1).Object;
        Character mockTarget = new Mock<Character>("name", 100, 1).Object;
        CharacterAction characterAction = new CharacterAction(mockActor, testAction, mockTarget);

        characterAction.Use();

        Assert.True(actionRun);
    }
}
