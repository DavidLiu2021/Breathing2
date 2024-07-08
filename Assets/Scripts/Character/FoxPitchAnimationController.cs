using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxPitchAnimationController : MonoBehaviour
{
    //Get frequency information from AudioPeer to use pitch/frequency to control the Fox animation
    
    // animation control
    public Animator animator;
    public float jumpDuration = 1.5f; // the time duration that fox jump across the gap

    // transform control
    public float runspeed = 6f;
    public float walkspeed  = 5f;
    public float jumpforce = 5f;
    // public Transform GapLand02;
    public Transform checkBox; // detect whether the fox collides with the ground
    public LayerMask layermask; // detect the Ground layer

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool inGapTrigger = false;
    private Vector3 gapPosition;
    private bool canMove = true; // defined in ActiveDialogue scripts
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {     
        MoveControl();
        UpdateAnimations();
    }

    // Defines the basic movement including speed, normal jump and gap jump
    private void MoveControl(){
        animator.SetFloat("speed", speed());
        isGrounded = Physics.CheckSphere(checkBox.position, 0.01f, layermask);

        if (!inGapTrigger && canMove){
            transform.Translate(Vector3.forward * speed() * Time.deltaTime);
        }

        // detect space key presseds
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded){
            Jump();
        }

        // jump across the gap
        if (inGapTrigger && Input.GetKeyDown(KeyCode.Space)){
            StartCoroutine(JumpGap(gapPosition));
        }
    }

    // use AuidoPeer script to get the audio frequency information, and then control the movement speed
    float GetAverageFreqValue(int startBand, int endBand){
        float sum = 0;
        for (int i = startBand; i <= endBand; i++){
            sum += AudioPeer._freqBand[i];
        }
        return sum / (endBand - startBand + 1);
    }

    float speed(){
        float HighPitch = GetAverageFreqValue(2, 7);
        float LowPitch = GetAverageFreqValue(0, 1);

        if (LowPitch > 0.1f){
            return walkspeed;
        }else if (HighPitch > 0.1f){
            return runspeed;
        }

        return 0f;
    }

    // update the bool value in animator
    void UpdateAnimations(){
        if (isGrounded){
            animator.SetBool("onGround", true);
        }else{
            animator.SetBool("onGround", false);
        }
    }

    // define the normal jump (not in the gap position)
    public void Jump(){
        rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        // rb.AddForce(Vector3.forward * jumpforce, ForceMode.Impulse);
        animator.SetTrigger("Jump");
    }

    // detect whether the fox is near the gap
    void OnTriggerEnter(Collider other){
        if (other.CompareTag("JumpTrigger")){
            gapPosition = other.transform.position;
            inGapTrigger = true;
        }
    }

    void OnTriggerExit(Collider other){
        if (other.CompareTag("JumpTrigger")){
            inGapTrigger = false;
        }
    }

    // define the gap jump
    IEnumerator JumpGap(Vector3 currentPosition)
    {
        Vector3 startPosition = transform.position;
        // Vector3 endPosition = GapLand02.position + new Vector3(0, 1, 0);
        Vector3 endPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z) + new Vector3(0, 0, 6);
        float elapsedTime = 0f;

        rb.isKinematic = true;

        rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        animator.SetTrigger("Jump");
        

        while (elapsedTime < jumpDuration){
            // transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / jumpDuration);

            float t = elapsedTime / jumpDuration;
            float height = 4 * 2f * (t - t * t);
            transform.position = Vector3.Lerp(startPosition, endPosition, t) + new Vector3(0, height, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition; 
        rb.isKinematic = false;
    }

    // see more details in ActiveDialogue scripts
    public void SetCanMove(bool value){
        canMove = value;
    }
}
