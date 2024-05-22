using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioColorChanger : MonoBehaviour
{
    public Renderer objectRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float freqBandValue = AudioPeer._freqBand[6];

        Color color = FrequencyValueToColor(freqBandValue);
        objectRenderer.material.color = color;
    }

    Color FrequencyValueToColor(float value){
        float hue = Mathf.Clamp01(value / 10f);
        return Color.HSVToRGB(hue, 1.0f, 1.0f);
    }
}
