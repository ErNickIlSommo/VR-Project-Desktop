using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DialogueNPCInside : GeneralDialogue, IInteractable
{
    public event Action<DialogEventInfo> OnDialogueStarted;
    public event Action<DialogEventInfo> OnDialogueFinished;
    public event Action<DialogEventInfo> OnDialogueRunning;
    
    // [SerializeField] private TextMeshProUGUI _ui;
    // [SerializeField] private GameObject _panel;

    [SerializeField] private int indexNPC = 0;
    
    [SerializeField] private DialogueData[] _dialogueAssets;
    private List<Line>[] _mainDialogues;
    private List<Line>[] _otherDialogues;
    private string _speaker;

    private int[] _mainLengths;
    private int[] _otherLengths;
    private int _dialogueDataCount;

    private int _dialogueIndex;
    private int _dialogueDataIndex;

    [SerializeField] private bool _isMainDialogueDone;
    [SerializeField] private bool _hasCompletedActivity1;
    [SerializeField] private bool _hasCompletedActivity2;

    public bool HasCompletedActivity1
    {
        get => _hasCompletedActivity1;
        set
        {
            _hasCompletedActivity1 = value;
            _isMainDialogueDone = false;
            _dialogueDataIndex = -1;
        }
    }

    public bool HasCompletedActivity2
    {
        get => _hasCompletedActivity2;
        set
        {
            _hasCompletedActivity2 = value;
            _isMainDialogueDone = false;
            _dialogueDataIndex = -1;
        }
    }
    
    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (_dialogueAssets == null || _dialogueAssets.Length == 0)
        {
            Debug.LogError("Dialogue assets non assegnati o vuoti.");
            return;
        }

        _speaker = _dialogueAssets[0].Speaker;
        _dialogueDataCount = _dialogueAssets.Length;
        
        _mainDialogues = new List<Line>[_dialogueDataCount];
        _otherDialogues = new List<Line>[_dialogueDataCount];
        
        _mainLengths = new int[_dialogueDataCount];
        _otherLengths = new int[_dialogueDataCount];

        for (int i = 0; i < _dialogueDataCount; i++)
        {
            _mainDialogues[i] = new List<Line>(_dialogueAssets[i].MainLines);
            _otherDialogues[i] = new List<Line>(_dialogueAssets[i].OtherLines);
            
            _mainLengths[i] = _mainDialogues[i].Count;
            _otherLengths[i] = _otherDialogues[i].Count;
        }

        _isMainDialogueDone = false;
        _hasCompletedActivity1 = false;
        _hasCompletedActivity2 = false;

        _dialogueIndex = -1;
        _dialogueDataIndex = 0;
    }
    
    
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Dialogue Interaction");
        if (_dialogueAssets == null || _dialogueAssets.Length == 0) return false;
        
        _dialogueIndex++;

        if (_dialogueIndex == 0)
        {
            BlockMovement(interactor);
            
            if(OnDialogueStarted != null) OnDialogueStarted.Invoke(
                new DialogEventInfo(
                    indexNPC,
                    _speaker,
                    true
                ));
            // _panel.SetActive(true);
        }

        if (IsDialogueFinished())
        {
            UnlockMovement(interactor);
            return true;
        }

        GoToNextLine();
        
        return true;
    }
    
    public void BlockMovement(Interactor interactor)
    {
        InputActionMap map = interactor.InputActions.FindActionMap("Controls", true);
        map.FindAction("Move", true).Disable(); 
        map.FindAction("Fly", true).Disable();
    }

    public void UnlockMovement(Interactor interactor)
    {
        InputActionMap map = interactor.InputActions.FindActionMap("Controls", true);
        map.FindAction("Move", true).Enable(); 
        map.FindAction("Fly", true).Enable(); 
    }

    private void GoToNextLine()
    {
        FindDialogueIndex();
        
        var currentDialogue = !_isMainDialogueDone ? 
            _mainDialogues[_dialogueDataIndex] : _otherDialogues[_dialogueDataIndex];
        
        if (OnDialogueRunning != null)
            OnDialogueRunning.Invoke(
                new DialogEventInfo(
                    indexNPC,
                    _speaker,
                    $"<b>{_speaker}:</b> {currentDialogue[_dialogueIndex].DialogueLine}",
                    true
            ));
        
        // _ui.text = $"<b>{_speaker}:</b> {currentDialogue[_dialogueIndex].DialogueLine}"; 
        // Debug.Log(_speaker + " " + currentDialogue[_dialogueIndex].DialogueLine + "dialogue Index: " + _dialogueIndex);

    }

    private bool IsDialogueFinished()
    {
        FindDialogueIndex();
        
        if (!_isMainDialogueDone)
        {
            if (_dialogueIndex < _mainLengths[_dialogueDataIndex]) return false;
            _dialogueIndex = -1;
            _isMainDialogueDone = true;
            
            if (OnDialogueFinished != null)
                OnDialogueFinished.Invoke(
                    new DialogEventInfo(
                        indexNPC,
                        _speaker,
                        true
                ));
            
            // _ui.text = "";
            // _panel.SetActive(false);
            return true;
        }
        
        // We are in one of the other dialogues
        if (_dialogueIndex < _otherLengths[_dialogueDataIndex]) return false;
        
        _dialogueIndex = -1;
        
        if (OnDialogueFinished != null)
            OnDialogueFinished.Invoke(
                new DialogEventInfo(
                    indexNPC,
                    _speaker,
                    true
            ));
        // _ui.text = "";
        // _panel.SetActive(false);
        
        return true; 
    }

    private void FindDialogueIndex()
    {
        if (!_hasCompletedActivity1 && !_hasCompletedActivity2) _dialogueDataIndex = 0;
        if (_hasCompletedActivity1) _dialogueDataIndex = 1;
        if (_hasCompletedActivity2) _dialogueDataIndex = 2; 
    }
}
