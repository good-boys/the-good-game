using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System;

[Serializable]
public class Level
{

    [Header("Level")]
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
    int enemyWeaponBonusAttack;

    [SerializeField]
    int enemyWeaponBonusDefense;

    [SerializeField]
    int enemySpeed = 1;

    [SerializeField]
    CharacterActionPattern enemyActionPattern;

    public Enemy GetEnemy()
    {
        Enemy enemy = new Enemy(enemyName, enemyHealth, enemySpeed);

        enemy.EquipWeapon(new Weapon(enemyWeaponName, enemyWeaponDamage, enemyWeaponDefense, enemyWeaponBonusAttack, enemyWeaponBonusDefense, 0, 0, 0));

        enemy.SetActionPattern(enemyActionPattern);

        return enemy;
    }
}

[Serializable]
public class WeaponStats
{
    [SerializeField]
    string Name;

    [SerializeField]
    int Damage = 0;

    [SerializeField]
    int Defense = 0;

    [SerializeField]
    int BonusAttack = 0;

    [SerializeField]
    int BonusDefense = 0;

    [SerializeField]
    float GoalSize = 0;

    [SerializeField]
    float GoalPos = 0;

    [SerializeField]
    float TimerSpeed = 0;

    public Weapon GetWeapon(int weaponID)
    {
        return new Weapon(Name, Damage, Damage, BonusAttack, BonusDefense, GoalSize, GoalPos, TimerSpeed);
    }
}

public class GameFlowManager : MonoBehaviour 
{

    public List<Level> levels = new List<Level>();
    public List<WeaponStats> Weapons = new List<WeaponStats>();

    public CombatUI combatUI;
    public CombatManager combatManager;
    public GameObject camera;
    public bool inBattle;

    public static GameFlowManager instance = null;
    public AudioManager audioManager;
    public CombatInitializer combatInitializer;
    public CombatConfig combatConfig;
    public DataInitializer dataInitializer;

    public int currentWeapon = 0;
    public int currentLevel;
    bool loading;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        
        combatUI.gameObject.SetActive(false);
        camera.SetActive(false);

        

        StartCoroutine(FirstLoad());
    }

    IEnumerator FirstLoad()
    {
        if (combatConfig == null || combatInitializer == null || dataInitializer.SaveManager == null)
        {
            yield return null;
        }

        dataInitializer.SaveManager.Erase();

        if (combatUI.GetPlayerCombatHandler(dataInitializer.GameSave.Player) == null)
        {
            yield return null;
        }

        combatConfig.Initialize(dataInitializer.GameSave.Player, levels[currentLevel].GetEnemy());
        combatInitializer.Initialize();
        inBattle = true;
        SceneManager.LoadScene("Intro", LoadSceneMode.Additive);
    }

    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);

        if (sceneName == "Intro")
        {
            combatUI.gameObject.SetActive(true);
            camera.SetActive(true);
        }

        audioManager.Stop("Trees");
        audioManager.Play("Music 1", new AudioOptions(3,0,0.5f,true,false));
    }

    public void GameOver()
    {
        Debug.Log("You Lost");
        RestartLevel();
    }

    public void RestartLevel()
    {
        if (loading)
            return;

        StartCoroutine(ReloadLevel());
    }

    public void NextLevel()
    {
        if (loading)
            return;

        currentLevel++;
        if (currentLevel >= levels.Count)
        {
            loading = true;
            combatUI.DoFadeOut();
            Debug.Log("You won!");
        }
        else
        {
            StartCoroutine(ReloadLevel());
        }
    }

    IEnumerator ReloadLevel()
    {
        loading = true;
        combatUI.DoFadeOut();
        combatManager.Reset();

        yield return new WaitForSeconds(1f);
        
        Level level = levels[currentLevel];

        combatConfig.Initialize(dataInitializer.GameSave.Player, level.GetEnemy());

        combatUI.Reset();
        combatUI.DoFadeIn();
        combatInitializer.Initialize();
        loading = false;
        inBattle = true;
    }

    //OnEnemyDefeated function

    void Rest()
    {

    }

    void Upgrade()
    {
        int upgrade = currentWeapon++;
        Mathf.Clamp(upgrade, 0, Weapons.Count);
        combatConfig.GetPlayer().EquipWeapon(Weapons[upgrade].GetWeapon(upgrade));
        dataInitializer.SaveManager.Save(dataInitializer.GameSave);

        NextLevel();
    }

}
