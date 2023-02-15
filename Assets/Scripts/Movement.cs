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

    public float value;
    public float timeBeetweenBuycynes = 0;
    public float myTime = 1;
    public float height;
    public float offset;


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

    private void Update() {
        moveVector = Vector3.zero;
        bool inWater = IsInWater();

        if (inWater) {
            //Octi.transform.position = new Vector3(transform.position.x ,transform.position.y + (Mathf.Cos(Time.time * 4f) + offset) /height, transform.position.z);
            Octi.transform.position = new Vector3(transform.position.x, offset, transform.position.z);
        } else {


            if (characterController.isGrounded == false) {
                moveVector += new Vector3(0, -1);
            } else {
                moveVector.y = 0;
            }
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
        //if (hit.gameObject.layer == 4) {
        //    Debug.Log("collided with water");
        //}
    }

    private void OnTriggerEnter(Collider collider) {
        detectWater(collider);
    }

    private void OnTriggerStay(Collider collider) {
        detectWater(collider);
    }

    private void detectWater(Collider collider) {
        if (collider.gameObject.layer == 4) {
            Vector3 rayStartPosition = Octi.transform.position;
            rayStartPosition.y += 20f;
            Vector3 rayDirection = Vector3.down;
            float rayLength = 100f;
            RaycastHit hit;
            if (Physics.Raycast(rayStartPosition, rayDirection, out hit, rayLength)) {
                // The ray hit something
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
                Debug.Log("Hit point: " + hit.point);
                offset = hit.point.y + 0.5f;
                Debug.DrawLine(Octi.transform.position, hit.point, Color.magenta);
            }
            
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
