using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform followTargetTransform;
    private Vector3 cameraOffset;
    [SerializeField] private float followSpeed = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - followTargetTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, followTargetTransform.position + cameraOffset,
            Time.deltaTime * followSpeed);
    }
}
