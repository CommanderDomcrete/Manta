using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;
    [Header("Ground Check & Flying")]  
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRadius = 3f;

    [SerializeField] protected float gravityForce = -30f; 
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float groundedYVelocity = -20f;
    [SerializeField] protected float fallStartYVeclocity = -5;
    protected bool fallingVelocityHasBeenSet = false;
    [SerializeField] protected float inAirTimer = 0;
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
        HandleGroundCheck();
        if(character.isGrounded)
        {
            if (yVelocity.y < 0 && !character.isFlying)
            {
                inAirTimer = 0;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {   
            if(yVelocity.y > 0 && !character.isFlying)
            {
                yVelocity.y += 2 * gravityForce * Time.deltaTime;
            }
            if (character.isFlying)
            {
                inAirTimer = 0;
            }
            else
            {
                inAirTimer = inAirTimer + Time.deltaTime;
                character.animator.SetFloat("inAirTimer", inAirTimer);
                yVelocity.y += gravityForce * Time.deltaTime;
            }
        }
        character.characterController.Move(yVelocity * Time.deltaTime);
    }

    protected virtual void FixedUpdate()
    {
    }

    protected void HandleGroundCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }
}
