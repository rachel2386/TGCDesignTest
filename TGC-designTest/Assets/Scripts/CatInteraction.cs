using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CatInteraction : MonoBehaviour,Interactable
{
    [SerializeField] private TextMeshProUGUI triggerPromptUI;
    [SerializeField] private string promptText;
    [SerializeField] private Transform catRopeOffset;
    [SerializeField] private Transform catLandOffset;
    [SerializeField] private float DetectTriggerRadius = 0.5f;
    private bool canInteract = true;
    
    // Start is called before the first frame update
    void Awake()
    {
        canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract()
    {
        transform.parent = catRopeOffset;
        TweensManager.instance.MoveTargetToLocalPosition(transform, Vector3.zero, 0.3f);
        
        canInteract = false;
        HideInteractablePrompt();
        EnableOtherAnimalTriggers();
        FindObjectOfType<RopeBehavior>().AddWeightToRope(1);
        
    }

    void EnableOtherAnimalTriggers()
    {
        foreach (var animal in FindObjectsOfType<OtherAnimalBehavior>())
        {
            animal.OnActivateTrigger();
        }
    }

    public void OnRescued()
    {
        TweensManager.instance.MoveTargetToPosition(transform, catRopeOffset.position, 0.6f);
    }

    public bool CanInteract()
    {
        return canInteract;
    }

    public float DetectionRadius()
    {
        return DetectTriggerRadius;
    }

    public void HideInteractablePrompt()
    {
        triggerPromptUI.text = promptText;
        triggerPromptUI.gameObject.SetActive(false);
    }

    public void ShowInteractablePrompt()
    {
        triggerPromptUI.text = promptText;
        triggerPromptUI.gameObject.SetActive(true);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, DetectTriggerRadius);
    }
}
