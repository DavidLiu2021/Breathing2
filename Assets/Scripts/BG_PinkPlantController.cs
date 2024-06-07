using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_PinkPlantController : MonoBehaviour
{
    public GameObject BGPlantPrefab;
    public float volumeThreshold = 0.0f;
    public float highPitchMultiplier = 1.0f;
    public float lowPitchMultiplier = 0.5f;
    public float spawnInterval = 0.05f;
    public Vector2 spawnRange = new Vector2(10.0f, 10.0f);

    private List<GameObject> animatedObjects = new List<GameObject>();
    private float timeSinceLastSpawn = 0.0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get loudness info
        float currentVolume = GetCurrentVolume();

        // Frequency controls the animation play speed
        float pitch = GetPitch();

        foreach (var obj in animatedObjects){
            Animator animator = obj.GetComponent<Animator>();
            if (animator != null){
                animator.speed = pitch;
            }
        }

        timeSinceLastSpawn += Time.deltaTime;
        if (currentVolume > volumeThreshold && timeSinceLastSpawn > spawnInterval){
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnRange.x, spawnRange.x),
                -0.6f,
                Random.Range(-spawnRange.y, spawnRange.y)
            );
            
            GameObject newObject = Instantiate(BGPlantPrefab, randomPosition, Quaternion.identity);
            Animator animator = newObject.GetComponent<Animator>();
            if (animator != null){
                animator.SetBool("PlayAnimation", true);
                animator.speed = pitch;
            }
            animatedObjects.Add(newObject);
            timeSinceLastSpawn = 0.0f;
        }

    }

    private float GetCurrentVolume(){
        float sum = 0f;
        foreach (var sample in AudioPeer._samples){
            sum += sample * sample;
        }
        return Mathf.Sqrt(sum / AudioPeer._samples.Length);
    }

    private float GetHighPitch(){
        float maxFreq = 0f;
        for (int i = 4; i < 8; i++){
            if (AudioPeer._freqBand[i] > maxFreq){
                maxFreq = AudioPeer._freqBand[i];
            }
        }
        return maxFreq;
    }

    private float GetLowPitch(){
        float maxFreq = 0f;
        for (int i = 0; i < 4; i++){
            if (AudioPeer._freqBand[i] > maxFreq){
                maxFreq = AudioPeer._freqBand[i];
            }
        }
        return maxFreq;
    }
    
    private float GetPitch(){
        float highPitch = GetHighPitch();
        float lowPitch = GetLowPitch();

        if (highPitch > lowPitch){
            return highPitchMultiplier;
        }else{
            return lowPitchMultiplier;
        }
    }
}