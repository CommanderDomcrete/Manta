using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool isFlying = false;
    public bool isGrounded = true;
    public bool dead;

    [Header("Stats")]
    public int power = 1;
    public float currentEnergy;
    public int maxEnergy;
    public int maxHitPointsValue = 1;
    public int currentHitPointsValue;

    [Header("Traits")]
    public Transform lockOnTransform;
    public ParticleSystem deathEffect;
    Material skinMaterial;
    Color skinColour;

    public event System.Action OnDeath;
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        currentHitPointsValue = maxHitPointsValue;
        if(GetComponent<Renderer>() != null)
        {
            skinMaterial = GetComponent<Renderer>().material;
            skinMaterial.color = skinColour;
            var main = deathEffect.main;
            main.startColor = new Color(skinColour.r, skinColour.g, skinColour.b, 1);
        }

    }

    protected virtual void Update()
    {
        animator.SetBool("isGrounded", isGrounded);
    }

    protected virtual void LateUpdate()
    {
        //isPerformingAction = animator.GetBool("isPerformingAction");
    }
    public virtual void TakeDamage(Vector3 hitPoint, Vector3 hitDirection)
    {
        currentHitPointsValue -= 1;

        AudioManager.instance.PlaySound("Impact", transform.position);

        if (currentHitPointsValue <= 0 && !dead)
        {
            Die();
            AudioManager.instance.PlaySound("Enemy Death", transform.position);
            Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)), deathEffect.main.startLifetimeMultiplier);
        }

    }
    protected virtual void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        
    }
}
