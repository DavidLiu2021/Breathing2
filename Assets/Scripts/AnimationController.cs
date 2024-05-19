using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public AudioSource audioSource;
    public Animator animator;
    public float loudnessThreshold = 0.1f;
    public AudioLoudnessDetection detector;

    private bool isPlayingAnimation = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromAudioClip(audioSource.timeSamples, audioSource.clip);
        //Debug.Log("Loudness: " + loudness);
        //Debug.Log("Loudness Threshold: " + loudnessThreshold);

        // Check if Blooming animation is playing/finished
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Blooming") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f){
                isPlayingAnimation = false;
            }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Closing") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f){
                isPlayingAnimation = false;
            }
        
        
         if (loudness > loudnessThreshold && !isPlayingAnimation)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Closing"))
            {
                Debug.Log("Playing Blooming animation");
                animator.SetTrigger("PlayBlooming");
                isPlayingAnimation = true;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Blooming"))
            {
                Debug.Log("Playing Closing animation");
                animator.SetTrigger("PlayClosing");
                isPlayingAnimation = true;
            }
        }

    }
}
