using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;
using UnityEngine.AI;
using UnityEngine.XR;
using UnityEngine.Animations.Rigging;

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
    public Transform raycastCorner;
    public Transform raycastForward;
    public Transform Visual;
    public PlayerInput playerInput;
    private InputAction MoveInput;
    private InputAction InteractInput;
    public bool walting;
    public Transform orientacion;
    public Transform mid;

    public Rig ArmsRig;
    [Header("R hand")]
    public Transform RHand;
    public Transform RHandraycast;

    [Header("L hand")]
    public Transform LHand;
    public Transform LHandraycast;

    [HideInInspector]
    public Transform PickUpSpot;

    public LayerMask layerMask;

    [HideInInspector]
    public CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    [SerializeField]
    private float cameraYOffset = 0.4f;
    private Camera playerCamera;

    private bool Rope =false;

    public Inventory inventory;
    [HideInInspector]
    public Animator animator;

    private PlayerState playerState;
    private Movemnt Movement;
    private Climbing Climbing;
    private WallClimbing WallClimbing;


    public void Awake()
    {
        playerCamera = Camera.main;
        inventory = GetComponent<Inventory>();
        PickUpSpot = inventory.PickUpSpot;
        MoveInput = playerInput.actions.FindAction("Move");
        InteractInput = playerInput.actions.FindAction("Interact");
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        Movement = new Movemnt(this.gameObject ,walkingSpeed,  runningSpeed,  jumpSpeed,  gravity,  trunSmoothVeleocity,  smoothTime,  visor,  raycastDown,  raycastFoword,
         Visual, MoveInput,InteractInput,  walting,  characterController,  moveDirection,  rotationX,  canMove,  cameraYOffset,  playerCamera, PickUpSpot, layerMask,animator);

        Climbing = new Climbing(walkingSpeed, runningSpeed, jumpSpeed, gravity, trunSmoothVeleocity, smoothTime, visor, raycastDown, raycastCorner, raycastForward,
         Visual, MoveInput, walting, characterController, moveDirection, rotationX, canMove, cameraYOffset, playerCamera, orientacion,mid, RHand, RHandraycast, LHand, LHandraycast, ArmsRig);

        WallClimbing = new WallClimbing(this.gameObject, walkingSpeed, runningSpeed, jumpSpeed, gravity, trunSmoothVeleocity, smoothTime, visor, raycastDown, raycastFoword,
         Visual, MoveInput,InteractInput, walting, characterController, moveDirection, rotationX, canMove, cameraYOffset, playerCamera, PickUpSpot);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerState = Movement;
    }

    void Update()
    {
        playerState.Update();
        ChenckUpdate(Movement, Climbing);
        ChenckUpdate(Climbing, Movement);
        ChenckUpdate(Movement, WallClimbing, CanClimb());
        ChenckUpdate(WallClimbing, Movement, WallClimbing.CanExit());
       // Debug.Log(playerState);
    }

    private bool CanClimb()
    {
        if (Rope)
        {
            Rope = false;
            return true;
        }
        return false;
    }

    public void SetRope(bool isRope)
    {
        Rope = isRope;
    }

    public Camera GetCamera()
    {
        return playerCamera;
    }

    public PlayerState GetActiveState()
    {
        return playerState;
    }

    public void ChenckUpdate(PlayerState From, PlayerState To)
    {
        if(From != playerState) { return; }
        if (From.CanExit())
        {
            playerState.OnExit();
            playerState = To;
            playerState.OnEnter();
        }
    }

    public void ChenckUpdate(PlayerState From, PlayerState To,bool CanGo)
    {
        if (From != playerState) { return; }
        if (CanGo)
        {
            playerState.OnExit();
            playerState = To;
            playerState.OnEnter();
        }
    }

    public void SetMovement(bool canMove)
    {
        playerState.canMove = canMove;
    }

    public void SetDirectionVisual(Vector3 vector3)
    {
        Movement.dir = vector3;
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

    protected RaycastHit hithitDown;

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

    public virtual void OnExit()
    {

    }
}

public class Movemnt : PlayerState
{
    private bool canExit;
    Transform raycastFoword;
    RaycastHit hithitFoword;
    private InputAction InteractInput;
    private Transform PickUpSpot;
    private GameObject Player;
    private Inventory inventory;
    private LayerMask LayerMask;
    public Vector3 dir;
    private Animator animator;
    private float xspeed;
    private float yspeed;
    private bool crouch;
    public Movemnt(GameObject Player,float walkingSpeed, float runningSpeed, float jumpSpeed, float gravity, float trunSmoothVeleocity, float smoothTime, GameObject visor, Transform raycastDown, Transform raycastFoword,
        Transform Visual, InputAction playerInput, InputAction InteractInput, bool walting, CharacterController characterController, Vector3 moveDirection, float rotationX, bool canMove, float cameraYOffset, Camera playerCamera,Transform PickUpSpot,LayerMask layerMask, Animator animator)
    {
        this.Player = Player;
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
        this.InteractInput = InteractInput;
        this.walting = walting;
        this.characterController = characterController;
        this.moveDirection = moveDirection;
        this.rotationX = rotationX;
        this.canMove = canMove;
        this.cameraYOffset = cameraYOffset;
        this.playerCamera = playerCamera;
        this.PickUpSpot = PickUpSpot;
        this.LayerMask = layerMask;
        this.animator = animator;
        inventory = Player.GetComponent<PlayerMovement>().inventory;
        InteractInput.started += StartedInteract;
    }

    private void StartedInteract(InputAction.CallbackContext context)
    {
        if(!canMove)  return;

        if (inventory.IsInventoryFull())
        {
            inventory.Drop();
            return;
        }
        RaycastHit[] results = new RaycastHit[5];
        Physics.SphereCastNonAlloc(PickUpSpot.position,1,PickUpSpot.up*-1, results,Mathf.Infinity, LayerMask, queryTriggerInteraction: QueryTriggerInteraction.Collide);
        foreach (RaycastHit item in results)
        {
            if(item.collider == null) return;
            if(item.collider.TryGetComponent<IInteractable>(out IInteractable component))
            {
                component.Interact(Player);
                return;
            }
        }
    }

    public override bool CanEnter()
    {
        return false;
    }

    public override void Update()
    {
        //if(!canMove)return;
        bool isRunning = false;

        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);
        xspeed = Mathf.Lerp(xspeed, inputAction.ReadValue<Vector2>().y, 5 * Time.deltaTime);
        yspeed = Mathf.Lerp(yspeed, inputAction.ReadValue<Vector2>().x, 5 * Time.deltaTime);
        // We are grounded, so recalculate move direction based on axis
        Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);
        Vector3 right = playerCamera.transform.TransformDirection(Vector3.right);
        float curSpeedX = canMove ? (isRunning ? (crouch ? walkingSpeed : runningSpeed) : walkingSpeed) * xspeed : 0;
        float curSpeedY = canMove ? (isRunning ? (crouch ? walkingSpeed : runningSpeed) : walkingSpeed) * yspeed : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        if (Input.GetKey(KeyCode.Space) && canMove && characterController.isGrounded)
        {
            canExit = MoundCheck();
            moveDirection.y = jumpSpeed;
            SetAnimationJump(true);
            SetAnimationLand(false);
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
     
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
            SetAnimationFalling(true);
            SetAnimationLand(false);
        }
        else
        {
            if (animator.GetBool("IsFalling"))
            {
                SetAnimationJump(false);
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                crouch = !crouch;
                SetAnimationCrouch(crouch);
                if (crouch)
                {
                    //something here
                }
            }
            SetAnimationFalling(false);
            SetAnimationLand(true);
          
        }

        if (canMove)
        {
            dir = new Vector3(inputAction.ReadValue<Vector2>().x, 0, inputAction.ReadValue<Vector2>().y).normalized;
        }
        float runMultyplier = isRunning ? 2 : 1;
        if (Mathf.Abs(xspeed) >0.05 || Mathf.Abs(yspeed) > 0.05)
        {
            //animator.SetFloat("Value", characterController.velocity.magnitude*0.5f/ walkingSpeed * runMultyplier, 0.3f, Time.deltaTime);
            SetAnimatorMovemnt2D(characterController.velocity.magnitude * 0.5f / walkingSpeed * runMultyplier,0.3f);        
        }
        else
        {
            //animator.SetFloat("Value", 0, 0.05f, Time.deltaTime);
            SetAnimatorMovemnt2D(0, 0.5f);
        }

        if (dir.magnitude >= 0.1f && canMove)
        {
            float tar = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(Visual.transform.eulerAngles.y, tar, ref trunSmoothVeleocity, smoothTime);
            Visual.transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void SetAnimationJump(bool isTrue)
    {
        if (!canMove) return;
        animator.SetBool("IsJumping", isTrue);
    }

    private void SetAnimationLand(bool isTrue)
    {
        if (!canMove) return;
        animator.SetBool("IsLanded", isTrue);
    }

    private void SetAnimationFalling(bool isTrue)
    {
        if (!canMove) return;
        animator.SetBool("IsFalling", isTrue);
    }

    private void SetAnimationCrouch(bool isTrue)
    {
        if (!canMove) return;
        animator.SetBool("IsCrouched", isTrue);
    }
    //este neviem ci to treba
    private void SetAnimatorMovemnt2D(float Value,float DampTime)
    {
        if (!canMove) return;
        animator.SetFloat("Value", Value, DampTime, Time.deltaTime);
    }

    private bool MoundCheck()
    {
        if((inventory.IsInventoryFull()) ||!canMove) { return false; }

        bool hitDown = Physics.Raycast(raycastDown.position, raycastDown.up * -1, out hithitDown, 1.5f);
        if (hitDown)
        {
            raycastFoword.transform.position = new Vector3(raycastFoword.transform.position.x, hithitDown.point.y-0.5f, raycastFoword.transform.position.z);
            Ray ray = new Ray(raycastFoword.transform.position, raycastFoword.transform.forward);
            bool hitFoword = Physics.Raycast(ray, out hithitFoword, 2f);
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
        if (!canMove) return false;
        return canExit;
    }

    private void Teleport()
    {
        characterController.enabled = false;
        characterController.transform.position = new Vector3(characterController.transform.position.x, hithitDown.point.y-0.3f, characterController.transform.position.z);
        characterController.enabled = true;
    }

    public override void OnEnter()
    {
        moveDirection = Vector3.zero;
        canExit = false;
    }

    public override void OnExit()
    {

    }

    public Vector3 GetHeight()
    {
        return hithitDown.point;
    }
}

public class Climbing : PlayerState
{
    private Transform CornerCast;
    RaycastHit hithitCorner;

    private Transform FowordCast;
    RaycastHit hithitFoword;

    Transform orientacion;

    Transform RHand;
    Transform RHandraycast;
    Vector3 RHandPos;
    RaycastHit RightHandhit;
    GameObject RhandPosition;
    private float tRhand = 0.0f;
    private Vector3 startPositionRHand;
    private Vector3 endPositionRHand;
    private bool isMovingRHand = false;
    float RHandDistance = 0.3f;


    Transform LHand;
    Transform LHandraycast;
    Vector3 LHandPos;
    RaycastHit LightHandhit;
    GameObject LhandPosition;
    private float tLhand = 0.0f;
    private Vector3 startPositionLHand;
    private Vector3 endPositionLHand;
    private bool isMovingLHand = false;
    float LHandDistance = 0.3f;

    Rig ArmsRig;

    Vector3 lastFowordNormal;
    Vector3 lastDownNormal;
    GameObject tra;
    Transform mid;
    public Vector3 startPosition;
    public Vector3 controlPoint; // Mid-point to define the curve
    public Vector3 targetPosition;
    public float speed = 1.0f;
    private float timePassed = 0.0f;

    private bool coner =false;

    public Climbing(float walkingSpeed, float runningSpeed, float jumpSpeed, float gravity, float trunSmoothVeleocity, float smoothTime, GameObject visor, Transform raycastDown, Transform CornerCast, Transform FowordCast,
    Transform Visual, InputAction playerInput, bool walting, CharacterController characterController, Vector3 moveDirection, float rotationX, bool canMove, float cameraYOffset, Camera playerCamera, Transform orientacion,Transform mid,Transform RHand, Transform RHandraycast, Transform LHand,Transform LHandraycast, Rig ArmsRig)
    {
        this.walkingSpeed = walkingSpeed;
        this.runningSpeed = runningSpeed;
        this.jumpSpeed = jumpSpeed;
        this.gravity = gravity;
        this.smoothTime = smoothTime;
        this.visor = visor;
        this.FowordCast = FowordCast;
        this.raycastDown = raycastDown;
        this.CornerCast = CornerCast;
        this.Visual = Visual;
        this.inputAction = playerInput;
        this.walting = walting;
        this.characterController = characterController;
        this.moveDirection = moveDirection;
        this.rotationX = rotationX;
        this.canMove = canMove;
        this.cameraYOffset = cameraYOffset;
        this.playerCamera = playerCamera;
        this.orientacion = orientacion;
        this.mid = mid;
        this.RHand = RHand;
        this.RHandraycast = RHandraycast;
        this.LHand = LHand;
        this.LHandraycast = LHandraycast;
        this.ArmsRig = ArmsRig;
        tra = new GameObject();
        RhandPosition = new GameObject();
        RhandPosition.name = "RightHandRealTimePostion";
        LhandPosition = new GameObject();
        LhandPosition.name = "LeftHandRealTimePostion";
        this.walkingSpeed = 3;
    }

    public override bool CanEnter()
    {
        return false;
    }

    public override void Update()
    {
        bool isRunning = false;
        RhandPosition.transform.position = RHand.transform.position;
        Ray rayRightHand = new Ray(RHandraycast.transform.position, RHandraycast.up * -1);
        bool RightHand = Physics.Raycast(rayRightHand, out RightHandhit, 3f);
        // If we detect a hit and not already moving, start the arc motion
        if (RightHand && !isMovingRHand)
        {
            if (Vector3.Distance(RhandPosition.transform.position, RightHandhit.point) > RHandDistance && !isMovingLHand)
            {
                RHand.transform.localEulerAngles = new Vector3(90, -90, -90);
                startPositionRHand = RhandPosition.transform.position;
                endPositionRHand = RightHandhit.point;
                isMovingRHand = true;
                tRhand = 0.0f; // Reset t to start the movement
            }
            else if (Vector3.Distance(RhandPosition.transform.position, RightHandhit.point) < RHandDistance)
            {
                RHand.transform.position = RHandPos;
            }
        }

        // If moving, proceed with the arc motion
        if (isMovingRHand)
        {
            // Increment t based on speed
            float speed = 5f; // Adjust for desired speed
            tRhand += speed * Time.deltaTime;
            tRhand = Mathf.Clamp01(tRhand);

            // Calculate the horizontal position along the straight line
            Vector3 linearPosition = Vector3.Lerp(startPositionRHand, endPositionRHand, tRhand);

            // Calculate a vertical offset using a sine wave for the arc effect
            float arcHeight = 0.2f; // Adjust for desired arc height
            float heightOffset = Mathf.Sin(tRhand * Mathf.PI) * arcHeight;

            // Add the height offset to create the arc
            Vector3 archedPosition = new Vector3(linearPosition.x, linearPosition.y + heightOffset, linearPosition.z);
            RHand.transform.position = archedPosition;
            if (tRhand >= 1.0f)
            {
                tRhand = 0.0f; // Reset for looping or another curve
                RHandPos = RhandPosition.transform.position;
                isMovingRHand = false;
            }

        }

        LhandPosition.transform.position = LHand.transform.position;
        Ray rayLightHand = new Ray(LHandraycast.transform.position, LHandraycast.up * -1);
        bool LightHand = Physics.Raycast(rayLightHand, out LightHandhit, 3f);
        // If we detect a hit and not already moving, start the arc motion
        if (LightHand && !isMovingLHand)
        {
            if (Vector3.Distance(LhandPosition.transform.position, LightHandhit.point) > LHandDistance && !isMovingRHand)
            {
                LHand.transform.localEulerAngles = new Vector3(90, -90, -90);
                startPositionLHand = LhandPosition.transform.position;
                endPositionLHand = LightHandhit.point;
                isMovingLHand = true;
                tLhand = 0.0f; // Reset t to start the movement
            }
            else if (Vector3.Distance(LhandPosition.transform.position, LightHandhit.point) < LHandDistance)
            {
                LHand.transform.position = LHandPos;
            }
        }

        // If moving, proceed with the arc motion
        if (isMovingLHand)
        {
            // Increment t based on speed
            float speed = 5f; // Adjust for desired speed
            tLhand += speed * Time.deltaTime;
            tLhand = Mathf.Clamp01(tLhand);

            // Calculate the horizontal position along the straight line
            Vector3 linearPosition = Vector3.Lerp(startPositionLHand, endPositionLHand, tLhand);

            // Calculate a vertical offset using a sine wave for the arc effect
            float arcHeight = 0.2f; // Adjust for desired arc height
            float heightOffset = Mathf.Sin(tLhand * Mathf.PI) * arcHeight;

            // Add the height offset to create the arc
            Vector3 archedPosition = new Vector3(linearPosition.x, linearPosition.y + heightOffset, linearPosition.z);
            LHand.transform.position = archedPosition;
            if (tLhand >= 1.0f)
            {
                tLhand = 0.0f; // Reset for looping or another curve
                LHandPos = LhandPosition.transform.position;
                isMovingLHand = false;
            }

        }

        if (coner)
        {
            timePassed += Time.deltaTime * speed;

            // Calculate the percentage of progress (from 0 to 1)
            float t = Mathf.Clamp01(timePassed);

            // Calculate the Bezier curve position
            Vector3 bezierPosition = Mathf.Pow(1 - t, 2) * startPosition
                                     + 2 * (1 - t) * t * controlPoint
                                     + Mathf.Pow(t, 2) * targetPosition;

            // Move the character along the curve
            characterController.transform.position = bezierPosition;
            Visual.transform.forward =  Vector3.Lerp(Visual.transform.forward, hithitCorner.normal * -1, 0.05f);
            
            if (t >= 1.0f)
            {
                timePassed = 0.0f; // Reset for looping or another curve
                characterController.enabled = true;
                coner = false;
            }
            return;
        }

        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // We are grounded, so recalculate move direction based on axis
        Vector3 forward = orientacion.transform.TransformDirection(Vector3.forward);
        Vector3 right = orientacion.transform.TransformDirection(Vector3.right);

        float curSpeedX = 0;
        float curSpeedY = canMove ? walkingSpeed * inputAction.ReadValue<Vector2>().x : 0;
        curSpeedY = isMovingRHand || isMovingLHand ? 0 : curSpeedY;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);


        Ray ray3 = new Ray(raycastDown.transform.position, raycastDown.up * -1);
        bool hitDown = Physics.Raycast(ray3, out hithitDown, 3f);
        CornerCast.localEulerAngles = new Vector3(0, -55 * inputAction.ReadValue<Vector2>().x, Vector2.Angle(characterController.transform.up, hithitDown.normal));
        CornerCast.localPosition = new Vector3(0.45f* inputAction.ReadValue<Vector2>().x, CornerCast.localPosition.y, CornerCast.localPosition.z);
        FowordCast.localEulerAngles = new Vector3(0, 55 * inputAction.ReadValue<Vector2>().x, Vector2.Angle(characterController.transform.up, hithitDown.normal));
        Ray ray = new Ray(CornerCast.transform.position, CornerCast.forward);
        bool hitcorner = Physics.Raycast(ray, out hithitCorner, 1f);
        Ray ray2 = new Ray(FowordCast.transform.position, FowordCast.forward);
        bool hitFoword = Physics.Raycast(ray2, out hithitFoword, 0.4f);

        if (hitFoword)
        {
            if (hithitFoword.normal != lastFowordNormal)
            {
                Visual.transform.forward = hithitFoword.normal * -1;
                tra.transform.forward = hithitFoword.normal * -1;
                orientacion.eulerAngles = new Vector3(0, tra.transform.eulerAngles.y, -Vector2.Angle(characterController.transform.up, hithitDown.normal));
                lastFowordNormal = hithitFoword.normal;
            }
        }
        else if (hitcorner)
        {
            if (hithitCorner.normal != lastFowordNormal)
            {
                SaveTransform();
                //Visual.transform.forward = hithitCorner.normal * -1;
                Debug.Log(hithitCorner.normal);
                tra.transform.forward = hithitCorner.normal * -1;
                characterController.enabled = false;
                coner = true;
                orientacion.eulerAngles = new Vector3(0, tra.transform.eulerAngles.y, Vector2.Angle(characterController.transform.up, hithitDown.normal));
                lastFowordNormal = hithitCorner.normal;
            }
        }

        if (hitDown)
        {
            if (hithitDown.normal != lastDownNormal)
            {
                orientacion.localEulerAngles = new Vector3(0, tra.transform.eulerAngles.y, Vector2.Angle(characterController.transform.up, hithitDown.normal)* Yaxis());
                lastDownNormal = hithitDown.normal;
                Teleport();
            }
           // Debug.Log(Vector2.Angle(characterController.transform.up, hithitDown.normal));
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
    private void Teleport()
    {
        characterController.enabled = false;
        characterController.transform.position = new Vector3(characterController.transform.position.x, hithitDown.point.y - 0.3f, characterController.transform.position.z);
        characterController.enabled = true;
    }

    public void SaveTransform()
    {
        if(!coner)
        {
            startPosition = characterController.transform.position;
            mid.localPosition = new Vector3( - 0.82f * -inputAction.ReadValue<Vector2>().x, mid.localPosition.y, mid.localPosition.z);
            controlPoint =mid.position;
             Vector3 point = new Vector3(hithitCorner.point.x + hithitCorner.normal.z * 0.4f * -inputAction.ReadValue<Vector2>().x, characterController.transform.position.y, hithitCorner.point.z + hithitCorner.normal.x * 0.4f * inputAction.ReadValue<Vector2>().x);
            targetPosition = point + hithitCorner.normal*0.3f;
        }
    }
       

    private float Yaxis()
    {
        float y = orientacion.eulerAngles.y; 
        Debug.Log(y);
        if (y < 1 ||y>180)
        {
            return -1;
        }
        return 1;
    }

    public override bool CanExit()
    {
        if (!canMove && coner) return false;
        if (inputAction.ReadValue<Vector2>().y > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            Ray ray = new Ray(raycastDown.transform.position, raycastDown.transform.up * -1);
            bool down = Physics.Raycast(ray, out hithitDown, 3f);
            if (down)
            {
                characterController.enabled = false;
                characterController.transform.position = hithitDown.point+ new Vector3(0,3,0);
                characterController.enabled = true;
                return true;
            }
        }
        else if (inputAction.ReadValue<Vector2>().y < 0 && Input.GetKeyDown(KeyCode.Space))
        {
            Ray ray = new Ray(characterController.transform.position, characterController.transform.up*-1);
            bool down = Physics.Raycast(ray,out hithitDown,3f);
            if(down)
            {
                characterController.enabled = false;
                characterController.transform.position = hithitDown.point;
                characterController.enabled = true;
                return true;
            }
        }
        return false;
    }

    public override void OnEnter()
    {
        ArmsRig.weight = 1.0f;
        Physics.Raycast(raycastDown.position, raycastDown.up * -1, out hithitDown, 3f);
        this.CornerCast.position = new Vector3(CornerCast.position.x, hithitDown.point.y-0.2f, CornerCast.position.z);
        this.FowordCast.position = new Vector3(FowordCast.position.x, hithitDown.point.y-0.2f, FowordCast.position.z);
        Ray rayRightHant = new Ray(RHandraycast.transform.position, RHandraycast.up * -1);
        bool RightHand = Physics.Raycast(rayRightHant, out RightHandhit, 3f);
        if(RightHand) RHandPos = RightHandhit.point;
        Ray rayLightHant = new Ray(LHandraycast.transform.position, LHandraycast.up * -1);
        bool LightHand = Physics.Raycast(rayLightHant, out LightHandhit, 3f);
        if (LightHand) LHandPos = LightHandhit.point;
        lastFowordNormal = Vector3.zero;
        lastDownNormal = Vector3.zero;
    }


    public override void OnExit()
    {
        ArmsRig.weight = 0f;
    }
}

public class WallClimbing : PlayerState
{
    private bool canExit;
    Transform raycastFoword;
    RaycastHit hithitFoword;
    private Transform PickUpSpot;
    private InputAction Interact;
    private GameObject Player;
    private Inventory inventory;
    private bool up = false;
    public WallClimbing(GameObject Player, float walkingSpeed, float runningSpeed, float jumpSpeed, float gravity, float trunSmoothVeleocity, float smoothTime, GameObject visor, Transform raycastDown, Transform raycastFoword,
        Transform Visual, InputAction playerInput,InputAction Interact, bool walting, CharacterController characterController, Vector3 moveDirection, float rotationX, bool canMove, float cameraYOffset, Camera playerCamera, Transform PickUpSpot)
    {
        this.Player = Player;
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
        this.Interact = Interact;
        this.walting = walting;
        this.characterController = characterController;
        this.moveDirection = moveDirection;
        this.rotationX = rotationX;
        this.canMove = canMove;
        this.cameraYOffset = cameraYOffset;
        this.playerCamera = playerCamera;
        this.PickUpSpot = PickUpSpot;
        inventory = Player.GetComponent<PlayerMovement>().inventory;
    }


    public override bool CanEnter()
    {
        return false;
    }

    public override void Update()
    {
        bool isRunning = false;

        RaycastHit hit;
        Physics.Raycast(Visual.transform.position, Visual.transform.forward, out hit, 1f);
        Visual.transform.forward = hit.normal * -1;

        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // We are grounded, so recalculate move direction based on axis
        Vector3 forward = playerCamera.transform.TransformDirection(Vector3.up);
        Vector3 right = playerCamera.transform.TransformDirection(Vector3.right);
        float curSpeedX =0;
        float curSpeedY = 0;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        moveDirection.y = canMove ? (isRunning ? runningSpeed : walkingSpeed) * inputAction.ReadValue<Vector2>().y : 0;
        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }



    public override bool CanExit()
    {
        
        if (!canMove) return false;
        up = !Physics.Raycast(raycastFoword.position, raycastFoword.forward, 2f);
        if (up)
        {
            return true;
        }
        if (Physics.Raycast(Visual.position, Visual.up * -1, 1.3f)) 
        {
            up = false;
            return true;
        }
        return false;
    }


    public override void OnEnter()
    {
        moveDirection = Vector3.zero;
        ClimbingRope climbingRope = inventory.GetHoldingItem().GetComponent<ClimbingRope>();
        if (Vector3.Distance(Visual.transform.position, climbingRope.top.position)< Vector3.Distance(Visual.transform.position, climbingRope.bottom.position)) 
        {
            Teleport(climbingRope.top.position);
        }
        else
        {
            Teleport(climbingRope.bottom.position);
        }
        
        RaycastHit hit;
        Vector3 vc = (inventory.GetHoldingItem().transform.position - characterController.transform.position).normalized;
        Physics.Raycast(Visual.transform.position, vc , out hit, 1f);
        Visual.transform.forward = hit.normal * -1;
        raycastFoword.localPosition = new Vector3(raycastFoword.localPosition.x, 1,raycastFoword.localPosition.z);
    }

    public override void OnExit()
    {
        Physics.Raycast(raycastDown.transform.position, raycastDown.up * -1, out hithitDown, 3f);
        inventory.SetItem(null,true);
        if(up) Teleport(new Vector3(hithitDown.point.x, hithitDown.point.y + 1f, hithitDown.point.z));
    }

    private void Teleport(Vector3 newPosition)
    {
        characterController.enabled = false;
        characterController.transform.position = newPosition;
        characterController.enabled = true;
    }
}
