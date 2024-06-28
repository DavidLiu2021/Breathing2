using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRiseController : MonoBehaviour
{
    public Transform player;
    public float activeDistance = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++){
            Transform tile = transform.GetChild(i);
            float Zdistance = Mathf.Abs(player.position.z - tile.position.z);
            Animator animator = tile.GetComponent<Animator>();

            if (Zdistance < activeDistance){
                tile.gameObject.SetActive(true);
                if (animator != null){
                    animator.SetBool("IsRising", true);
                }
            }else{
                if (animator != null){
                    animator.SetBool("IsRising", false);
                }
                tile.gameObject.SetActive(false);
            }
        }
    }
}
