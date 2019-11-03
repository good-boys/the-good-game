using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    public void IsNewGame()
    {
        GameFlowManager.instance.ClearSave();
    }

    public void EndIntro()
    {
        GameFlowManager.instance.UnloadScene("Intro");
    }
}
