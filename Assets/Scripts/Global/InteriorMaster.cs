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
    // [SerializeField] private DialogueNPCInside foragingBeeOutside;

    // Activities
    // [SerializeField] private NurseActivity nurseActivity;
    [SerializeField] private CorpseActivity corpseActivity;

    [SerializeField] private bool testGlobal;
    [SerializeField] private GlobalData globalData;
    [SerializeField] private Exit exit;

    

    private void Awake()
    {
        
        // nurseActivity.CanStartActivity = false;
        corpseActivity.CanStartActivity = false;
        
        dialogueText.text = "";
        dialoguePanel.SetActive(false);

        /*
         * Dialogues Event Handler Initializations
         */
        foragingBeeInside.OnDialogueStarted += HandleDialogueStarted;
        nurseBee.OnDialogueStarted += HandleDialogueStarted;
        corpseBee.OnDialogueStarted += HandleDialogueStarted;
        /*
        foragingBeeOutside.OnDialogueStarted += HandleDialogueStarted;
        */

        foragingBeeInside.OnDialogueRunning += HandleDialogueRunning;
        nurseBee.OnDialogueRunning += HandleDialogueRunning;
        corpseBee.OnDialogueRunning += HandleDialogueRunning;
        /*
        foragingBeeOutside.OnDialogueRunning += HandleDialogueRunning;
        */
        
        foragingBeeInside.OnDialogueFinished += HandleDialogueFinished;
        nurseBee.OnDialogueFinished += HandleDialogueFinished;
        corpseBee.OnDialogueFinished += HandleDialogueFinished;
        /*
        foragingBeeOutside.OnDialogueFinished += HandleDialogueFinished;
        */
        
        /*
         * Activity Event Handler Initializations
         */
        // nurseActivity.OnActivityCompleted += HandleNurseActivityTerminated;
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
        
        Debug.Log("Index NPC: " + dialogueInfo.IndexNPC);
        
        // Foraging Bee
        if (dialogueInfo.IndexNPC == 1)
        {
            Debug.Log("Starter dialog done");
            nurseBee.HasCompletedActivity1 = true;
            globalData.FirstTalkComplete = true;
        }
        
        // Nurse Bee
        if (dialogueInfo.IndexNPC == 2 && nurseBee.HasCompletedActivity1)
        {
            // nurseActivity.CanStartActivity = true;
        }
        
        // Corpse Bee
        if (dialogueInfo.IndexNPC == 3 && corpseBee.HasCompletedActivity1)
        {
            corpseActivity.CanStartActivity = true;
            Debug.Log("Master, set CanStartActivity to corpse activity");
        }

        if (dialogueInfo.IndexNPC == 1 && foragingBeeInside.HasCompletedActivity2)
        {
            // Do nothing for now
            Debug.Log("MASTER: It's time to go outside");
            exit.CanExit = true;
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
        globalData.NurseComplete = true;
    }
    
    private void HandleCorpseActivityTerminated(bool status)
    {
        if (!status) return;
        
        corpseBee.HasCompletedActivity2 = true;
        foragingBeeInside.HasCompletedActivity2 = true;
        globalData.CorpseComplete = true;
        // nurseBee.enabled = false;
        nurseBee.ChangeLayer(0);
    }
}
