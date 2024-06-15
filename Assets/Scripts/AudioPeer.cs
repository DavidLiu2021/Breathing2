using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// get spectrum info and divide into 8 freqBands
public class AudioPeer : MonoBehaviour
{
    public AudioSource _audioSource;
    public static float[] _samples = new float[512];
    public static float[] _freqBand = new float[8];
    
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
    }


    void GetSpectrumAudioSource(){
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBands(){
        int count = 0;
        for (int i = 0; i < 8; i++){
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if(i == 7){
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++){
                average += _samples[count] * (count + 1);
                    count++;
            }

            average /= count;
            _freqBand[i] = average * 10;
        }

        /*
        * 22050 / 512 = 43hertz per sample
        *
        * 20-60 hertz
        * 60-250 hertz
        * 250-500 hertz
        * 500-2000 hertz
        * 2000 - 4000 hertz
        * 4000 - 6000 hertz
        * 6000 - 20000 hertz
        *
        * 0 - 2 = 0 - 86 hertz
        * 1 - 4 = 87 - 258 hertz
        * 2 - 8 = 259 - 602 hertz
        * 3 - 16 = 603 - 1290 hertz
        * 4 - 32 = 1291 - 2666 hertz
        * 5 - 64 = 2667 - 5418 hertz
        * 6 - 128 = 5419 - 10922 hertz
        * 7 - 256 = 10923 - 21930 hertz
        */
    }

}
