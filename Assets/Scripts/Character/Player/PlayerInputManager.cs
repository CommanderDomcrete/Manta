using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerManager player;
    PlayerControls playerControls;
    PlayerCamera playerCamera;

    [Header("MOVEMENT INPUT")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("CAMERA INPUT")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    [Header("PLAYER ACTION INPUT")]
    [SerializeField] bool quickBoostInput = false;
    public bool boost_input = false;
    public bool flight_inputPrimer = false;
    public bool flight_input = false;
    private float tapWaitTime = 0.6f;
    private bool fireRight_input = false;
    private bool fireLeft_input = false;
    public bool switchlockOn_input;

    public bool lockOnFlag;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        //WHEN THE SCENE CHANGES, RUN THIS LOGIC
        SceneManager.activeSceneChanged += OnSceneChange;

        instance.enabled = false;
        //player.OnDeath += OnPlayerDeath;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        //IF WE ARE LOADING INTO OUT WORLD SCENE, ENABLE OUR PLAYER CONTROLS
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
        }
        // OTHERWISE WE MUST BE AT THE MAIN MENU, DISABLE PLAYER CONTROLS
        //THIS IS SO OUR PLAYER CANNOT MOVE AROUND IF WE ENTER THINGS LIKE MECH CUSTOMISATION
        else
        {
            instance.enabled = false;
        }
        player.OnDeath += OnPlayerDeath;
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            // Whenever this action is performed, take the values from the performed action (assigned 'i') and give them to our Vector2 variable (assigned 'movementInput').        
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.QuickBoost.performed += i => quickBoostInput = true;

            playerControls.PlayerActions.Boost.performed += i => boost_input = true;
            playerControls.PlayerActions.Boost.canceled += i => boost_input = false;

            playerControls.PlayerActions.FlightPrimer.performed += i => flight_inputPrimer = true;
            playerControls.PlayerActions.Flight.performed += i => flight_input = true;
            playerControls.PlayerActions.Flight.canceled += i => flight_input = false;

            playerControls.PlayerActions.FireRightWeapon.performed += i => fireRight_input = true;
            playerControls.PlayerActions.FireRightWeapon.canceled += i => fireRight_input = false;

            playerControls.PlayerActions.FireLeftWeapon.performed += i => fireLeft_input = true;
            playerControls.PlayerActions.FireLeftWeapon.canceled += i => fireLeft_input = false;

            playerControls.PlayerActions.SwitchLockOn.performed += i => switchlockOn_input = true;

        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        //IF WE DESTROY THIS OBJECT, UNSUBSCRIBE FROM THIS EVENT
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    public void OnPlayerDeath()
    {
        instance.enabled = false;

        cameraInput = Vector2.zero;
        cameraVerticalInput = 0;
        cameraHorizontalInput = 0;

        movementInput = Vector2.zero;
        verticalInput = 0;
        horizontalInput = 0;
        moveAmount = 0;

        quickBoostInput = false;
        boost_input = false;
        player.playerLocomotionManager.isBoosting = false;
        flight_input = false;
        fireRight_input = false;
        fireLeft_input = false;
        
    }

    private void Update()
    {
        HandleAllInputs();
    }
    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleQuickBoostInput();
        HandleBoostInput();
        HandleFlightInput();
        HandleFireInput();
        HandleSwitchLockOnInput();
    }

    // MOVEMENT

    private void HandlePlayerMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        //RETURNS THE ABSOLUTE NUMBER, (meaning number without the negative sign so it is always positive)
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        if(player == null)
        {
            return;
        }

        player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.playerLocomotionManager.isBoosting);
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;

    }

    // ACTION
    private void HandleQuickBoostInput()
    {
        if (quickBoostInput)
        {
            quickBoostInput = false;
            player.playerLocomotionManager.AttemptQuickBoost();
            
        }
        
    }
    private void HandleBoostInput()
    {
        if (boost_input && moveAmount > 0.5f)
        {
            player.playerLocomotionManager.isBoosting = true;
        }
        else
        {
            player.playerLocomotionManager.isBoosting = false;
        }
    }

    private void HandleFlightInput()
    {
        if (!player.isGrounded)
        {
            if (flight_input && player.currentEnergy > 0)
            {
                player.isFlying = true;
            }
            else
            {
                player.isFlying = false;
            } 
        }
        else
        {
            if (flight_inputPrimer)
            {
                tapWaitTime -= Time.deltaTime;
            }

            if (tapWaitTime <= 0 && flight_inputPrimer && !flight_input)
            {
                flight_inputPrimer = false;
                tapWaitTime = 0.6f;
                player.isFlying = false;
            }
            else if (tapWaitTime > 0 && flight_inputPrimer && flight_input && player.currentEnergy > 0)
            {
                player.isFlying = true;
            }
        }
    }

    private void HandleFireInput()
    {
        
        if (fireRight_input)
        {
            player.gunController.OnTriggerHoldRight();
        }
        if (!fireRight_input)
        {
            player.gunController.OnTriggerReleaseRight();
        }
        if (fireLeft_input)
        {
            player.gunController.OnTriggerHoldLeft();
        }
        if (!fireLeft_input)
        {
            player.gunController.OnTriggerReleaseLeft();
        }
    }

    private void HandleSwitchLockOnInput()
    {
        if (switchlockOn_input && lockOnFlag)
        {
            switchlockOn_input = false;
            //playerCamera.HandleLockOn();
        }
        else
        {
            switchlockOn_input = false;
        }
    }
}
