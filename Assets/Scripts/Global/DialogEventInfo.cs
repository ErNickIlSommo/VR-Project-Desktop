using UnityEngine;

public class DialogEventInfo
{
    private int _indexNPC;
    private string _nameNPC;
    private string _textNPC;
    private bool _status;
    
    public int IndexNPC  { get => _indexNPC; set => _indexNPC = value; }
    public string NameNPC { get => _nameNPC; set => _nameNPC = value; }
    public string TextNPC { get => _textNPC; set => _textNPC = value; }
    public bool Status { get => _status; set => _status = value; }

    public DialogEventInfo(int indexNPC, bool status)
    {
        _indexNPC = indexNPC;
        _status = status;
        _nameNPC = "";
        _textNPC = "";
    }

    public DialogEventInfo(int indexNPC, string nameNPC, bool status)
    {
        _indexNPC = indexNPC;
        _nameNPC = nameNPC;
        _status = status;
        _textNPC = "";
    }

    public DialogEventInfo(int indexNPC, string nameNPC, string textNPC, bool status)
    {
        _indexNPC = indexNPC;
        _nameNPC = nameNPC;
        _textNPC = textNPC;
        _status = status;
    }
}
