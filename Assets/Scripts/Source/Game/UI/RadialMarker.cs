using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMarker : MonoBehaviour {

    [SerializeField]
    Transform markerParent;

    [SerializeField]
    Image radialFill;

    [SerializeField]
    Image goalFill;

    public void Spin(float goalSize, float goalPos, float speed)
    {
        radialFill.gameObject.SetActive(true);
        StartCoroutine(SpinRoutine(goalSize, goalPos, speed));
    }

    IEnumerator SpinRoutine(float goalSize, float goalPos, float speed)
    {
        float timer = 0f;

        goalFill.fillAmount = goalSize;


        Vector3 goalTarget = Vector3.zero;
        goalTarget.z = goalPos;
        goalFill.transform.localEulerAngles = goalTarget;

        Vector3 targetRot = Vector3.zero;
        while (timer < speed)
        {
            timer += Time.deltaTime;

            radialFill.fillAmount = timer / speed;
            targetRot.z = (timer / speed) * -360f;
            markerParent.transform.localEulerAngles = targetRot;
            yield return null;
        }

        radialFill.gameObject.SetActive(false);
    }
}
