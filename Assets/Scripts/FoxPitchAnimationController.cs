using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxPitchAnimationController : MonoBehaviour
{
    //Get frequency information from AudioPeer to use pitch/frequency to control the Fox animation
    
    //public Renderer objectRenderer;
    public Animator animator;
    public float transitionDuration = 0.1f;

    private int walkHash;
    private int runHash;
    
    // Start is called before the first frame update
    void Start()
    {
        walkHash = Animator.StringToHash("Walk");
        runHash = Animator.StringToHash("Run");
    }

    // Update is called once per frame
    void Update()
    {
        float lowFreqBandValue = GetAverageFreqValue(0, 3);
        float highFreqBandValue = GetAverageFreqValue(4, 7);

        if (highFreqBandValue > 0.1f){
            animator.CrossFade(runHash, transitionDuration);
        }else if (lowFreqBandValue > 0.1f){
            animator.CrossFade(walkHash, transitionDuration);
        }
    }

    float GetAverageFreqValue(int startBand, int endBand){
        float sum = 0;
        for (int i = startBand; i <= endBand; i++){
            sum += AudioPeer._freqBand[i];
        }
        return sum / (endBand - startBand + 1);
    }
}
