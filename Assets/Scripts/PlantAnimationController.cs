using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantAnimationController : MonoBehaviour
{
    public GameObject PlantPrefab;
    public Animator animator1;
    public Animator animator2;
    public float volumeThreshold = 0.5f;
    public float highPitchMultiplier = 1.0f;
    public float lowPitchMultiplier = 0.5f;
    public float spawnInterval = 0.05f;

    private List<GameObject> animatedObjects = new List<GameObject>();
    private float timeSinceLastSpawn = 0.0f;
    private bool isAnimation1Playing = false;
    
    
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
        animator1.speed = pitch;
        animator2.speed = pitch;

        foreach (var obj in animatedObjects){
            Animator animator = obj.GetComponent<Animator>();
            if (animator != null){
                animator.speed = pitch;
            }
        }

        timeSinceLastSpawn += Time.deltaTime;
        if (currentVolume > volumeThreshold && timeSinceLastSpawn > spawnInterval){
            GameObject newObject = Instantiate(PlantPrefab, transform.position, Quaternion.identity);
            Animator animator = newObject.GetComponent<Animator>();
            if (animator != null){
                animator.SetBool("PlayAnimation", true);
                animator.speed = pitch;
            }
            animatedObjects.Add(newObject);
            timeSinceLastSpawn = 0.0f;
        }

        //two test plant animation controller
        if (currentVolume > volumeThreshold && !isAnimation1Playing){
            animator1.SetBool("PlayAnimation", true);
            isAnimation1Playing = true;
        }

        if (isAnimation1Playing && animator1.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f){
            animator2.SetBool("PlayAnimation", true);
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
