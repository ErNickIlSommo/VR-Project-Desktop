using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Rendering;

public class GuideBeeDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] TextMeshProUGUI _ui;
    [SerializeField] private GameObject _panel;
    
    [SerializeField] private DialogueData _dialogueAsset;
    
    private List<Line> _mainDialogue;
    private List<Line> _otherDialogue;
    private string _speaker;
    
    private int _mainLength;
    private int _otherLength;

    private int _index;

    private bool _isMainDialogueDone = false;

    private void Awake()
    {
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
        if (_index == 0)
        {
            BlockMovement();
            // Show Panel
            _panel.SetActive(true);
        }

        GoToNextLine();

        if (IsDialogueFinished())
        {
            UnlockMovement();
            // Close action
        }
        return true;
    }

    private void BlockMovement()
    {
        Debug.Log("Block Movement");
    }

    private bool IsDialogueFinished()
    {
        // We are in the main dialogue
        if (!_isMainDialogueDone)
        {
            if (_index < _mainLength) return false;
            _index = 0;
            _isMainDialogueDone = true;
            _ui.text = "";
            _panel.SetActive(false);
            return true;
        }

        // We are in the other dialogue
        if (_index < _otherLength) return false;
        _index = 0;
        _ui.text = "";
        _panel.SetActive(false);
        return true;
    }

    private void UnlockMovement()
    {
        Debug.Log("Unlock Movement");
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
