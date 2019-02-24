using UnityEngine;
using UnityEngine.SceneManagement;
public class GameFlowManager : MonoBehaviour 
{

    public GameObject combatCanvas;
    public GameObject combatUI;
    public GameObject camera;

    public static GameFlowManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        combatCanvas.SetActive(false);
        combatUI.SetActive(false);
        camera.SetActive(false);

        SceneManager.LoadScene("Intro", LoadSceneMode.Additive);
    }

    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);

        if (sceneName == "Intro")
        {
            combatCanvas.SetActive(true);
            combatUI.SetActive(true);
            camera.SetActive(true);
        }
    }
}
