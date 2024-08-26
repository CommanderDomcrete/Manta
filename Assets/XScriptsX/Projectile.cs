using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public CharacterManager character;
    public LayerMask collisionMask;
    public Color trailColour;
    float speed = 10;
    //float damage = 1;

    float lifeTime = 3;
    float skinWidth = 0.1f;

    void Start() {
        Destroy(gameObject, lifeTime);

        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 0.1f, collisionMask);
        if (initialCollisions.Length > 0) {
            OnHitObject(initialCollisions[0], transform.position);
        }

        GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColour);
    }
    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }
    private void Update() {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollisions(float moveDistance) {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide)) {
            OnHitObject(hit.collider, hit.point);
        }
    }

    void OnHitObject(Collider c, Vector3 hitPoint) {
        //IDamageable damageableObject = c.GetComponent<IDamageable>();
        //if (damageableObject != null) {
        //    damageableObject.TakeHit(damage, hitPoint, transform.forward);
        //}
        character = c.GetComponent<CharacterManager>();
        if(character != null)
        {
            character.TakeDamage(hitPoint, transform.forward);
        }
        Destroy(gameObject);
    }
}
