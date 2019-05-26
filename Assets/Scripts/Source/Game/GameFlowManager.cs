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

public class GameFlowManager : MonoBehaviour 
{

    public List<Level> levels = new List<Level>();
    public CombatUI combatUI;
    public GameObject camera;

    public static GameFlowManager instance = null;
    public AudioManager audioManager;
    public CombatInitializer combatInitializer;
    public CombatConfig combatConfig;
    public DataInitializer dataInitializer;

    int currentLevel;
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

        yield return new WaitForSeconds(1f);
        
        Level level = levels[currentLevel];

        combatConfig.Initialize(dataInitializer.GameSave.Player, level.GetEnemy());

        combatUI.Reset();
        combatUI.DoFadeIn();
        combatInitializer.Initialize();
        loading = false;
    }

    //OnEnemyDefeated function

    //Heal function

    //Weapon selection screen
}
