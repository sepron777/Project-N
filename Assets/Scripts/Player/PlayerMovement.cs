using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
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

    private PlayerState playerState;
    private Movemnt Movement;
    private Climbing Climbing;


    public void Awake()
    {
        playerCamera = Camera.main;
        inputAction = playerInput.actions.FindAction("Move");
        characterController = GetComponent<CharacterController>();
        Movement = new Movemnt( walkingSpeed,  runningSpeed,  jumpSpeed,  gravity,  trunSmoothVeleocity,  smoothTime,  visor,  raycastDown,  raycastFoword,
         Visual, inputAction,  walting,  characterController,  moveDirection,  rotationX,  canMove,  cameraYOffset,  playerCamera);

        Climbing = new Climbing(walkingSpeed, runningSpeed, jumpSpeed, gravity, trunSmoothVeleocity, smoothTime, visor, raycastDown, raycastFoword,
         Visual, inputAction, walting, characterController, moveDirection, rotationX, canMove, cameraYOffset, playerCamera);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerState = Movement;
    }

    void Update()
    {
        playerState.Update();
        if (playerState.CanExit())
        {
           if(playerState == Movement)
           {
                playerState = Climbing;
                playerState.OnEnter();
           }
           else
           {
                playerState = Movement;
                playerState.OnEnter();
            }
        }
        /*
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
        Debug.Log(characterController.isGrounded);
        if (Input.GetKeyDown(KeyCode.Space) && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
            walting = MoundCheck();
            Teleport();
            Debug.Log(MoundCheck());
            Debug.Log(hithitDown.collider.name);
        }
        else
        {
            if (!walting)
            {
                moveDirection.y = movementDirectionY;
            }

        }
        
        if (!characterController.isGrounded && !walting)
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
        */
    }

    public Camera GetCamera()
    {
        return playerCamera;
    }

    public PlayerState GetActiveState()
    {
        return playerState;
    }
}

public class PlayerState
{
    public float walkingSpeed;
    public float runningSpeed;
    public float jumpSpeed;
    public float gravity;
    protected float trunSmoothVeleocity;
    public float smoothTime;
    public GameObject visor;
    public Transform raycastDown;
    public Transform raycastFoword;
    protected RaycastHit hithitDown;
    protected RaycastHit hithitFoword;
    public Transform Visual;
    public PlayerInput playerInput;
    protected InputAction inputAction;
    public bool walting;

    protected CharacterController characterController;
    protected Vector3 moveDirection;
    protected float rotationX;

    [HideInInspector]
    public bool canMove;

    [SerializeField]
    protected float cameraYOffset;
    protected Camera playerCamera;

    public virtual bool CanEnter()
    {
        return false;
    }

    public virtual void Update()
    {

    }

    public virtual bool CanExit()
    {
        return false;
    }

    public virtual void OnEnter()
    {

    }
}

public class Movemnt : PlayerState
{
    private bool canExit;
    public Movemnt(float walkingSpeed, float runningSpeed , float jumpSpeed , float gravity , float trunSmoothVeleocity, float smoothTime, GameObject visor, Transform raycastDown, Transform raycastFoword,
        Transform Visual, InputAction playerInput, bool walting, CharacterController characterController, Vector3 moveDirection, float rotationX, bool canMove, float cameraYOffset, Camera playerCamera)
    {
        this.walkingSpeed = walkingSpeed;
        this.runningSpeed = runningSpeed;
        this.jumpSpeed = jumpSpeed;
        this.gravity = gravity;
        this.smoothTime = smoothTime;
        this.visor = visor;
        this.raycastDown = raycastDown;
        this.raycastFoword = raycastFoword;
        this.Visual = Visual;
        this.inputAction = playerInput;
        this.walting = walting;
        this.characterController = characterController;
        this.moveDirection = moveDirection;
        this.rotationX = rotationX;
        this.canMove = canMove;
        this.cameraYOffset = cameraYOffset;
        this.playerCamera = playerCamera;
    }

    public override bool CanEnter()
    {
        return false;
    }

    public override void Update()
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
        if (Input.GetKeyDown(KeyCode.Space) && canMove && characterController.isGrounded)
        {
            canExit = MoundCheck();
            moveDirection.y = jumpSpeed;
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
       
        if (hitDown)
        {
            raycastFoword.transform.position = new Vector3(raycastFoword.transform.position.x, hithitDown.point.y, raycastFoword.transform.position.z);
            Ray ray = new Ray(Visual.transform.position, Visual.transform.forward);
            bool hitFoword = Physics.Raycast(ray, out hithitFoword, 1.5f);
            if (hitFoword)
            {
                Teleport();
                return true;
            }

        }

        return false;
    }

    public override bool CanExit()
    {
        return canExit;
    }

    private void Teleport()
    {
        characterController.enabled = false;
        characterController.transform.position = new Vector3(characterController.transform.position.x, hithitDown.point.y, characterController.transform.position.z);
        characterController.enabled = true;
    }

    public override void OnEnter()
    {
        Debug.Log("ide");
        moveDirection = Vector3.zero;
        canExit = false;
    }
}

public class Climbing: PlayerState
{
    public Climbing(float walkingSpeed, float runningSpeed, float jumpSpeed, float gravity, float trunSmoothVeleocity, float smoothTime, GameObject visor, Transform raycastDown, Transform raycastFoword,
    Transform Visual, InputAction playerInput, bool walting, CharacterController characterController, Vector3 moveDirection, float rotationX, bool canMove, float cameraYOffset, Camera playerCamera)
    {
        this.walkingSpeed = walkingSpeed;
        this.runningSpeed = runningSpeed;
        this.jumpSpeed = jumpSpeed;
        this.gravity = gravity;
        this.smoothTime = smoothTime;
        this.visor = visor;
        this.raycastDown = raycastDown;
        this.raycastFoword = raycastFoword;
        this.Visual = Visual;
        this.inputAction = playerInput;
        this.walting = walting;
        this.characterController = characterController;
        this.moveDirection = moveDirection;
        this.rotationX = rotationX;
        this.canMove = canMove;
        this.cameraYOffset = cameraYOffset;
        this.playerCamera = playerCamera;
    }

    public override bool CanEnter()
    {
        return false;
    }

    public override void Update()
    {
        bool isRunning = false;

        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // We are grounded, so recalculate move direction based on axis
        Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);
        Vector3 right = playerCamera.transform.TransformDirection(Vector3.right);

        float curSpeedX = 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * inputAction.ReadValue<Vector2>().x : 0;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    public override bool CanExit()
    {
        bool ground = Physics.Raycast(raycastDown.transform.position, raycastDown.transform.up*-1, out hithitDown,2.5f);
        Debug.DrawRay(raycastDown.transform.position, raycastDown.transform.up * -2.5f,Color.red);
        return !ground;
    }

    public override void OnEnter()
    {
        Debug.Log("ide");
    }
}
