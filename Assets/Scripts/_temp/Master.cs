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
    
    // Activities
    [SerializeField] private NewNurse nurseActivity;
    [SerializeField] private DeadActivity undertakerActivity;
    
    // Global Data
    [SerializeField] private GlobalData globalData;
    
    [SerializeField] private Exit exit;

    private void Awake()
    {
        dialogueText.text = "";
        canvasGroup.alpha = 0;

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
    }

    private void DialogueFinished(DialogEventInfo dialogueInfo)
    {
        if (!dialogueInfo.Status) return;
        dialogueText.text = "";
        canvasGroup.alpha = 1;

        if (dialogueInfo.IndexNPC == 1)
        {
            nurseBee.HasCompletedActivity1 = true;
            globalData.FirstTalkComplete = true;
        }

        if (dialogueInfo.IndexNPC == 2 && nurseBee.HasCompletedActivity1)
        {
            nurseActivity.EnableActivity();
        }

        if (dialogueInfo.IndexNPC == 3 && undertakerBee.HasCompletedActivity1)
        {
            undertakerActivity.EnableActivity();
        }

        if (dialogueInfo.IndexNPC == 1 && foragingBee.HasCompletedActivity2)
        {
            exit.CanExit = true;
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
}