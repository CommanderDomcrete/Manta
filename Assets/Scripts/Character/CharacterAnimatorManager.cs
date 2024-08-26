using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isBoosting)
    {
        if (isBoosting)
        {
            horizontalValue = 0;
            verticalValue = 2;
        }

        character.animator.SetFloat("Horizontal", horizontalValue, 0.15f, Time.deltaTime);
        character.animator.SetFloat("Vertical", verticalValue, 0.15f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true)
    {
        character.animator.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        // CAN BE USED TO STOP CHARACTER FORM ATTEMPTING NEW ACTIONS
        // FOR EXAMPLE, IF YOU LAND HEAVILY, AND BEGIN PERFORMING A LANDING ANIMATION
        // THIS FLAG WILL TURN TO TRUE IF YOU ARE STUNNED
        // WE CAN THEN CHECK FOR THIS BEFORE ATTEMPTING NEW ACTIONS
        character.isPerformingAction = isPerformingAction;
    }
}
