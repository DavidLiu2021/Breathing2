using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach the camera to the character
/// </summary>

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset0; // fox position
    
    // Start is called before the first frame update
    void Start()
    {
        offset0 = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset0;
    }
}
