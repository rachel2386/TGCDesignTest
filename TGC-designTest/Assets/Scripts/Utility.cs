using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
} 
public interface IDistanceTriggerable
{
    bool CanTrigger();
    void OnDistanceTriggerEnter();
    void OnDistanceTriggerExit();
    
    
    void ShowTriggerPrompt();
    void HideTriggerPrompt();

}



public interface Interactable
{
    void OnInteract();
    bool CanInteract();
    void ShowInteractablePrompt();
    void HideInteractablePrompt();
    float DetectionRadius();

}

 

