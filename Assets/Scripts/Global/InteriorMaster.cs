using System;
using TMPro;
using UnityEngine;

public class InteriorMaster : MonoBehaviour
{
    // HUD
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    // Dialogues NPCs
    [SerializeField] private DialogueNPCInside foragingBeeInside;
    [SerializeField] private DialogueNPCInside nurseBee;
    [SerializeField] private DialogueNPCInside corpseBee;
    [SerializeField] private DialogueNPCInside foragingBeeOutside;

    // Activities
    [SerializeField] private NurseActivity nurseActivity;
    [SerializeField] private CorpseActivity corpseActivity;

    
    private void Awake()
    {
        nurseActivity.CanStartActivity = false;
        corpseActivity.CanStartActivity = false;
        
        dialogueText.text = "";
        dialoguePanel.SetActive(false);

        /*
         * Dialogues Event Handler Initializations
         */
        foragingBeeInside.OnDialogueStarted += HandleDialogueStarted;
        nurseBee.OnDialogueStarted += HandleDialogueStarted;
        corpseBee.OnDialogueStarted += HandleDialogueStarted;
        foragingBeeOutside.OnDialogueStarted += HandleDialogueStarted;

        foragingBeeInside.OnDialogueRunning += HandleDialogueRunning;
        nurseBee.OnDialogueRunning += HandleDialogueRunning;
        corpseBee.OnDialogueRunning += HandleDialogueRunning;
        foragingBeeOutside.OnDialogueRunning += HandleDialogueRunning;
        
        foragingBeeInside.OnDialogueFinished += HandleDialogueFinished;
        nurseBee.OnDialogueFinished += HandleDialogueFinished;
        corpseBee.OnDialogueFinished += HandleDialogueFinished;
        foragingBeeOutside.OnDialogueFinished += HandleDialogueFinished;
        
        /*
         * Activity Event Handler Initializations
         */
        nurseActivity.OnActivityCompleted += HandleNurseActivityTerminated;
        corpseActivity.OnActivityCompleted += HandleCorpseActivityTerminated;
    }

    /*
     * Dialogues Handlers
     */
    private void HandleDialogueStarted(DialogEventInfo dialogueInfo)
    {
        if (!dialogueInfo.Status) return;
        dialoguePanel.SetActive(true); 
    }
    
    private void HandleDialogueRunning(DialogEventInfo dialogueInfo)
    {
        if (!dialogueInfo.Status) return;
        dialogueText.text = dialogueInfo.TextNPC;
    }
    
    private void HandleDialogueFinished(DialogEventInfo dialogueInfo)
    {
        if (!dialogueInfo.Status) return;
        dialogueText.text = "";
        dialoguePanel.SetActive(false); 
        
        // Foraging Bee
        if (dialogueInfo.IndexNPC == 1)
        {
            nurseBee.HasCompletedActivity1 = true;
        }
        
        // Nurse Bee
        if (dialogueInfo.IndexNPC == 2)
        {
            nurseActivity.CanStartActivity = true;
        }
        
        // Corpse Bee
        if (dialogueInfo.IndexNPC == 3)
        {
            corpseActivity.CanStartActivity = true;
        }
        
    }
    
    /*
     * Activity Handlers
     */
    private void HandleNurseActivityTerminated(bool status)
    {
        if (!status) return;
        
        nurseBee.HasCompletedActivity2 = true;
        foragingBeeInside.HasCompletedActivity1 = true;
        corpseBee.HasCompletedActivity1 = true;
    }
    
    private void HandleCorpseActivityTerminated(bool status)
    {
        if (!status) return;
        
        corpseBee.HasCompletedActivity2 = true;
        foragingBeeInside.HasCompletedActivity2 = true;
    }
}
