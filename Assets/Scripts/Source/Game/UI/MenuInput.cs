using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MenuInput : MonoBehaviour {
    public EventSystem eventSystem;
    public GameObject lastSelectedObj;
    public GameObject selectedObject;

    public Color selectedColor;
    public Color unSelectedColor;
    private bool buttonSelected;
    private bool movedMenu;

    // Update is called once per frame
    void Update()
    {
        if (buttonSelected)
        {
            //If user clicks off of currently selected object
            if (eventSystem.currentSelectedGameObject == null && selectedObject != eventSystem.currentSelectedGameObject && Input.GetAxisRaw("Vertical") == 0 && !Input.GetMouseButtonDown(0))
            {
                eventSystem.SetSelectedGameObject(selectedObject);
            }

            //If there is a current button selected and the user inputs vertical
            if ((Input.GetAxisRaw("Vertical") != 0 || Input.GetMouseButtonDown(0)) && eventSystem.currentSelectedGameObject != selectedObject && eventSystem.currentSelectedGameObject != null)
            {
                lastSelectedObj = selectedObject;

                selectedObject = eventSystem.currentSelectedGameObject;

                movedMenu = true;
            }
        }

        //Do something when moving buttons
        if (movedMenu)
        {
            if (lastSelectedObj.GetComponentInChildren<Text>() != null)
            {
                lastSelectedObj.GetComponentInChildren<Text>().color = unSelectedColor;
            }

            if (selectedObject.GetComponentInChildren<Text>() != null)
            {
                selectedObject.GetComponentInChildren<Text>().color = selectedColor;
            }
            
            movedMenu = false;
        }

        //If there is no button selected, Get one
        if ((Input.GetAxisRaw("Vertical") != 0 || Input.GetMouseButtonDown(0)) && buttonSelected == false)
        {
            if (selectedObject != null && Input.GetMouseButtonDown(0))
            {
                selectedObject = eventSystem.currentSelectedGameObject;
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
