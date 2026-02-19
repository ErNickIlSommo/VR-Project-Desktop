using System;
using UnityEngine;
using TMPro;

public class OutsideMaster : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    
    [SerializeField] private GlobalData globalData;
    [SerializeField] private DialogueNPCInside foragingBee;
    [SerializeField] private Exit exit;
    [SerializeField] private Flowers outsideActivity;

    private void Awake()
    {
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
        
        foragingBee.OnDialogueStarted += HandleDialogueStarted;
        foragingBee.OnDialogueRunning += HandleDialogueRunning;
        foragingBee.OnDialogueFinished += HandleDialogueFinished;
        
        outsideActivity.OnActivityFinished += HandleActivityFinished;
        outsideActivity.OnActivityRunning += HandleOnActivityRunning;
    }

    private void Start()
    {
        exit.CanExit = false;
    }

    private void HandleActivityFinished(bool status)
    {
        if (!status) return;
        globalData.ForagingComplete = true;
        foragingBee.HasCompletedActivity2 = true;
        Debug.Log("OUTSIDE MASTER: outside activity finished");
    }

    private void HandleOnActivityRunning(bool status)
    {
        if (!status) return;
        foragingBee.HasCompletedActivity1 = true;
    }

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
       outsideActivity.StartActivity();
       
       exit.CanExit = true;
    }
}
