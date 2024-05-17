using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public AudioSource audioSource;
    public Animator animator;
    public string animationName;
    public float loudnessThreshold = 0.1f;
    public AudioLoudnessDetection detector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromAudioClip(audioSource.timeSamples, audioSource.clip);
        Debug.Log("Loudness" + loudness);
        
        if (loudness > loudnessThreshold){
            animator.Play(animationName);
        }
    }
}
