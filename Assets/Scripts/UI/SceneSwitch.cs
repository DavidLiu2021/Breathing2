using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{

    public void ConfirmLevel(){
        SceneManager.LoadScene("Selection_Animal");
    }
    
    public void ConfirmAnimal(){
        SceneManager.LoadScene("Selection_Landscape");
    }

    public void ConfirmLandscape(){
        SceneManager.LoadScene("Selection_Decoration");
    }

    public void ConfirmDecoration(){
        SceneManager.LoadScene("Map");
        // StartCoroutine(LoadMapScene());
    }

    // private IEnumerator LoadMapScene()
    // {
    //     AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);

    //     while (!asyncLoad.isDone)
    //     {
    //         yield return null;
    //     }

    //     Scene mapScene = SceneManager.GetSceneByName("Map");
        
    //     if (mapScene.IsValid())
    //     {
    //         SceneManager.SetActiveScene(mapScene);
    //     }
    //     else
    //     {
    //         Debug.LogError("Failed to load and activate the Map scene.");
    //     }
    // }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}