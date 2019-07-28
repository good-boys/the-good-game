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
    Tutorial attackTutorial, defendTutorial, comboTutorial;

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
        bool unableToLoad = false;
        if (SaveManager == null)
        {
            SaveManager = new SaveManager(getSavePath());
        }
        if(SaveManager.HasSave())
        {
            try
            {
                GameSave = SaveManager.Load();
            }
            catch(System.Exception e)
            {
                Debug.LogError(e);
                unableToLoad = true;
            }
        }
        else
        {
            unableToLoad = true;
        }

        if(unableToLoad)
        {
            attackTutorial.SetName(Tutorial.ATTACK_TUTORIAL);
            defendTutorial.SetName(Tutorial.DEFEND_TUTORIAL);
            comboTutorial.SetName(Tutorial.COMBO_TUTORIAL);
            Tutorial[] newTutorials = { attackTutorial, defendTutorial, comboTutorial };
            GameSave = new GameSave(seed, new Player(startingPlayer), newTutorials);
            GameSave.Player.EquipWeapon(new Weapon("Default", 5, 5, 2, 2, .2f, -145f, 5f));
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
