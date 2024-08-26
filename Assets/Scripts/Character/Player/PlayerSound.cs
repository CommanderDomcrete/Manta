using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioClip mechStepSound;
    PlayerManager player;

    private AudioSource source;
    void Start()
    {
        player = GetComponent<PlayerManager>();
        source = GetComponent<AudioSource>();
    }

    void PlayMechStep(AnimationEvent animationEvent)
    {
        if(animationEvent.animatorClipInfo.weight > 0.49 && player.isGrounded)
        {
            AudioClip clip = mechStepSound;
            source.clip = clip;
            source.PlayOneShot(clip);
        }
    }
}
