using UnityEngine;

public class LarvaEventInfo
{
    private int _id;
    private string _larvaName;
    private bool _requestResult;
    
    // Getters and Setters
    
    
    public int Id { get { return _id; } set { _id = value; } }
    
    public string LarvaName => _larvaName;
    
    public bool RequestResutlt
    {
        get { return _requestResult; }
        // set { _requestResult = value; }
    }

    public LarvaEventInfo(int id, string name, bool requestResult)
    {
        _requestResult = requestResult;
    }

    public LarvaEventInfo(int id, bool requestResult)
    {
        _id = id;
        _larvaName = string.Empty;
        _requestResult = requestResult;
    }
}
