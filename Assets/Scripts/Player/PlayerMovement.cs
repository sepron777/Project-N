using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Base setup")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    private float trunSmoothVeleocity;
    public float smoothTime;
    public GameObject visor;
    public Transform raycastDown;
    public Transform raycastFoword;
    RaycastHit hithitDown;
    RaycastHit hithitFoword;
    public Transform Visual;
    public PlayerInput playerInput;
    private InputAction inputAction;
    public bool walting;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    [SerializeField]
    private float cameraYOffset = 0.4f;
    private Camera playerCamera;


    public void Awake()
    {
        playerCamera = Camera.main;
        inputAction = playerInput.actions.FindAction("Move");
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        bool isRunning = false;

        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // We are grounded, so recalculate move direction based on axis
        Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);
        Vector3 right = playerCamera.transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * inputAction.ReadValue<Vector2>().y : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * inputAction.ReadValue<Vector2>().x : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            //moveDirection.y = jumpSpeed;
            walting = MoundCheck();
            transform.position = new Vector3(transform.position.x,hithitDown.point.y, transform.position.z);
            Debug.Log(MoundCheck());
            Debug.Log(hithitDown.collider.name);
            Debug.Log(hithitFoword.collider.name);
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        Vector3 dir = new Vector3(inputAction.ReadValue<Vector2>().x, 0, inputAction.ReadValue<Vector2>().y).normalized;

        if (dir.magnitude >= 0.1f)
        {
            float tar = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(Visual.transform.eulerAngles.y, tar, ref trunSmoothVeleocity, smoothTime);
            Visual.transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private bool MoundCheck()
    {
        bool hitDown = Physics.Raycast(raycastDown.position, raycastDown.up * -1, out hithitDown, 1.5f);
        //nem hit
        bool hitFoword = Physics.Raycast(Visual.transform.position, Visual.transform.forward, out hithitFoword, 1.5f);
        if (hitDown && hitFoword)
        { 
            return true;
        }

        return false;
    }

    public Camera GetCamera()
    {
        return playerCamera;
    }
}
