using System.Collections;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable {

    public float startingHealth;
    public float health { get; protected set; }
    protected bool dead;

    public event System.Action OnDeath; // This is a Delegate event which is triggered when an enemy dies

    protected virtual void Start() {
        health = startingHealth;
    }
    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection) {
        //Do some Stuff here with the hit var
        TakeDamage (damage);
    }

    public virtual void TakeDamage( float damage) {
        health -= damage;

        if (health <= 0 && !dead) {
            Die();
        }

    }

    [ContextMenu("Self Destruct")]
    public virtual void Die() {
        dead = true;
        if (OnDeath != null) {
            OnDeath();
        }
        GameObject.Destroy(gameObject);

    }
}