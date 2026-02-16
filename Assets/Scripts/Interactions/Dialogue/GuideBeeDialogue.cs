using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GuideBeeDialogue : GeneralDialogue, IInteractable
{
    [SerializeField] TextMeshProUGUI _ui;
    [SerializeField] private GameObject _panel;
    
    [SerializeField] private DialogueData _dialogueAsset;
    
    private List<Line> _mainDialogue;
    private List<Line> _otherDialogue;
    private string _speaker;
    
    private int _mainLength;
    private int _otherLength;

    private int _index = -1;

    private bool _isMainDialogueDone = false;

    private void Awake()
    {
        base.Awake();
        _panel.SetActive(false);
    }

    private void Start()
    {
        _speaker = _dialogueAsset.Speaker;
        _mainDialogue = _dialogueAsset.MainLines;
        _otherDialogue = _dialogueAsset.OtherLines;
        
        _mainLength = _mainDialogue.Count;
        _otherLength = _otherDialogue.Count;
    }

    public bool Interact(Interactor interactor)
    {
        if (!_dialogueAsset) return false;

        _index++;
        
        if (_index == 0)
        {
            BlockMovement(interactor);
            // Show Panel
            _panel.SetActive(true);
        }
        
        if (IsDialogueFinished())
        {
            UnlockMovement(interactor);
            // Close action
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

    private bool IsDialogueFinished()
    {
        // We are in the main dialogue
        if (!_isMainDialogueDone)
        {
            if (_index < _mainLength) return false;
            _index = -1;
            _isMainDialogueDone = true;
            _ui.text = "";
            _panel.SetActive(false);
            return true;
        }

        // We are in the other dialogue
        if (_index < _otherLength) return false;
        _index = -1;
        _ui.text = "";
        _panel.SetActive(false);
        return true;
    }

    private void UnlockMovement(Interactor interactor)
    {
        InputActionMap map = interactor.InputActions.FindActionMap("Controls", true);
        map.FindAction("Move", true).Enable();
    }

    private void GoToNextLine()
    {
        var currentDialogue = !_isMainDialogueDone ? _mainDialogue : _otherDialogue;
        
        // Debug.Log(currentDialogue[_index].DialogueLine);
        _ui.text = $"<b>{_speaker}:</b> {currentDialogue[_index].DialogueLine}";
        
        _index++;
        
        // check
        
    }
}
