using UnityEngine;
using UnityEngine.SceneManagement;
public class GameFlowManager : MonoBehaviour 
{
    
    public CombatUI combatUI;
    public GameObject camera;

    public static GameFlowManager instance = null;
    public AudioManager audioManager;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        
        combatUI.gameObject.SetActive(false);
        camera.SetActive(false);

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

        audioManager.Stop("Trees", null, 3);
        audioManager.Play("Music 1", null, new AudioOptions(3,0,0.5f,true,false));
    }
}
