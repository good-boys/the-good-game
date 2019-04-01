using UnityEngine;

public class 
TestDataInitializer : MonoBehaviour
{
    [SerializeField]
    DataInitializer dataInitializer;

    SaveManager manager;
    GameSave save;

    void Start()
    {
        Debug.Log("Start data initializer");
        save = dataInitializer.GameSave;
        manager = dataInitializer.SaveManager;
        Debug.Log(save.Player);
        Debug.Log(save.Random.Next());
        Debug.Log(save.Random.Next());
        Debug.Log(save.Random.Next());
        save.Reset();
        manager.Save(save);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            manager.Erase();
        }
    }
}
