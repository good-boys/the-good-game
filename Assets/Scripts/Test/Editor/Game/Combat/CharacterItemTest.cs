using NUnit.Framework;

public class CharacterItemTest
{
    [Test]
    public void TestInit()
    {
        string itemName = "ITEM_NAME";

        CharacterItem item = new CharacterItem(itemName);

        Assert.AreEqual(itemName, item.Name);
    }
}
