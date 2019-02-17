using System;
using Eppy;
using NUnit.Framework;

public class CharacterCombatHandlerTest
{
    Action testAttackHandler;
    Action testDefendHandler;
    Action<int, int, int> testDamageHandler;
    Action testDeathHandler;
    bool testAttackHandlerRun;
    bool testDefendHandlerRun;
    bool testDamageHandlerRun;
    bool testDeathHandlerRun;
    Tuple<int, int, int> damageHandlerValues = null;
    CharacterCombatHandler combatHandler;

    [SetUp]
    public void Setup()
    {
        testAttackHandler = () =>
         {
             testAttackHandlerRun = true;
         };
        testDefendHandler = () =>
         {
             testDefendHandlerRun = true;
         };
        testDamageHandler = (int remainingHealth, int maxHealth, int damage) =>
         {
             testDamageHandlerRun = true;
             damageHandlerValues = new Tuple<int, int, int>(remainingHealth, maxHealth, damage);
         };
        testDeathHandler = () =>
         {
             testDeathHandlerRun = true;
         };
        combatHandler = new CharacterCombatHandler(testAttackHandler,
                                                    testDefendHandler,
                                                    testDamageHandler,
                                                    testDeathHandler);
    }

    [TearDown]
    public void TearDown()
    {
        testAttackHandlerRun = false;
        testDefendHandlerRun = false;
        testDamageHandlerRun = false;
        testDeathHandlerRun = false;
        damageHandlerValues = null;
    }

    [Test]
    public void TestInit()
    {
        combatHandler = new CharacterCombatHandler(testAttackHandler,
                                                    testDefendHandler,
                                                    testDamageHandler,
                                                    testDeathHandler);


        Assert.True(combatHandler != null);
    }

    [Test]
    public void TestSubscribe()
    {
        CharacterCombatHandler targetCombatHandler = new CharacterCombatHandler();

        targetCombatHandler.Subscribe(combatHandler);

        Assert.AreEqual(combatHandler, targetCombatHandler);
    }

    [Test]
    public void TestOnAttack()
    {
        combatHandler.OnAttack();

        Assert.True(testAttackHandlerRun);
    }

    [Test]
    public void TestOnDefend()
    {
        combatHandler.OnDefend();

        Assert.True(testDefendHandlerRun);
    }

    [Test]
    public void TestOnDamage()
    {
        int remainingHealth = 5;
        int maxHealth = 10;
        int damageDone = 5;
        combatHandler.OnDamage(remainingHealth, maxHealth, damageDone);

        Assert.True(testDamageHandlerRun);
        Assert.AreEqual(remainingHealth, damageHandlerValues.Item1);
        Assert.AreEqual(maxHealth, damageHandlerValues.Item2);
        Assert.AreEqual(damageDone, damageHandlerValues.Item3);
    }

    [Test]
    public void TestOnDeath()
    {
        combatHandler.OnDeath();

        Assert.True(testDeathHandlerRun);
    }
}
