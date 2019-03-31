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


    void Awake()
    {
        SaveManager = new SaveManager(getSavePath());
        if(SaveManager.HasSave())
        {
            GameSave = SaveManager.Load();
        }
        else
        {
            GameSave = new GameSave(seed);
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
