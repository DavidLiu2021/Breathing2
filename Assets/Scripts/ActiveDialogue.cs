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
    private bool inNPCtrigger = false;
    private FoxPitchAnimationController foxMove;
    private bool NPCactive = false;

    void Start()
    {
        defaultCamera.gameObject.SetActive(true);
        zoomedCamera.gameObject.SetActive(false);
        subtitleUI.SetActive(false);

        foxMove = GetComponent<FoxPitchAnimationController>();
    }

    void Update()
    {
        if (!inNPCtrigger){
            foxMove.SetCanMove(true);
        }else if (Input.GetKeyDown(KeyCode.E)){
            if (isZoomedIn){
                ZoomOut();
            }else{
                ZoomIn();
            }
            foxMove.SetCanMove(true);
            NPCactive = true;
        }else{
            foxMove.SetCanMove(false);
        }

        if (inNPCtrigger){
            Debug.Log("inNPCTrigger");
        }

        if (NPCactive){
            foxMove.SetCanMove(true);
        }
    }

    

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("NPCTrigger")){
            inNPCtrigger = true;
        }
    }

    void OnTriggerExit(Collider other){
        if (other.CompareTag("NPCTrigger")){
            inNPCtrigger = false;
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
