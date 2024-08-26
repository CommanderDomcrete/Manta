using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlightManager : CharacterLocomotionManager
{
    EnemyManager enemy;

    [Header("Flight Settings")]
    [SerializeField] Vector3 moveDirection;
    [SerializeField] float lateralSpeed = 45f;
    [SerializeField] float flightSpeed = 10f;
    [SerializeField] float upThrust = 10;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] Vector3 currentPosition;
    [SerializeField] Vector3 backstabOffset;
    [SerializeField] Transform target;

    [SerializeField] Vector3 targetRotationDirection;
    [SerializeField] float rotationSpeed = 1f;
    protected override void Awake()
    {
        base.Awake();

        
        enemy = GetComponent<EnemyManager>();

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {

            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    public void HandleAllMovement()
    {
        HandleFlight();
        HandleLateralMovement();
        HandleRotation();
    }
    private void HandleFlight()
    {
        
        if (targetPosition.y > enemy.transform.position.y)  
        {
            enemy.isFlying = true;
            
            if (yVelocity.y < flightSpeed)
            {
                yVelocity.y += upThrust * Time.deltaTime;
            }
            if (yVelocity.y > flightSpeed)
            {
                yVelocity.y = flightSpeed;
            }
        }
        else
        {
            yVelocity.y = 0;
            enemy.isFlying = false;
        }

            
    }
    private void HandleLateralMovement()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            backstabOffset = new Vector3(0, 30, 0) + (target.transform.forward.normalized * 30);
        }

        if (target != null)
        {
            targetPosition = target.position + backstabOffset;
            currentPosition = enemy.transform.position;
        }

        if (currentPosition != targetPosition)
        {
           enemy.characterController.Move(enemy.transform.forward * lateralSpeed * Time.deltaTime); 
           //enemy.characterController.Move(targetPosition * lateralSpeed * Time.deltaTime);
        }
        else
        {
            enemy.characterController.Move(Vector3.zero);
        }
 
            
        
    }

    private void HandleRotation()
    {
        moveDirection = (targetPosition - currentPosition).normalized;

        targetRotationDirection = Vector3.zero;
        targetRotationDirection = moveDirection;

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }
    
}
