using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetVelocity : MonoBehaviour
{
    public Vector3 velocity;
    Vector3 oldPos = Vector3.zero;
    Vector3 currentPos;

    void Update()
    {
        currentPos = transform.position;
        velocity = (currentPos - oldPos) / Time.deltaTime;
        oldPos = currentPos;
    }

}
