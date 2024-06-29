using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxPitchAnimationController : MonoBehaviour
{
    //Get frequency information from AudioPeer to use pitch/frequency to control the Fox animation
    
    // animation control
    public Animator animator;
    public float jumpDuration = 1.5f;

    // transform control
    public float runspeed = 6f;
    public float walkspeed  = 5f;
    public float jumpforce = 5f;
    public Transform GapLand02;
    public Transform checkBox;
    public LayerMask layermask;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool inGapTrigger = false;
    private bool canMove = true;
    
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
            // JumpGap();
            StartCoroutine(JumppGap());
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    float GetAverageFreqValue(int startBand, int endBand){
        float sum = 0;
        for (int i = startBand; i <= endBand; i++){
            sum += AudioPeer._freqBand[i];
        }
        return sum / (endBand - startBand + 1);
    }

    float speed(){
        float HighPitch = GetAverageFreqValue(3, 7);
        float LowPitch = GetAverageFreqValue(0, 2);

        if (LowPitch > 0.1f){
            return walkspeed;
        }else if (HighPitch > 0.1f){
            return runspeed;
        }

        return 0f;
    }

    void UpdateAnimations(){
        if (isGrounded){
            animator.SetBool("onGround", true);
        }else{
            animator.SetBool("onGround", false);
        }
    }

    public void Jump(){
        rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        animator.SetTrigger("Jump");
    }

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("JumpTrigger")){
            inGapTrigger = true;
        }
    }

    void OnTriggerExit(Collider other){
        if (other.CompareTag("JumpTrigger")){
            inGapTrigger = false;
        }
    }

    // void JumpGap(){
    //     transform.position = GapLand02.position + new Vector3(0, 1, 0);
    // }

    IEnumerator JumppGap()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = GapLand02.position + new Vector3(0, 1, 0);
        float elapsedTime = 0f;

        // rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        // animator.SetTrigger("Jump");
        

        while (elapsedTime < jumpDuration){
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition; 
    }

    public void SetCanMove(bool value){
        canMove = value;
    }
}
