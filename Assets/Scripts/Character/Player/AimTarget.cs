using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTarget : MonoBehaviour
{

    [Header("Aim Target Settings")]
    [SerializeField] public Transform target;
    [SerializeField] Transform targetPivotTransform;
    float currentAimTargetZPosition;
    LayerMask playerMask;

    private void Start()
    {
        playerMask = LayerMask.GetMask("Player");
        
        currentAimTargetZPosition = 500f;
    }
    private void Update()
    {
        AimAtTarget();
        
    }
    public void AimAtTarget()
    {
        Camera cam = PlayerCamera.instance.cameraObject;
        target.transform.rotation = cam.transform.rotation;

        if (PlayerCamera.instance.currentLockOnTarget != null)
        {
            Vector3 displacement = target.transform.position - targetPivotTransform.position;
            float distance = displacement.sqrMagnitude;
            float maxDistance = PlayerCamera.instance.maximumLockOnDistance;
            target.transform.position = PlayerCamera.instance.currentLockOnTarget.position + (PlayerCamera.instance.currentLockOnTarget.parent.GetComponent<GetVelocity>().velocity * (distance / (maxDistance))/100);
            Debug.DrawRay(targetPivotTransform.position, target.transform.position - targetPivotTransform.position, Color.red);
        }
        else
        {
            RaycastHit hit;
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentAimTargetZPosition, ~playerMask);

            if (hit.distance > 0)
            {
                target.transform.position = hit.point;
            }
            else
            {
                target.transform.position = cam.transform.position + cam.transform.forward * currentAimTargetZPosition;
            }
          
        }
    }

}
