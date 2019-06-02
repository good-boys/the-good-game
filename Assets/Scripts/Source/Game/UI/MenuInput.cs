using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MenuInput : MonoBehaviour
{
    public GameObject lastSelectedObj;
    public GameObject selectedObject;

    public Color selectedColor;
    public Color unSelectedColor;
    private bool buttonSelected;
    private bool movedMenu;

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (buttonSelected)
        {
            //If user clicks off of currently selected object
            if (EventSystem.current.currentSelectedGameObject == null && selectedObject != EventSystem.current.currentSelectedGameObject && Input.GetAxisRaw("Vertical") == 0 && !Input.GetMouseButtonDown(0))
            {
                EventSystem.current.SetSelectedGameObject(selectedObject);
            }

            //If there is a current button selected and the user inputs vertical
            if ((Input.GetAxisRaw("Vertical") != 0 || Input.GetMouseButtonDown(0)) && EventSystem.current.currentSelectedGameObject != selectedObject && EventSystem.current.currentSelectedGameObject != null)
            {
                lastSelectedObj = selectedObject;

                selectedObject = EventSystem.current.currentSelectedGameObject;

                movedMenu = true;
            }
        }

        //Do something when moving buttons
        if (movedMenu)
        {
            if (lastSelectedObj != null)
            {
                if (lastSelectedObj.GetComponentInChildren<Text>() != null)
                {
                    lastSelectedObj.GetComponentInChildren<Text>().color = unSelectedColor;
                }
            }

            if (selectedObject != null)
            {
                if (selectedObject.GetComponentInChildren<Text>() != null)
                {
                    selectedObject.GetComponentInChildren<Text>().color = selectedColor;
                }
            }
            
            movedMenu = false;
        }

        //If there is no button selected, Get one
        if ((Input.GetAxisRaw("Vertical") != 0 || Input.GetMouseButtonDown(0)) && buttonSelected == false)
        {
            if (selectedObject != null && Input.GetMouseButtonDown(0))
            {
                selectedObject = EventSystem.current.currentSelectedGameObject;
            }

            if (selectedObject == null)
            {
                List<Button> btns = new List<Button>();
                btns.AddRange(FindObjectsOfType<Button>());
                if (btns.Count > 0)
                {
                    EventSystem.current.SetSelectedGameObject(btns[0].gameObject);
                    selectedObject = EventSystem.current.currentSelectedGameObject;
                }
            }

            if (selectedObject != null)
            {
                if (selectedObject.GetComponentInChildren<Text>() != null)
                {
                    selectedObject.GetComponentInChildren<Text>().color = selectedColor;
                }
            }

            buttonSelected = true;
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
