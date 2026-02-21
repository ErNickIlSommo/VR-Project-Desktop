using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Master: MonoBehaviour
{
    // HUD
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI dialogueText;

    // Dialogues NPCs
    [SerializeField] private DialogueNPCInside foragingBee;
    [SerializeField] private DialogueNPCInside nurseBee;
    [SerializeField] private DialogueNPCInside undertakerBee;
    [SerializeField] private DialogueNPCInside foragingBeeOutside;
    
    // Activities
    [SerializeField] private NewNurse nurseActivity;
    [SerializeField] private DeadActivity undertakerActivity;
    [SerializeField] private FlowerActivity flowerActivity;
    
    // Global Data
    [SerializeField] private GlobalData globalData;
    
    [SerializeField] private Exit exit;

    private void Awake()
    {
        dialogueText.text = "";
        canvasGroup.alpha = 0;

        if (globalData.InsideDone)
        {
            foragingBeeOutside.OnDialogueStarted += DialogueStarted;
            foragingBeeOutside.OnDialogueRunning += DialogueRunning;
            foragingBeeOutside.OnDialogueFinished += DialogueFinished;
            
            flowerActivity.ActivityRunning += FlowerRunning;
            flowerActivity.ActivityFinished += FlowerFinished;
            return;
        }
        
        foragingBee.OnDialogueStarted += DialogueStarted;
        nurseBee.OnDialogueStarted += DialogueStarted;
        undertakerBee.OnDialogueStarted += DialogueStarted;

        foragingBee.OnDialogueRunning += DialogueRunning;
        nurseBee.OnDialogueRunning += DialogueRunning;
        undertakerBee.OnDialogueRunning += DialogueRunning;

        foragingBee.OnDialogueFinished += DialogueFinished;
        nurseBee.OnDialogueFinished += DialogueFinished;
        undertakerBee.OnDialogueFinished += DialogueFinished;

        nurseActivity.ActivityFinished += NurseFinished;
        undertakerActivity.ActivityFinished += UndertakerFinished;
        
    }

    private void DialogueStarted(DialogEventInfo dialogueInfo)
    {
        if (!dialogueInfo.Status) return;
        canvasGroup.alpha = 1;
    }

    private void DialogueRunning(DialogEventInfo dialogueInfo)
    {
        if (!dialogueInfo.Status) return;
        dialogueText.text = dialogueInfo.TextNPC;
    }

    private void DialogueFinished(DialogEventInfo dialogueInfo)
    {
        if (!dialogueInfo.Status) return;
        dialogueText.text = "";
        canvasGroup.alpha = 0;

        // Foraging Bee Inside
        if (dialogueInfo.IndexNPC == 1 && !globalData.InsideDone)
        {
            if (foragingBee.HasCompletedActivity2)
                exit.CanExit = true;
            else
            {
                nurseBee.HasCompletedActivity1 = true;
                globalData.FirstTalkComplete = true;
            }
        }

        // Nurse bee
        if (dialogueInfo.IndexNPC == 2 && nurseBee.HasCompletedActivity1)
        {
            nurseActivity.EnableActivity();
        }

        // Undertaker Bee
        if (dialogueInfo.IndexNPC == 3 && undertakerBee.HasCompletedActivity1)
        {
            undertakerActivity.EnableActivity();
        }

        // Foraging Bee Inside (when all activities are finished)
        /*if (dialogueInfo.IndexNPC == 1 && foragingBee.HasCompletedActivity2 && !globalData.InsideDone)
        {
            exit.CanExit = true;
        }*/

        // Foraging Bee Outside starter dialogue
        if (dialogueInfo.IndexNPC == 1 && globalData.InsideDone)
        {
            if (foragingBeeOutside.HasCompletedActivity2)
            {
                exit.CanExit = true;
            }
            else
            {
                Debug.Log("Master: Enabling Activity");
                flowerActivity.EnableActivity();
                flowerActivity.StartActivity();
            }
        }
    }

    private void NurseFinished(bool status)
    {
        if (!status) return;
        
        nurseBee.HasCompletedActivity2 = true;
        foragingBee.HasCompletedActivity1 = true;
        undertakerBee.HasCompletedActivity1 = true;
        globalData.NurseComplete = true;
    }

    private void UndertakerFinished(bool status)
    {
        if (!status) return;
        undertakerBee.HasCompletedActivity2 = true;
        foragingBee.HasCompletedActivity2 = true;
        globalData.CorpseComplete = true;

        nurseBee.DisableInteraction();
    }

    private void FlowerRunning(bool status)
    {
        if (!status) return;
        foragingBeeOutside.HasCompletedActivity1 = true;
    }
    
    private void FlowerFinished(bool status)
    {
        if (!status) return;
        foragingBeeOutside.HasCompletedActivity2 = true;
        // flowerActivity.EnableActivity();
    }
}