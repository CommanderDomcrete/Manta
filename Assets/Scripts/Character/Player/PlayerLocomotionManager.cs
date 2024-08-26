using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;

    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;

    public bool isBoosting;

    [Header("Movement Settings")]
    [SerializeField]private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float walkSpeed = 5;
    [SerializeField] float runSpeed = 20;
    [SerializeField] float boostSpeed = 40;
    [SerializeField] float rotationSpeed = 15;
    public float turnSpeed;

    [Header("Flight Settings")]
    [SerializeField] float flightSpeed = 40;
    [SerializeField] float upThrust = 80;
    [SerializeField] float flightEnergyCost = 20;

    [Header("Quick Boost")]
    [SerializeField] private Vector3 quickBoostDirection;
    [SerializeField] float quickBoostSpeed = 150;
    [SerializeField] float quickBoostDuration = 0.01f;
    float modifiedQuickBoostDuration;
    [SerializeField] bool quickBoosting = false;
    [SerializeField] float quickBoostEnergyCost = 40;
    float speedDamp = 0f;
    Vector3 currentBoostDirection;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        HandleFlight();
        HandleGroundedMovement();
        HandleRotation();
    }

    private void GetVerticalAndHorizontalInputs()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
    }

    private void HandleGroundedMovement()
    {
        GetVerticalAndHorizontalInputs();

        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isBoosting)
        {
            moveDirection = player.transform.forward * boostSpeed;
            turnSpeed = rotationSpeed/rotationSpeed * 2;
        }
        else
        {
            turnSpeed = rotationSpeed;
            if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                moveDirection = moveDirection * runSpeed;
            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                moveDirection = moveDirection * walkSpeed;
            } 
        }
        player.characterController.Move((moveDirection * Time.deltaTime) + (quickBoostDirection * Time.deltaTime));
        
        if(quickBoostDirection != Vector3.zero && !quickBoosting)
        {
            speedDamp += Time.deltaTime;
            if(speedDamp > 1)
            {
                speedDamp = 1;
            }
            quickBoostDirection = Vector3.Lerp(currentBoostDirection, Vector3.zero, speedDamp);
        }
    }

    private void HandleRotation()
    {
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    public void HandleFlight()
    {
        if (player.isPerformingAction)
            return;
        
        if (player.currentEnergy <= 0)
        {
            player.isFlying = false;
            return;
        }

        if (player.isFlying)
        {
            if(yVelocity.y < flightSpeed)
            {
                yVelocity.y += upThrust * Time.deltaTime;
            }
            if(yVelocity.y > flightSpeed)
            {
                yVelocity.y = flightSpeed;
            }
            player.currentEnergy -= flightEnergyCost * Time.deltaTime;
        }
    }

    public void AttemptQuickBoost()
    {
        if (!player.isGrounded)
        {
            modifiedQuickBoostDuration = quickBoostDuration * 5;
        }
        else
        {
            modifiedQuickBoostDuration = quickBoostDuration;
        }

        if (player.currentEnergy <= 0)
            return;

        quickBoosting = true;

        
        if (PlayerInputManager.instance.moveAmount > 0 && quickBoosting)
        {
            quickBoostDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            quickBoostDirection +=PlayerCamera.instance.transform.right * horizontalMovement;
            quickBoostDirection.y = 0;
            quickBoostDirection.Normalize();

            StartCoroutine(QuickBoost());
        }

        else if(PlayerInputManager.instance.moveAmount <= 0 && quickBoosting)
        {
            quickBoostDirection = PlayerCamera.instance.transform.forward;
            
            StartCoroutine(QuickBoost());
        }
        else if (!quickBoosting)
        {
            quickBoostDirection = Vector3.zero;
        }
    }
    public IEnumerator QuickBoost()
    {
        quickBoostDirection *= quickBoostSpeed;
        currentBoostDirection = quickBoostDirection;

        yield return new WaitForSeconds(modifiedQuickBoostDuration);
        player.currentEnergy -= quickBoostEnergyCost;
        speedDamp = 0;
        quickBoosting = false;
    }
}


