using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTunnelInteraction : MonoBehaviour, Interactable
{
    [SerializeField] private TextMeshProUGUI triggerPromptUI;
    [SerializeField] private string promptText;
    [SerializeField] private Transform playerRopeOffset;
    [SerializeField] private Transform playerLandOffset;
    [SerializeField] private float DetectTriggerRadius = 0.5f;
    [SerializeField] private RopeBehavior _ropeBehavior;
    private PlayerController player;
   

    public float DetectionRadius()
    {
        return DetectTriggerRadius;
    }

    // Start is called before the first frame update
    public bool CanInteract()
    {
        return true;
    }

    void Awake()
    {
        gameObject.tag = "Interactable";
    }

    private void Start()
    {
        HideInteractablePrompt();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void OnInteract()
    {
        if (player._currentPlayerState == PlayerController.PlayerState.onLand)
            OnGetOnRope();
        else if (player._currentPlayerState == PlayerController.PlayerState.onRope)
            OnGetOffRope();
       
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

    private void OnGetOnRope()
    {
        _ropeBehavior.ropeIsActive = true;
        _ropeBehavior.AddWeightToRope(2);
        player._currentPlayerState = PlayerController.PlayerState.onRope;
        TweensManager.instance.MoveTargetToPosition(player.transform, playerRopeOffset.position, 0.5f);
        
        
        
    }

    private void OnGetOffRope()
    {
        _ropeBehavior.ropeIsActive = false;
        if (player.catRescued)
        {
            GameManager.instance.OnPlayerWin();
        }

        player._currentPlayerState = PlayerController.PlayerState.onLand;
        TweensManager.instance.MoveTargetToPosition(player.transform, playerLandOffset.position, 0.3f);
        player.transform.rotation = playerLandOffset.rotation;
        
    }
}