using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxPitchAnimationController : MonoBehaviour
{
    //Get frequency information from AudioPeer to use pitch/frequency to control the Fox animation
    
    // animation control
    public Animator animator;
    // public float transitionDuration = 0.2f;
    // public float toJumpDuration = 0.1f;
    public float jumpDuration = 1.5f;

    // transform control
    public float runspeed = 6f;
    public float walkspeed  = 5f;
    public float jumpforce = 5f;
    public Vector3 direction = Vector3.forward;
    public Transform GapLand02;

    private int walkHash;
    private int runHash;
    private int jumpHash;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool InGapTrigger = false;
    
    // Start is called before the first frame update
    void Start()
    {
        walkHash = Animator.StringToHash("Walk");
        runHash = Animator.StringToHash("Run");
        jumpHash = Animator.StringToHash("Jump");

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {     
        float lowFreqBandValue = GetAverageFreqValue(0, 3);
        float highFreqBandValue = GetAverageFreqValue(4, 7);

        if (highFreqBandValue > 0.1f && (!InGapTrigger || isGrounded)){
            animator.CrossFade(runHash, 0.01f);
            transform.Translate(direction * runspeed * Time.deltaTime);
        }else if (lowFreqBandValue > 0.1f && (!InGapTrigger || isGrounded)){
            animator.CrossFade(walkHash, 0.01f);
            transform.Translate(direction * walkspeed * Time.deltaTime);
        }

        // detect space key pressed
        if (Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("Space key pressed");
            Jump();
        }

        // jump across the gap
        if (InGapTrigger && Input.GetKeyDown(KeyCode.Space)){
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

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Ground")){
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision){
        if (collision.gameObject.CompareTag("Ground")){
            isGrounded = false;
        }
    }

    public void Jump(){
        if (isGrounded){
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
            animator.CrossFade(jumpHash, 0.0001f);
            isGrounded = false;
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.CompareTag("JumpTrigger")){
            InGapTrigger = true;
        }
    }

    void OnTriggerExit(Collider other){
        if (other.CompareTag("JumpTrigger")){
            InGapTrigger = false;
        }
    }

    void JumpGap(){
        transform.position = GapLand02.position + new Vector3(0, 1, 0);
        animator.CrossFade(jumpHash, 0.1f);
    }

    IEnumerator JumppGap()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = GapLand02.position + new Vector3(0, 1, 0);
        float elapsedTime = 0f;

        animator.CrossFade(jumpHash, 0.01f); // 假设过渡时间为0.1秒

        while (elapsedTime < jumpDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition; // 确保到达最终位置
    }
}
