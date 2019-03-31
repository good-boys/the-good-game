using UnityEngine;

public class TestDataInitializer : MonoBehaviour
{
    [SerializeField]
    DataInitializer dataInitializer;

    void Start()
    {
        GameSave save = dataInitializer.GameSave;
        SaveManager manager = dataInitializer.SaveManager;
        Debug.Log(save.Random.Next());
        manager.Save(save);
    }
}
