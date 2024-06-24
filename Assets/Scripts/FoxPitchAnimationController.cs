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
    private bool isJumping = false;
    private bool inGapTrigger = false;
    
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
        transform.Translate(Vector3.forward * speed() * Time.deltaTime);
        isGrounded = Physics.CheckSphere(checkBox.position, 0.1f, layermask);

        // detect space key pressed
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded){
            Jump();
        }

        // jump across the gap
        if (inGapTrigger && Input.GetKeyDown(KeyCode.Space)){
            // JumpGap();
            StartCoroutine(JumppGap());
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
        if (isJumping && isGrounded){
            isJumping = false;
            animator.SetBool("onGround", true);
        }else if(!isGrounded){
            animator.SetBool("onGround", false);
        }
    }

    public void Jump(){
        rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        isJumping = true;
        animator.SetTrigger("Jump");
        // isGrounded = false;
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

    void JumpGap(){
        transform.position = GapLand02.position + new Vector3(0, 1, 0);
        // animator.CrossFade(jumpHash, 0.1f);
    }

    IEnumerator JumppGap()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = GapLand02.position + new Vector3(0, 1, 0);
        float elapsedTime = 0f;


        while (elapsedTime < jumpDuration){
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition; 
    }
}
