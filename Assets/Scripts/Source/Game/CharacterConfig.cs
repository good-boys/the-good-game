using System;
using UnityEngine;

[Serializable]
public class CharacterConfig
{
    public string Name { get{ return name; } }
    public int Health { get { return health; } }
    public int Speed { get { return speed; } }

    [SerializeField]
    string name;

    [SerializeField]
    int health;

    [SerializeField]
    int speed;

    public CharacterConfig(string name, int health, int speed)
    {
        this.name = name;
        this.health = health;
        this.speed = speed;
    }
}
