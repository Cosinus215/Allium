using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private CharacterController characterController;
    private Vector2 movementVector;
    private Vector3 moveVector;
    private Vector3 startPosition;
    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private float deadZone = 0.25f;
    [SerializeField] private GameObject directionPoint;
    [SerializeField] private GameObject Octi;
    [SerializeField] private GameObject joystick;

    private Quaternion rotationLeft    = new Quaternion(0, 0, 0, 0);
    private Quaternion rotationRight   = new Quaternion(0, 180, 0, 0);

    private Vector3 directionUp        = new Vector3(0, 0, 1.5f);
    private Vector3 directionDown      = new Vector3(0, 0, -1.5f);
    private Vector3 directionLeft      = new Vector3(-1.5f, 0, 0);
    private Vector3 directionRight     = new Vector3(1.5f, 0, 0);

    [SerializeField] private PlayerInput input;

    void Start()
    {
        startPosition = transform.position;

        if (Application.platform == RuntimePlatform.Android) {
            input.neverAutoSwitchControlSchemes = true;
            input.SwitchCurrentControlScheme(Gamepad.current);
            joystick.SetActive(true);
        } else {
            input.neverAutoSwitchControlSchemes = false;
            joystick.SetActive(false);
        }
        characterController = GetComponent<CharacterController>();
    }

    public void PlayerMovement(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
        Player_Rotation();
        Player_LookDirection();
    }

    private void Update()
    {
        moveVector = Vector3.zero;

        if (!IsInWater()) {
            if (characterController.isGrounded == false) {
                moveVector += Physics.gravity;
            } else {
                moveVector.y = 0;
            }
        } else {
            Physics.gravity = new Vector3(0, -1);
            moveVector.y = Mathf.MoveTowards(transform.position.y, transform.position.y, 3 * Time.deltaTime);
        }
        characterController.Move(new Vector3(movementVector.x, moveVector.y, movementVector.y) * Time.deltaTime * movementSpeed);

    }

    private bool IsInWater() {
        return Physics.CheckSphere(transform.position, 0.5f, LayerMask.GetMask("Water"));
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.tag == "Teleport") {
            transform.position = startPosition;
        }
    }

    private void Player_Rotation()
    {
        if (movementVector.x > deadZone)
        {
            Octi.transform.localRotation = rotationRight;
        }
        else if (movementVector.x < -deadZone)
        {
            Octi.transform.localRotation = rotationLeft;
        }
    }

    private void Player_LookDirection()
    {
        if (movementVector.x > deadZone)
        {
            directionPoint.transform.localPosition = directionRight;
        }
        else if (movementVector.x < -deadZone)
        {
            directionPoint.transform.localPosition = directionLeft;
        }
        else if (movementVector.y > deadZone)
        {
            directionPoint.transform.localPosition = directionUp;
        }
        else if (movementVector.y < -deadZone)
        {
            directionPoint.transform.localPosition = directionDown;
        }
    }



}
