using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OtherAnimalBehavior : MonoBehaviour
{
    
    public bool canTrigger = false;

    [SerializeField] private Transform triggerVolume;
    [SerializeField] private float DetectTriggerRadius = 0.5f;
    [SerializeField] private Transform playerOffset;
    [SerializeField] private GameObject animalMesh;
    private Transform player;
    
    

    // Start is called before the first frame update
    void Start()
    {

        GameManager.instance.PlayerWon += DisableAnimal;
        player = FindObjectOfType<PlayerController>().transform;
        animalMesh.SetActive(false);

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
        AttachToPlayer();
        canTrigger = false;

    }

    void DisableAnimal()
    {
        canTrigger = false;
        gameObject.SetActive(false);

    }

    public void OnActivateTrigger()
    {
        canTrigger = true;
        animalMesh.SetActive(canTrigger);
    }

    void AttachToPlayer()
    {
        transform.parent = playerOffset;
        TweensManager.instance.MoveTargetToLocalPosition(transform, Vector3.zero, 0.5f);
        TweensManager.instance.RotateTargetToRotation(transform, playerOffset.eulerAngles, 0.5f);
        FindObjectOfType<RopeBehavior>().AddWeightToRope(1);
        
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