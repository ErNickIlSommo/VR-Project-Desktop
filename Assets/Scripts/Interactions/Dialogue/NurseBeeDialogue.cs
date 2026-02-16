using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;

public class NurseBeeDialogue : GeneralDialogue, IInteractable
{
    [SerializeField] TextMeshProUGUI _ui;
    [SerializeField] private GameObject _panel;
    
    [SerializeField] private DialogueData[] _dialogueAssets;
    private List<Line>[] _mainDialogues;
    private List<Line>[] _otherDialogues;
    private string _speaker;

    private int[] _mainLengths;
    private int[] _otherLengths;
    private int _dialogueDataCount;

    private int _dialogueIndex;
    private int _dialogueDataIndex;
    
    private bool _isMainDialogueDone;
    private bool _hasCompletedActivity;

    public bool HasCompletedActivity
    {
        get => _hasCompletedActivity;
        set
        {
            _hasCompletedActivity = value;
            _isMainDialogueDone = false;
            _dialogueDataIndex = -1;
        }
    }

    private void Awake()
    {
        base.Awake();
        _panel.SetActive(false);
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
        _hasCompletedActivity = false;
        
        _dialogueIndex = -1;
        _dialogueDataIndex = 0;
    }
    
    public bool Interact(Interactor interactor)
    {
        if (_dialogueAssets == null || _dialogueAssets.Length == 0) return false;

        _dialogueIndex++;

        if (_dialogueIndex == 0)
        {
            BlockMovement(interactor);
            _panel.SetActive(true);
        }
        
        if (IsDialogueFinished())
        {
            UnlockMovement(interactor);
            return true;
        }

        GoToNextLine();

        
        
        return true;
    }

    private void BlockMovement(Interactor interactor)
    {
        InputActionMap map = interactor.InputActions.FindActionMap("Controls", true);
        map.FindAction("Move", true).Disable(); 
    }

    private void UnlockMovement(Interactor interactor)
    {
        InputActionMap map = interactor.InputActions.FindActionMap("Controls", true);
        map.FindAction("Move", true).Enable(); 
    }

    private void GoToNextLine()
    {
        _dialogueDataIndex = _hasCompletedActivity ? 1 : 0;
        
        var currentDialogue = !_isMainDialogueDone ? 
            _mainDialogues[_dialogueDataIndex] : _otherDialogues[_dialogueDataIndex];
        
        _ui.text = $"<b>{_speaker}:</b> {currentDialogue[_dialogueIndex].DialogueLine}"; 
        
        Debug.Log(_speaker + " " + currentDialogue[_dialogueIndex].DialogueLine + "dialogue Index: " + _dialogueIndex);
        
        // _dialogueIndex++;
    }

    private bool IsDialogueFinished()
    {
        _dialogueDataIndex = _hasCompletedActivity ? 1 : 0;

        // We are in one of the main dialogues
        if (!_isMainDialogueDone)
        {
            if (_dialogueIndex < _mainLengths[_dialogueDataIndex]) return false;
            _dialogueIndex = -1;
            _isMainDialogueDone = true;
            _ui.text = "";
            _panel.SetActive(false);
            return true;
        }
        
        // We are in one of the other dialogues
        if (_dialogueIndex < _otherLengths[_dialogueDataIndex]) return false;
        _dialogueIndex = -1;
        _ui.text = "";
        _panel.SetActive(false);
        return true;
    }
}
