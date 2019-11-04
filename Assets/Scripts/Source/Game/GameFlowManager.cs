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
public class WeaponSelect
{
    public string Name;
    public int Damage;
    public int Defense;
    public int BonusAttack;
    public int BonusDefense;
    public float GoalSize;
    public float GoalPos;
    public float TimerSpeed;

    public Weapon GetWeapon()
    {
        return new Weapon(
            Name,
            Damage,
            Defense,
            BonusAttack,
            BonusDefense,
            GoalSize,
            GoalPos,
            TimerSpeed
            );
    }
}

public class GameFlowManager : MonoBehaviour 
{

    public List<Level> levels = new List<Level>();
    public List<WeaponSelect> weapons = new List<WeaponSelect>();
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

    [SerializeField]
    TutorialManager tutorialManager;

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
        
        if (combatUI.GetPlayerCombatHandler(dataInitializer.GameSave.Player) == null)
        {
            yield return null;
        }

        currentLevel = dataInitializer.GameSave.EnemyLevel;
        combatConfig.Initialize(dataInitializer.GameSave.Player, levels[currentLevel].GetEnemy());
        combatInitializer.Initialize();

        combatConfig.GetPlayer().EquipWeapon(weapons[dataInitializer.GameSave.WeaponLevel].GetWeapon());
        inBattle = true;
        SceneManager.LoadScene("Intro", LoadSceneMode.Additive);
    }

    public void StartTutorial()
    {
        startFirstAvailableTutorial();
        tutorialManager.OnTutorialComplete(startFirstAvailableTutorial);
    }

    private void startFirstAvailableTutorial()
    {
        List<Tutorial> tuts = tutorialManager.AvailableTutorials;
        if(tuts.Count > 0)
        {
            tutorialManager.TriggerStep(tuts[0]);
        }
    }

    public void ClearSave()
    {
        dataInitializer.SaveManager.Erase();
        dataInitializer.GameSave.Reset();

        combatConfig.Initialize(dataInitializer.GameSave.Player, levels[currentLevel].GetEnemy());
        combatInitializer.Initialize();

        combatConfig.GetPlayer().EquipWeapon(weapons[0].GetWeapon());
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
        float fadeOutTime = combatUI.DoFadeOut();
        float fadeInDelay = combatUI.GetFadeTransitionDelay();
        combatManager.Reset();
        yield return new WaitForSeconds(fadeOutTime + fadeInDelay);
        
        Level level = levels[currentLevel];

        dataInitializer.SaveManager.Save(new GameSave(dataInitializer.GameSave.GetSeed(), 
                                                      combatConfig.GetPlayer(), 
                                                      currentWeapon, 
                                                      currentLevel,
                                                      dataInitializer.GameSave.Tutorials));
        combatConfig.Initialize(dataInitializer.GameSave.Player, level.GetEnemy());
        combatUI.Reset();
        combatUI.DoFadeIn();
        combatInitializer.Initialize();
        loading = false;
        inBattle = true;
        combatConfig.GetPlayer().EquipWeapon(weapons[currentWeapon].GetWeapon());
    }

    public void DoRest()
    {
        combatConfig.GetPlayer().Heal(10);
        //TODO: some healing animation
        combatUI.OnHealthChange(combatConfig.GetPlayer().Health, combatConfig.GetPlayer().MaxHealth);

        NextLevel();
    }

    public void DoUpgrade()
    {
        int nextWeapon = Mathf.Clamp(currentWeapon + 1, 0, weapons.Count - 1);
        currentWeapon = nextWeapon;
        //TODO: some upgrading animation
        
        NextLevel();
    }
}
