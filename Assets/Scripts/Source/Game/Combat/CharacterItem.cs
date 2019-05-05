using System;

[Serializable]
public class CharacterItem
{
    public string Name
    {
        get;
        private set;
    }

    public CharacterItem(string name)
    {
        this.Name = name;
    }
}
