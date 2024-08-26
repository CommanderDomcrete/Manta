using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetingManager : MonoBehaviour
{
    
    [SerializeField] Transform weaponHolder;
    [HideInInspector] GunController gunController;
    [SerializeField] Vector3 aimDirection;
    [SerializeField] Vector3 targetRotationDirection;
    float gunRotationSpeed = 100f;
    [SerializeField] Transform target;

    LayerMask playerLayer;
    void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        gunController = GetComponent<GunController>();
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {

            target = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterManager>().lockOnTransform;
        }
    }

    public void HandleAllTargeting()
    {
        HandleAiming();
        HandleFiring();
    }

    private void HandleAiming()
    {
        if (target != null)
        {
            aimDirection = (target.position - weaponHolder.position).normalized;
        }
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = aimDirection;
        
        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
            
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(weaponHolder.transform.rotation, newRotation, gunRotationSpeed * Time.deltaTime);
        weaponHolder.rotation = targetRotation;
    }

    private void HandleFiring()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, aimDirection, out hit, Mathf.Infinity) && target != null)
        {
            if (hit.collider == target.transform.parent.GetComponent<Collider>() && Vector3.Distance(transform.position, target.position) < 120f)
            {
                gunController.OnTriggerHoldRight();
            }
        }

    }
}
