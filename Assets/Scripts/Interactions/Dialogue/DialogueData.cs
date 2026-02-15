using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    [SerializeField] private string _speaker;
    [SerializeField] private List<Line> _mainlines = new();
    [SerializeField] private List<Line> _otherLines = new();
    
    public string Speaker { get => _speaker; }
    public List<Line> MainLines { get => _mainlines; }
    public List<Line> OtherLines { get => _otherLines; }
    
}

[Serializable]
public class Line
{
    [SerializeField] private string _line;
    [SerializeField] private bool _shouldProcessed;

    public string DialogueLine => _line;
    public bool ShouldProcessed => _shouldProcessed;
}