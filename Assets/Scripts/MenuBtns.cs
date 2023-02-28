using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuBtns : MonoBehaviour {
    [SerializeField] private GameObject menu;
    private bool menuActive = false;
    [SerializeField] private string website;

    public void OpenMenu(InputAction.CallbackContext context) {
        if (context.performed) {
            if (menuActive) menuActive = false; 
            else menuActive = true;

            menu.SetActive(menuActive);
        }
    }

    public void Exit() {
        Application.Quit();
    }

    public void OpenWebsite() {
        Application.OpenURL(website);
    }
}
