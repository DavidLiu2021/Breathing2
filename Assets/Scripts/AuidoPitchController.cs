using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuidoPitchController : MonoBehaviour
{
    public AudioSource audioSource;
    public Renderer objectRenderer;
    public int sampleSize = 512;

    private float[] spectrumData;

    // Start is called before the first frame update
    void Start()
    {
        spectrumData = new float[sampleSize];
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);

        float maxVal = 0;
        float maxIndex = 0;
        for (int i = 0; i < sampleSize; i++){
            if (spectrumData[i] > maxVal){
                maxVal = spectrumData[i];
                maxIndex = i;
            }
        }

        float freq = maxIndex * AudioSettings.outputSampleRate / 2 / sampleSize;

        Color color = FrequencyToColor(freq);
        objectRenderer.material.color = color;
    }

    // change the render color based on frequency value
    Color FrequencyToColor(float frequency){
        float hue = Mathf.InverseLerp(20, 2000, frequency);
        return Color.HSVToRGB(hue, 1.0f, 1.0f);
    }
}
