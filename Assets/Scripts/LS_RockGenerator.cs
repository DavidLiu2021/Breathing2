using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LS_RockGenerator : MonoBehaviour
{
    public GameObject[] landscapes;
    public Transform player;
    public float spawnDistance = 10f;
    public int landscapesLength = 10;
    public float despawnDistance = 20f;
    public int poolSize = 10;

    private ObjectPool<GameObject> LSpool;
    private float nextspawnPosition = 0f;
    private List<GameObject> activeLandscapes = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        LSpool = new ObjectPool<GameObject>(CreateLS, OnGetLS, OnReleaseLS, OnDestroyLS, 
        false, poolSize, poolSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.z + spawnDistance > nextspawnPosition){
            SpawnLS();
        }

        DespawnLS();
    }

    GameObject CreateLS(){
        int randomIndex = Random.Range(0, landscapes.Length);
        GameObject landscape = Instantiate(landscapes[randomIndex]);
        landscape.SetActive(false);
        return landscape;
    }

    void OnGetLS(GameObject landscape){
        landscape.SetActive(true);
    }

    void OnReleaseLS(GameObject landscape){
        landscape.SetActive(false);
    }

    void OnDestroyLS(GameObject landscape){
        Destroy(landscape);
    }

    void SpawnLS(){
        GameObject landscape = LSpool.Get();
        landscape.transform.position = new Vector3(10, -2, nextspawnPosition);
        nextspawnPosition += landscapesLength;
        activeLandscapes.Add(landscape);
    }

    void DespawnLS(){
        for (int i = activeLandscapes.Count - 1; i >= 0; i--){
            if (activeLandscapes[i].transform.position.z < player.position.z - despawnDistance){
                LSpool.Release(activeLandscapes[i]);
                activeLandscapes.RemoveAt(i);
            }
        }
    }
}
