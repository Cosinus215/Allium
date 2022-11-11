using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private CharacterController characterController;
    public Vector2 movementVector;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    public void PlayerMovement(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
        if (context.started)
        {
            Debug.Log("KLIK!");
        }
        if (context.canceled)
        {
            Debug.Log("canceled!");
        }
    }
}
