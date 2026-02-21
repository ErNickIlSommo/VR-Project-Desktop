using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Master: MonoBehaviour
{
    // Dialogue HUD
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI dialogueText;
    
    // Goal HUD
    [SerializeField] private TextMeshProUGUI goalText;
    
    // Score HUD
    [SerializeField] private CanvasGroup scoreCanvasGroup;
    // [SerializeField] private TextMeshProUGUI scoreText;

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
        scoreCanvasGroup.alpha = 0;
        
        if (globalData.InsideDone)
        {
            foragingBeeOutside.OnDialogueStarted += DialogueStarted;
            foragingBeeOutside.OnDialogueRunning += DialogueRunning;
            foragingBeeOutside.OnDialogueFinished += DialogueFinished;
            
            flowerActivity.ActivityRunning += FlowerRunning;
            flowerActivity.ActivityFinished += FlowerFinished;
            
            goalText.text = "<b>Obiettivo:</b> Parla con l'ape bottinatrice vicino all'entrata";
            return;
        }
        
        goalText.text = "<b>Obiettivo:</b> Parla con l'ape bottinatrice vicino all'entrata"; 
        
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
        nurseActivity.OnStartActivity += NurseInit;
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
            {
                // Last dialogue inside the hive
                goalText.text = "<b>Obiettivo:</b> Esci dall'alveare";
                exit.CanExit = true;
            }
            else
            {
                // Start Dialogue
                nurseBee.HasCompletedActivity1 = true;
                globalData.FirstTalkComplete = true;
                goalText.text = "<b>Obiettivo:</b> Cerca l'ape nutrice";
            }
        }

        // Nurse bee
        if (dialogueInfo.IndexNPC == 2)
        {
            // When you talk with nurse bee but you havent complete the activity
            if (nurseBee.HasCompletedActivity1 && !nurseBee.HasCompletedActivity2)
            {
                nurseActivity.EnableActivity();
                goalText.text = "<b>Obiettivo:</b> Sfama le larve";
            }

            // When you talk with nurse bee and you have complete the activity
            if (nurseBee.HasCompletedActivity1 && nurseBee.HasCompletedActivity2)
            {
                goalText.text = "<b>Obiettivo:</b> Cerca l'ape spazzina"; 
            }
        }

        // Undertaker Bee
        if (dialogueInfo.IndexNPC == 3)
        {
            if (undertakerBee.HasCompletedActivity1 && !undertakerBee.HasCompletedActivity2)
            {
                undertakerActivity.EnableActivity();
                undertakerActivity.StartActivity();
                goalText.text = "<b>Obiettivo:</b> Butta i cadaveri nell'abisso";
            }

            if (undertakerBee.HasCompletedActivity1 && undertakerBee.HasCompletedActivity2)
            {
                goalText.text = "<b>Obiettivo:</b> Parla con l'ape bottinatrice vicino all'entrata"; 
            }
        }

        // Foraging Bee Outside starter dialogue
        if (dialogueInfo.IndexNPC == 1 && globalData.InsideDone)
        {
            if (foragingBeeOutside.HasCompletedActivity2)
            {
                goalText.text = "<b>Obiettivo:</b> Rientra nell'alveare";
                exit.CanExit = true;
            }
            else
            {
                Debug.Log("Master: Enabling Activity");
                goalText.text = "<b>Obiettivo:</b> Cerca ed impollina i fiori";
                flowerActivity.EnableActivity();
                flowerActivity.StartActivity();
            }
        }
    }

    // When you start nurse activity
    private void NurseInit(bool status)
    {
        goalText.text = "<b>Obiettivo:</b> Sfama le larve";
    }
    
    private void NurseFinished(bool status)
    {
        if (!status)
        {
            goalText.text = "Hai sbagliato troppe volte. Rifai l'attività"; 
            return;
        }
        
        nurseBee.HasCompletedActivity2 = true;
        foragingBee.HasCompletedActivity1 = true;
        undertakerBee.HasCompletedActivity1 = true;
        globalData.NurseComplete = true;
        goalText.text = "<b>Obiettivo:</b> Parla con l'ape nutrice";
    }

    private void UndertakerFinished(bool status)
    {
        if (!status) return;
        undertakerBee.HasCompletedActivity2 = true;
        foragingBee.HasCompletedActivity2 = true;
        globalData.CorpseComplete = true;

        nurseBee.DisableInteraction();
        goalText.text = "<b>Obiettivo:</b> Parla con l'ape spazzina";
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
        goalText.text = "<b>Obiettivo:</b> Parla con l'ape bottinatrice";
        // flowerActivity.EnableActivity();
    }
}