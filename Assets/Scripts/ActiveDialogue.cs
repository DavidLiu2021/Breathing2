using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDialogue : MonoBehaviour
{
    public Camera defaultCamera;
    public Camera zoomedCamera;
    public Transform npcTransform;

    public GameObject subtitleUI;
    private bool isZoomedIn = false;

    void Start()
    {
        defaultCamera.gameObject.SetActive(true);
        zoomedCamera.gameObject.SetActive(false);
        subtitleUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){
            if (isZoomedIn){
                ZoomOut();
            }else{
                ZoomIn();
            }
        }
    }

    void ZoomIn(){
        defaultCamera.gameObject.SetActive(false);
        zoomedCamera.gameObject.SetActive(true);
        transform.LookAt(npcTransform);
        subtitleUI.SetActive(true);
        isZoomedIn = true;
    }

    void ZoomOut(){
        defaultCamera.gameObject.SetActive(true);
        zoomedCamera.gameObject.SetActive(false);
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
        subtitleUI.SetActive(false);
        isZoomedIn = false;
    }
}
