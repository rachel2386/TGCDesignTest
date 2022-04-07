using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//basic movement
//check if isGrounded, add gravity
//a bool to test if player is on rope
//if close to rope, can interact
public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        onLand,
        onRope
    }

    public PlayerState _currentPlayerState = PlayerState.onLand;

    public bool isOnRope = false;
    // private bool isGrounded = false;

    [SerializeField] private float movementSpeed = 10;
    [SerializeField] private float checkObstacleRayDistance = 0.4f;
    [SerializeField] private Transform eyes;
    [SerializeField] private float interactableDetectionRadius = 0.5f;
    List<GameObject> allInteractablesInGame = new List<GameObject>();
    private Interactable _currentInteractableSelected;
    [SerializeField] private Transform OnRopeOffset;
    public bool catRescued = false;


    // Start is called before the first frame update
    void Start()
    {
        foreach (var interactable in GameObject.FindGameObjectsWithTag("Interactable"))
        {
            if (interactable.GetComponent<Interactable>() != null)
                allInteractablesInGame.Add(interactable);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentPlayerState == PlayerState.onLand)
        {
            Move(DetectObstacles());
        }
        else if (_currentPlayerState == PlayerState.onRope)
        {
            transform.SetPositionAndRotation(OnRopeOffset.position, OnRopeOffset.rotation);
        }

        LookForInteractables();

        if (Input.GetButtonDown("Interact"))
        {
            if (_currentInteractableSelected != null)
                _currentInteractableSelected.OnInteract();
        }
    }

    void LookForInteractables()
    {
        var interactablesDetected = new List<Transform>();

        // test if there are triggers within the detectable radius. If so, add them to the list of detectable triggers
        foreach (var interactable in allInteractablesInGame)
        {
            var distanceToPlayer = Vector3.Distance(transform.position, interactable.transform.position) -
                                   interactable.GetComponent<Interactable>().DetectionRadius();
            if (distanceToPlayer < interactableDetectionRadius)
            {
                if (!interactablesDetected.Contains(interactable.transform))
                    interactablesDetected.Add(interactable.transform);
            }
        }

        if (interactablesDetected.Count > 0)
        {
            var foundEligibleOne = false;
            var closestInteractable = interactablesDetected[0].GetComponent<Interactable>();
            var closestDistance = Mathf.Infinity;


            foreach (var target in interactablesDetected)
            {
                var distanceToPlayer = Vector3.Distance(transform.position, target.position);
                var interactable = target.GetComponent<Interactable>();


                //in the case of multiple detected interactables, choose the one closest to the player
                if (distanceToPlayer < closestDistance &&
                    interactable.CanInteract() &&
                    interactable != _currentInteractableSelected)
                {
                    foundEligibleOne = true;
                    closestDistance = distanceToPlayer;
                    closestInteractable = interactable;
                }
            }
            // if detected an interactable, disable last selected interactable enable new one
            if (!foundEligibleOne) return;
           
            if (_currentInteractableSelected != null)
            {
                _currentInteractableSelected.HideInteractablePrompt();
                _currentInteractableSelected = null;
            }
            _currentInteractableSelected = closestInteractable;
            _currentInteractableSelected.ShowInteractablePrompt();
        }
        else
        {
            
            // if no interactables found, disable last selected interactable
            if (_currentInteractableSelected != null)
            {
                _currentInteractableSelected.HideInteractablePrompt();
                _currentInteractableSelected = null;
            }
        }
    }

    void Move(bool seeObstacle)
    {
        var speedMultiplier = movementSpeed * Time.deltaTime;
        var movementDirection = transform.right * Input.GetAxis("Horizontal") +
                                transform.forward * Input.GetAxis("Vertical");

        if (movementDirection.magnitude <= 0) return;

        FaceMoveDirection(movementDirection);
        if (seeObstacle)
            speedMultiplier = 0;

        transform.Translate(movementDirection * speedMultiplier);
    }

    void FaceMoveDirection(Vector3 moveDirection)
    {
        eyes.rotation = Quaternion.LookRotation(moveDirection);
    }


    bool DetectObstacles()
    {
        bool obstaclesDetected;
        var hitInfo = new RaycastHit();
        var castOffset = transform.position; //+ Vector3.down * (transform.localScale.y * 0.5f);
        // var overlapSphereCols =
        //     Physics.OverlapSphere(castOffset, checkGroundCastRadius, ~(1 << 6)); //All layers expect Ground layer

        if (Physics.Raycast(new Ray(eyes.position, eyes.forward * checkObstacleRayDistance),
            out hitInfo, checkObstacleRayDistance, ~(1 << 6)) && hitInfo.collider != null)
        {
            obstaclesDetected = true;
        }
        else
            obstaclesDetected = false;

        return obstaclesDetected;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var castOffset = transform.position; // + Vector3.down * (transform.localScale.y * 0.5f);
        //  Gizmos.DrawSphere(castOffset, checkGroundCastRadius);
        Gizmos.DrawRay(eyes.position, eyes.forward * checkObstacleRayDistance);
    }
}