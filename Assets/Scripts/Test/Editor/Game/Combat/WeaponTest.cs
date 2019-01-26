using NUnit.Framework;

public class WeaponTest
{
    [Test]
    public void TestInit()
    {
        string weaponName = "WEAPON_NAME";
        int damage = 5;
        int defense = 2;

        Weapon weapon = new Weapon(weaponName, damage, defense);

        Assert.AreEqual(damage, weapon.Damage);
        Assert.AreEqual(defense, weapon.Defense);
    }
}
