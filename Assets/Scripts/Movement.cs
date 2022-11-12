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
    }

    private void Update()
    {
        if (movementVector != new Vector2(0, 0)) 
        {
            characterController.Move(new Vector3(movementVector.x, 0, movementVector.y) * Time.deltaTime * 10);
        }
    }
}
