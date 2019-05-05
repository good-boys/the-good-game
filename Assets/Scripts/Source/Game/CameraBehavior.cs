using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    public void EndIntro()
    {
        GameFlowManager.instance.UnloadScene("Intro");
    }
}
