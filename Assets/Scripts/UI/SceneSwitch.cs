using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{

    public void ConfirmAnimal(){
        SceneManager.LoadScene("Selection_Landscape");
    }

    public void ConfirmLandscape(){
        SceneManager.LoadScene("Selection_Decoration");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}