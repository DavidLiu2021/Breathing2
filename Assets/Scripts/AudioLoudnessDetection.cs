using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{
    public int sampleWindow = 20;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip){
        int startPosition = Mathf.Max(clipPosition - sampleWindow, 0);
        int samplesToRead = Mathf.Min(sampleWindow, clip.samples - startPosition);
        if (samplesToRead <= 0) return 0f;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        //compute total loudness of the sample window
        float totalLoudness = 0;
        for (int i = 0; i < samplesToRead; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / samplesToRead;
    }
}
