using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//rope behavior logic
//1. WS to extend/rewind rope
//2. AD to swing rope horizontally
//3. if exceed maximum weight limit, snap rope
public class RopeBehavior : MonoBehaviour
{
    public enum RopeState
    {
        connected = 0,
        snapped,
        reconnected
    }

    public RopeState currentRopeState;
    public bool ropeIsActive;
    
    [SerializeField] private Transform ropeScalerTransform;
    [SerializeField] private Transform ropeRotatorTransform;
    [SerializeField] private Transform ropeSnapPoint;
    [SerializeField] private Transform playerFollowOffset;
    [SerializeField] private float ropeExtendSpeed = 1;
    [SerializeField] private float ropeMaxAngle;
    [SerializeField] private float ropeRotationSpeed = 1;
    [SerializeField] private float ropeMinLength;
    [SerializeField] private float debug_ropeInitLength;
   
    private float ropeLength = 0; //0-1
    private float ropeRoll = 0;

    [SerializeField] private int maximumWeightLimit = 6;
    private int currentWeightOnRope = 0;
    private float ropeSnapDistanceFromPivot = 0;
    

    void Start()
    {
        currentRopeState = RopeState.connected;
        ropeLength = debug_ropeInitLength;
        if (ropeScalerTransform == null || ropeRotatorTransform == null)
            Debug.LogError("rope scaler and rope rotator must be assigned.");

        GameManager.instance.PlayerWon += ResetRope;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ropeIsActive) return;

        
        if (currentRopeState == RopeState.connected)
        {
            CalculateRopeScale();
            SwingRope();
        }
        else if (currentRopeState == RopeState.snapped)
        {
            CalculateRopeFall();
        }
        else
        {
            MovePlayerAlongReconnectedRope();
        }
       
    }

    void CalculateRopeScale()
    {
        ropeLength += Input.GetAxis("Vertical") * -ropeExtendSpeed * Time.deltaTime / 100;

        if (ropeLength < ropeMinLength)
            ropeLength = ropeMinLength;
        else if (ropeLength > 1)
            ropeLength = 1;

        var newScale = ropeScalerTransform.localScale;
        newScale.y = ropeLength;
        ropeScalerTransform.localScale = newScale;
    }

    void SwingRope()
    {
        var ropeDir = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        ropeRoll = ropeDir * ropeMaxAngle;
        var newEulerAngles = ropeRotatorTransform.localEulerAngles;
        //newEulerAngles.z = ropeRoll;
        newEulerAngles.z =Mathf.MoveTowardsAngle(newEulerAngles.z, ropeRoll, Time.deltaTime * ropeRotationSpeed);
        transform.localEulerAngles = newEulerAngles;
        
    }

    public void AddWeightToRope(int addedWeight)
    {
        if(currentWeightOnRope > maximumWeightLimit) return;
        currentWeightOnRope += addedWeight;
       StartCoroutine(TriggerAddWeightEffectAfterSeconds(0.1f));
       // TweensManager.instance.TweenFloatValue(ropeLength, ropeLength + 0.1f, 0.3f);
        if(currentWeightOnRope > maximumWeightLimit)
            RopeSnap();
    }

    private void RopeSnap()
    {
        print("rope snapped!");
        currentRopeState = RopeState.snapped;
        FindObjectOfType<RescuerChainBehavior>().OnActivateTrigger();
    }

    public void RopeReconnected()
    {
        print("rope reconnected!");
        ropeSnapDistanceFromPivot = Vector3.Distance(transform.position, playerFollowOffset.position);
        currentRopeState = RopeState.reconnected;
        FindObjectOfType<PlayerController>().catRescued = true;

    }

    void CalculateRopeFall()
    {
        var newRopePos = ropeSnapPoint.position;
        newRopePos += Physics.gravity * Time.deltaTime;
        ropeSnapPoint.position = newRopePos;

    }

    void MovePlayerAlongReconnectedRope()
    {
        var moveDirection = Vector3.Normalize(transform.position - playerFollowOffset.position);
        
        var moveDistance = Vector3.Distance(transform.position, playerFollowOffset.position);
        print(moveDistance);
        
        if (moveDistance > 1f && moveDistance <= ropeSnapDistanceFromPivot+ 0.1f)
            playerFollowOffset.position += Input.GetAxis("Vertical") * moveDirection * (ropeExtendSpeed/2) * Time.deltaTime;
       

    }

    IEnumerator TriggerAddWeightEffectAfterSeconds(float seconds)
    {
        var secondsPassed = 0f;
        while (secondsPassed < seconds)
        {
            secondsPassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        ropeLength += 0.02f;
        ropeExtendSpeed -= 0.5f;
    }

    void ResetRope()
    {
        ropeSnapPoint.position = ropeScalerTransform.position;
        playerFollowOffset.position = ropeScalerTransform.position;
        ropeLength = ropeMinLength;
        currentRopeState = RopeState.connected;
        ropeIsActive = false;

        
    }
}