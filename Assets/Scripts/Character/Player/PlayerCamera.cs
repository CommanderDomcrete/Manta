using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public PlayerManager player;
    public Camera cameraObject;
    public PlayerInputManager playerInputManager;
    [SerializeField] Transform cameraPivotTransform;

    //CHANGE THESE TO TWEAK CAMERA PERFORMANCE
    [Header("Camera Settings")]
    private float cameraSmoothSpeed = 1;    //THE BIGGER THE NUMBER, THE LONGER THE CAMERA WILL TAKE TO REACH ITS POSITION DURING MOVEMENT
    [SerializeField] float leftAndRightRotationSpeed = 220;
    [SerializeField] float upAndDownRotationSpeed = 220;
    [SerializeField] float minimumPivot = -60; //LOWEST POINT DOWN
    [SerializeField] float maximumPivot = 80; //HIGHEST POINT UP
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;

    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition; //USED FOR CAMERA COLLISIONS (MOVES THE CAMERA OBJECT TO THIS POSITION UPON COLLIDING WITH OBJECT)
    [SerializeField] float horizontalLookAngle;
    [SerializeField] float verticalLookAngle;
    private float cameraZPosition; // VALUES USED FOR CAMERA COLLISIONS
    private float targetCameraZPosition; // VALUES USED FOR CAMERA COLLISIONS

    [Header("Lock-On Settings")]
    public Transform currentLockOnTarget;
    public float maximumLockOnDistance = 500;
    public Transform nearestLockOnTarget;
    List<CharacterManager> availableTargets = new List<CharacterManager>();
    public Vector3 screenPoint;

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
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            HandleFollowTarget();
            HandleRotations();
            HandleCollisions();
            
        }
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleRotations()
    {
        //IF LOCKED ON, FORCE ROTATION TOWARDS TARGET - NOT!!!!
        //ELSE ROTATE REGULARLY

        //NORMAL ROTATIONS
        horizontalLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
        verticalLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
        verticalLookAngle = Mathf.Clamp(verticalLookAngle, minimumPivot, maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        //ROTATE THIS GAME OBJECT LEFT AND RIGHT
        cameraRotation.y = horizontalLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;


        //ROTATE THE PIVOT OBJECT UP AND DOWN
        cameraRotation = Vector3.zero;
        cameraRotation.x = verticalLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions()
    {
        targetCameraZPosition = cameraZPosition;

        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        //WE CHECK IF THERE IS AN OBJECT IN FRONT OF OUR DESIRED DIRECTION ^ SEE ABOVE
        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            // IF THERE IS, WE GET OUR DISTANCE FROM IT
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            // WE THEN EQUATE OUR TARGET Z POSITION TO THE FOLLOWING
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        // IF OUR TARGET POSITION IS LESS THAN OUR COLLISION RADIUS, WE SUBTRACT OUR COLLISION RADIUS (MAKING IT SNAP BACK)
        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        // WE THEN APPLY OUR FINAL POSITION USING A LERP OVER A TIME OF 0.2F
        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }

    public void HandleLockOn()
    {
        
        float shortestDistance = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, maximumLockOnDistance);



        if (!Physics.CheckSphere(player.transform.position, 100, 6))
        {
            //Debug.Log("No Targets");
            ClearLockOnTargets();
            
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            if(character != null)
            {

                Vector3 lockOnTargetDirection = character.transform.position  - cameraObject.transform.position;
                float distanceFromTarget = Vector3.Distance(player.transform.position, character.transform.position);
                float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraObject.transform.forward);

                if (character.transform.root != player.transform.root && viewableAngle > -18 && viewableAngle < 18 && distanceFromTarget <= maximumLockOnDistance)//REPLACE VALUE 50 WITH A FIELD RELATING TO LOCKBOX SIZE **********
                {
                    availableTargets.Add(character);
                    
                }
                
            }
            
        }

        for (int k = 0; k < availableTargets.Count; k++)
        {
            float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);

            if(distanceFromTarget < shortestDistance)
            {
                shortestDistance = distanceFromTarget;
                nearestLockOnTarget = availableTargets[k].lockOnTransform;
            }
        }
        if(nearestLockOnTarget != null)
        {
            currentLockOnTarget = nearestLockOnTarget;
            screenPoint = cameraObject.WorldToScreenPoint(currentLockOnTarget.position);
            screenPoint.z = 0;
            //Debug.Log(currentLockOnTarget);
        }
        
    }

    public void ClearLockOnTargets()
    {
        availableTargets.Clear();
        currentLockOnTarget = null;
        nearestLockOnTarget = null;
       // Debug.Log("Targets Cleared");
    }
}
