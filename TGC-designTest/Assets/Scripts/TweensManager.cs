using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweensManager : MonoBehaviour
{
    public static TweensManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTargetToPosition(Transform objectToMove, Vector3 targetPosition, float duration)
    {
        StartCoroutine(moveToPosition(objectToMove, targetPosition, duration));
        
    }

    IEnumerator moveToPosition(Transform objectToMove, Vector3 targetPosition, float duration)
    {
        var currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            objectToMove.position = Vector3.Lerp(objectToMove.position, targetPosition, currentTime/duration);
            yield return new WaitForEndOfFrame();
        }
        
    }
    
    public void MoveTargetToLocalPosition(Transform objectToMove, Vector3 targetLocalPosition, float duration)
    {
        StartCoroutine(moveToLocalPosition(objectToMove, targetLocalPosition, duration));
        
    }

    IEnumerator moveToLocalPosition(Transform objectToMove, Vector3 targetLocalPosition, float duration)
    {
        var currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            objectToMove.localPosition = Vector3.Lerp(objectToMove.localPosition, targetLocalPosition, currentTime/duration);
            yield return new WaitForEndOfFrame();
        }
        
    }
    
    public void RotateTargetToRotation(Transform objectToMove, Vector3 targetEulerAngle, float duration)
    {
        StartCoroutine(Rotate(objectToMove, targetEulerAngle, duration));
        
    }

    IEnumerator Rotate(Transform objectToMove, Vector3 targetEulerAngle, float duration)
    {
        var currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            objectToMove.eulerAngles = Vector3.Slerp(objectToMove.eulerAngles, targetEulerAngle, currentTime/duration);
            yield return new WaitForEndOfFrame();  
        }
        
    }
    
    public void TweenFloatValue(float targetFloat, float targetValue, float duration)
    {
        StartCoroutine(TweenFloat(targetFloat, targetValue, duration));
        
    }

    IEnumerator TweenFloat(float targetFloat, float toValue, float duration)
    {
        var currentTime = 0f;
       
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            targetFloat = Mathf.Lerp(targetFloat, toValue, currentTime/duration);
            yield return new WaitForEndOfFrame();  
        }
        
    }
}
