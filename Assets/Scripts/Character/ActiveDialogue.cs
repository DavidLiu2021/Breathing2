using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Press [E] to interact with bird NPC when entering NPC area, press [E] to jump out of the conversation
/// Fox will be stopped by a Log, press [F] to destroy the Log and keep moving
/// </summary>

public class ActiveDialogue : MonoBehaviour
{
    public Camera defaultCamera;
    public Camera zoomedCamera;
    public Transform npcTransform;
    public Transform logTransform;

    public GameObject subtitleUI;
    private bool isZoomedIn = false;
    private bool inNPCtrigger = false;
    private bool inLogtrigger = false;
    private FoxPitchAnimationController foxMove;
    private bool NPCactive = false;
    private bool Logactive = false;

    void Start()
    {
        defaultCamera.gameObject.SetActive(true);
        zoomedCamera.gameObject.SetActive(false);
        subtitleUI.SetActive(false);

        foxMove = GetComponent<FoxPitchAnimationController>();
    }

    void Update()
    {
        if (!inNPCtrigger && !inLogtrigger){
            foxMove.SetCanMove(true);
        }else if (inNPCtrigger && Input.GetKeyDown(KeyCode.E)){
            if (isZoomedIn){
                ZoomOut();
                transform.rotation = Quaternion.LookRotation(Vector3.forward);
                subtitleUI.SetActive(false);
            }else{
                ZoomIn();
                transform.LookAt(npcTransform);
                subtitleUI.SetActive(true);
            }
            foxMove.SetCanMove(true);
            NPCactive = true;
        }else if (inLogtrigger){
            ZoomIn();
            if (Input.GetKeyDown(KeyCode.F)){
                logTransform.gameObject.SetActive(false);
                Logactive = true;
            }
        }else{
            foxMove.SetCanMove(false);
        }


        if (NPCactive){
            foxMove.SetCanMove(true); // Once player finishes the conversation, fox can keep moving
        }

        if (Logactive){
            ZoomOut();
            foxMove.SetCanMove(true); // Once player destroys the log, fox can keep moving
        }
    }


    // trigger area detection
    void OnTriggerEnter(Collider other){
        if (other.CompareTag("NPCTrigger")){
            inNPCtrigger = true;
        }

        if (other.CompareTag("LogTrigger")){
            inLogtrigger = true;
        }
    }

    void OnTriggerExit(Collider other){
        if (other.CompareTag("NPCTrigger")){
            inNPCtrigger = false;
        }

        if (other.CompareTag("LogTrigger")){
            inLogtrigger = false;
        }
    }

    // switch between the main camera and zoomed camera
    void ZoomIn(){
        defaultCamera.gameObject.SetActive(false);
        zoomedCamera.gameObject.SetActive(true);
        isZoomedIn = true;
    }

    void ZoomOut(){
        defaultCamera.gameObject.SetActive(true);
        zoomedCamera.gameObject.SetActive(false);
        isZoomedIn = false;
    }
}
