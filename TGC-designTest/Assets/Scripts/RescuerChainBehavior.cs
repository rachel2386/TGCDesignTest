using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RescuerChainBehavior : MonoBehaviour
{
    
    public bool canTrigger = false;
    [SerializeField] private Transform triggerVolume;
    [SerializeField] private float DetectTriggerRadius = 0.5f;
    [SerializeField] private Transform ropeSnapPoint;
    [SerializeField] private GameObject rescuerMesh;
    [SerializeField] private GameObject endingMesh;
    private Transform player;
    
    

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
       rescuerMesh.SetActive(false);
       endingMesh.SetActive(false);
       canTrigger = false;
      
       GameManager.instance.PlayerWon += DisableChainAndShowFriends;

    }

    // Update is called once per frame
    void Update()
    {
        if (canTrigger && Vector3.Distance(triggerVolume.position, player.position) <= DetectTriggerRadius)
        {
            TargetEnterTrigger();
        }
        
        // if (canTrigger && Vector3.Distance(transform.position, player.position) > DetectTriggerRadius)
        // {
        //     TargetExitTrigger();
        // }
    }

    void TargetEnterTrigger()
    {
        RescuePlayer();
        canTrigger = false;

    }

    void TargetExitTrigger()
    {
        canTrigger = true;
       
    }

    public void OnActivateTrigger()
    {
        canTrigger = true;
        
    }

    void RescuePlayer()
    {
        
        rescuerMesh.SetActive(canTrigger);
        TweensManager.instance.MoveTargetToPosition(transform, ropeSnapPoint.position, 0.3f);
        FindObjectOfType<RopeBehavior>().RopeReconnected();
        
    }

    void DisableChainAndShowFriends()
    {
        rescuerMesh.SetActive(false);
        endingMesh.SetActive(true);
    }


    // void PromptUIControl(bool displayPrompt)
    // {
    //     triggerPromptUI.text = promptText;
    //     triggerPromptUI.gameObject.SetActive(displayPrompt);
    // }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(triggerVolume.position,DetectTriggerRadius);
    }
}