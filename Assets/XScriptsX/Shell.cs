using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public Rigidbody myRigibody;
    public float forceMin;
    public float forceMax;

    float lifetime = 4;
    float fadeTime = 2;
    void Start(){
        float force = Random.Range (forceMin, forceMax);    
        myRigibody.AddForce(transform.right * force);
        myRigibody.AddTorque(Random.insideUnitSphere * force);

        StartCoroutine (Fade ());
    }

    // Update is called once per frame
    IEnumerator Fade() {
        yield return new WaitForSeconds(lifetime);

        float percent = 0;
        float fadeSpeed = 1 / fadeTime;
        Material mat = GetComponent<Renderer>().material;
        Color initialColour = mat.color;

        while (percent < 1) {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initialColour, Color.clear, percent);
            yield return null;
        }

        Destroy (gameObject);
    }

}
