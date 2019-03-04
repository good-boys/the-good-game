using NUnit.Framework;

public class WeaponTest
{
    [Test]
    public void TestInit()
    {
        string weaponName = "WEAPON_NAME";

        int damage = 5;
        int defense = 2;
        int bonusAttack = 5;
        int bonusDefense = 2;

        Weapon weapon = new Weapon(weaponName, damage, defense, bonusAttack, bonusDefense);
        
        Assert.AreEqual(defense, weapon.Defense);
    }
}
