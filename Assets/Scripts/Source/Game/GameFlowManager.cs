using UnityEngine;
using UnityEngine.SceneManagement;
public class GameFlowManager : MonoBehaviour 
{

    public GameObject combatCanvas;
    public GameObject combatUI;
    public GameObject camera;

    private void Awake()
    {
        combatCanvas.SetActive(false);
        combatUI.SetActive(false);
        camera.SetActive(false);

        SceneManager.LoadScene("Intro", LoadSceneMode.Additive);
    }

    private void Start()
    {
        
    }
}
