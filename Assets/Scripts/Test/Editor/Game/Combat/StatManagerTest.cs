using UnityEngine;
using NUnit.Framework;
using Moq;

public class StatManagerTest
{
    GameObject gameObject;
    StatManager statManager;

    Mock<Attack> mockAttack;
    Mock<Defend> mockDefend;
    Mock<Defend> mockDefend_defenseTooHigh;
    Mock<Character> mockActor;
    Mock<Character> mockTarget;

    int attackDamage = 10;
    int defendDamageReduction = 5;
    int tooHighDefendDamageReduction = 15;

    [SetUp]
    public void Setup()
    {
        gameObject = new GameObject();
        statManager = gameObject.AddComponent<StatManager>();

        mockAttack = new Mock<Attack>(null, attackDamage, null, null);
        mockDefend = new Mock<Defend>(null, defendDamageReduction, null, null);
        mockDefend_defenseTooHigh = new Mock<Defend>(null, tooHighDefendDamageReduction, null, null);
        mockActor = new Mock<Character>("name", 100);
        mockTarget = new Mock<Character>("name", 100);
        
        mockAttack.Setup(attack => attack.Targets).Returns(new Character[] { mockTarget.Object });
        mockAttack.Setup(attack => attack.Actor).Returns(mockActor.Object);
        mockAttack.Setup(attack => attack.Damage).Returns(attackDamage);
        mockDefend.Setup(defend => defend.Targets).Returns(new Character[] { mockActor.Object });
        mockDefend.Setup(defend => defend.Defense).Returns(defendDamageReduction);
        mockDefend_defenseTooHigh.Setup(defend => defend.Targets).Returns(new Character[] { mockActor.Object });
        mockDefend_defenseTooHigh.Setup(defend => defend.Defense).Returns(tooHighDefendDamageReduction);

        mockTarget.Setup(target => target.Damage(It.IsAny<int>()));
        mockTarget.Setup(target => target.UseActiveAction());
    }

    [Test]
    public void TestProcessAction_Attack()
    {
        mockTarget.Setup(target => target.ActiveAction);

        statManager.ProcessAction(mockAttack.Object);

        mockTarget.Verify(target => target.ActiveAction);
        mockTarget.Verify(target => target.Damage(attackDamage));
    }

    [Test]
    public void TestProcessAction_Attack_TargetDefending()
    {
        mockTarget.Setup(target => target.ActiveAction).Returns(mockDefend.Object);

        statManager.ProcessAction(mockAttack.Object);

        mockTarget.Verify(target => target.ActiveAction);
        mockAttack.Verify(attack => attack.Actor);
        mockTarget.Verify(target => target.UseActiveAction());
        mockTarget.Verify(target => target.Damage(attackDamage - defendDamageReduction));
    }

    [Test]
    public void TestProcessAction_Attack_TargetDefending_DefenseHigherThanAttack()
    {
        mockTarget.Setup(target => target.ActiveAction).Returns(mockDefend_defenseTooHigh.Object);

        statManager.ProcessAction(mockAttack.Object);

        mockTarget.Verify(target => target.ActiveAction);
        mockAttack.Verify(attack => attack.Actor);
        mockTarget.Verify(target => target.UseActiveAction());
        mockTarget.Verify(target => target.Damage(0));
    }
}
