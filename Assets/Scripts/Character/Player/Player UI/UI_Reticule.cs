using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Reticule: MonoBehaviour {

    PlayerUIHUDManager playerUIHUDManager;

    public LayerMask targetMask;
    Image reticule;
    Color originalDotColour;
    Vector2 originalPosition;

    private void Start() {
        Cursor.visible = false;
        reticule = GetComponent<Image>();
        originalDotColour = reticule.color;
        playerUIHUDManager = GetComponent<PlayerUIHUDManager>();
        originalPosition = transform.position;
    }

    void Update() {
        FollowTarget();
    }

    public void FollowTarget()
    {
        if(PlayerCamera.instance.currentLockOnTarget != null)
        {
            transform.position = PlayerCamera.instance.screenPoint;
            reticule.color = Color.red;
            transform.Rotate(0,0,0.2f) ;
        }
        else
        {
            transform.position = originalPosition;
            reticule.color = originalDotColour;
            transform.rotation = Quaternion.identity;
        }
    }     
}
