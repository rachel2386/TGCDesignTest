using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableExample : MonoBehaviour,Interactable
{
    [SerializeField] private bool canInteract = false;
    [SerializeField]private float detectionRadius = 3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract()
    {
        print("interact");
    }

    public bool CanInteract()
    {
        return canInteract;
    }

    public float DetectionRadius()
    {
        return detectionRadius;
    }

    public void HideInteractablePrompt()
    {
       print("you cannot interact with me anymore");
    }

    public void ShowInteractablePrompt()
    {
        print("please interact with me");
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position,detectionRadius);
    }
}
