using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveformBarChart : MonoBehaviour
{
    public AudioSource audioSource;
    public int sampleWindow = 64;
    public GameObject barPrefab;
    public Transform barContainer;
    public float barWidth = 0.1f;
    public float heightMultiplier = 100f;

    private float[] waveData;

    void Start()
    {
        waveData = new float[sampleWindow];
        CreateBars();
    }

    void Update()
    {
        if (audioSource.isPlaying && audioSource.clip != null)
        {
            int clipPosition = audioSource.timeSamples;
            int startPosition = Mathf.Max(clipPosition - sampleWindow / 2, 0);
            int samplesToRead = Mathf.Min(sampleWindow, audioSource.clip.samples - startPosition);

            if (samplesToRead > 0)
            {
                audioSource.clip.GetData(waveData, startPosition);
                UpdateBars(samplesToRead);
            }
        }
    }

    void CreateBars()
    {
        for (int i = 0; i < sampleWindow; i++)
        {
            GameObject bar = Instantiate(barPrefab, barContainer);
            bar.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * barWidth, 0);
        }
    }

    void UpdateBars(int samplesToRead)
    {
        for (int i = 0; i < samplesToRead; i++)
        {
            RectTransform bar = barContainer.GetChild(i).GetComponent<RectTransform>();
            bar.sizeDelta = new Vector2(barWidth, waveData[i] * heightMultiplier);
        }
    }
}
