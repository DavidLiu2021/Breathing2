using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Get frequency info from AudioPeer, the 8 freqBands. then change the color based on freqBand selection
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
        float freqBandValue = AudioPeer._freqBand[7]; // get pitch info from the freqBand[]

        Color color = FrequencyValueToColor(freqBandValue);
        objectRenderer.material.color = color;
    }

    Color FrequencyValueToColor(float value){
        float hue = Mathf.Clamp01(value / 10f);
        return Color.HSVToRGB(hue, 0.6f, 0.8f);
    }
}
