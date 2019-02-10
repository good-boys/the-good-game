using UnityEngine;

public class TestCombatConfig : AbstractCombatConfig
{
    [Header("Player")]
    [SerializeField]
    string playerName = "Player";

    [SerializeField]
    int playerHealth;

    [SerializeField]
    string playerWeaponName = "Great Sword";

    [SerializeField]
    int playerWeaponDamage;

    [SerializeField]
    int playerWeaponDefense;

    [SerializeField]
    float playerSpeed = 1.0f;

    [Header("Enemy")]
    [SerializeField]
    string enemyName = "Troll";

    [SerializeField]
    string enemyWeaponName = "Club";

    [SerializeField]
    int enemyHealth;

    [SerializeField]
    int enemyWeaponDamage;

    [SerializeField]
    int enemyWeaponDefense;

    [SerializeField]
    float enemySpeed = 1.0f;

    Player player;
    Enemy enemy;

    void Awake()
    {
        player = new Player(playerName, playerHealth, playerSpeed);
        player.EquipWeapon(new Weapon(playerWeaponName, playerWeaponDamage, playerWeaponDefense));
        enemy = new Enemy(enemyName, enemyHealth, enemySpeed);
        enemy.EquipWeapon(new Weapon(enemyWeaponName, enemyWeaponDamage, enemyWeaponDefense));
    }

    public override Player GetPlayer()
    {
        return player;
    }

    public override Enemy[] GetEnemies()
    {
        return new Enemy[]{enemy};
    }
}
