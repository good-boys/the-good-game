using System.IO;
using UnityEngine;

public class DataInitializer : MonoBehaviour
{
    public GameSave GameSave { get; private set; }
    public SaveManager SaveManager { get; private set; }

    [SerializeField]
    string saveFile = "save.dat";

    [SerializeField]
    int seed = 0;

    [SerializeField]
    CharacterConfig startingPlayer;

    [SerializeField]
    int currentWeapon = 0;

    [SerializeField]
    int currentLevel = 0;

    public void Init(string saveFile, int seed, CharacterConfig startingPlayer, SaveManager saveManager, int currentWeapon, int currentLevel)
    {
        this.saveFile = saveFile;
        this.seed = seed;
        this.startingPlayer = startingPlayer;
        this.currentWeapon = currentWeapon;
        this.currentLevel = currentLevel;
        SaveManager = saveManager;
    }

    public void Awake()
    {
        if(SaveManager == null)
        {
            SaveManager = new SaveManager(getSavePath());
        }
        if(SaveManager.HasSave())
        {
            GameSave = SaveManager.Load();
        }
        else
        {
            GameSave = new GameSave(seed, new Player(startingPlayer), 0, 0);
            GameSave.Player.EquipWeapon(new Weapon("Default", 5, 5, 2, 2, .2f, -145f, 5f));
        }
    }

    public bool IsInitialized()
    {
        return GameSave != null;
    }

    string getSavePath()
    {
        return Path.Combine(Application.persistentDataPath, saveFile);
    }
}
