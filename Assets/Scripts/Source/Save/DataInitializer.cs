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
    Tutorial combatTutorial;

    public void Init(string saveFile, int seed, CharacterConfig startingPlayer, SaveManager saveManager)
    {
        this.saveFile = saveFile;
        this.seed = seed;
        this.startingPlayer = startingPlayer;
        SaveManager = saveManager;
    }

    public void Awake()
    {
        SetUp();
    }

    public void SetUp()
    {
        if (SaveManager == null)
        {
            SaveManager = new SaveManager(getSavePath());
        }
        if (SaveManager.HasSave())
        {
            GameSave = SaveManager.Load();
        }
        else
        {
            GameSave = new GameSave(seed, new Player(startingPlayer), combatTutorial);
        }
    }

    public bool IsInitialized()
    {
        return GameSave != null;
    }

    public void SaveCurrent()
    {
        if(SaveManager == null || GameSave == null)
        {
            Debug.LogError("Save data not initialized. Unable to save");
            return;
        }

        SaveManager.Save(GameSave);
    }

    string getSavePath()
    {
        return Path.Combine(Application.persistentDataPath, saveFile);
    }
}
