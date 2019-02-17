using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonPressed : MonoBehaviour {

    Button btn;
    Text btnText;

    UnityEvent actions;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        btnText = GetComponentInChildren<Text>();

        if (btn != null)
        {
            actions = btn.onClick;
            btn.onClick = new Button.ButtonClickedEvent();
            btn.onClick.AddListener(Flicker);
        }
	}

    void Flicker()
    {
        StartCoroutine(FlickerAnim());
    }
	
    IEnumerator FlickerAnim()
    {
        float flickerTime = 0;
        bool active = false;
        Color color = btn.image.color;

        //Play SFX Here
        yield return new WaitForSeconds(0.2f);
        while (flickerTime < 1)
        {
            flickerTime += 0.2f;

            if (active)
            {
                color.a = 0;
            }
            else
            {
                color.a = 1;
            }

            btn.image.color = color;
            if (btnText != null)
            {
                btnText.color = color;
            }
            active = !active;

            yield return new WaitForSeconds(0.2f);
        }

        active = true;
        color.a = 1;

        btn.image.color = color;
        if (btnText != null)
        {
            btnText.color = color;
        }

        actions.Invoke();
    }
}
