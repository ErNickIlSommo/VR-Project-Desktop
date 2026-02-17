using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Larvas : MonoBehaviour
{
    public event Action<LarvaEventInfo> SendInfoToMaster;
    public event Action<bool> IsLarvasTerminated;
    
    [SerializeField] private List<LarvaInteraction> larvas;
    private int _index = -1;

    // Getter and Setters

    
    public List<LarvaInteraction> NurseLarvas => larvas;
    
    public int Index { get => _index; set => _index = value; }
    
    // Methods
    
    
    private void Awake()
    {
        int counter = 1;
        foreach (LarvaInteraction larva in transform.GetComponentsInChildren<LarvaInteraction>())
        {
            larvas.Add(larva);
            larvas[counter - 1].OnRequestTerminated += HandleTerminatedLarva;
            counter++;
        } 
        
    }

    public void InitLarvasManager()
    {
        _index = -1;    
        ShuffleLarvasOrder();
        
        foreach (LarvaInteraction larva in transform.GetComponentsInChildren<LarvaInteraction>())
        {
            larva.InitLarva();
        } 
    }
    public void InitLarvasManager(int howMany)
    {        
        _index = -1;    
        ShuffleLarvasOrder(howMany);
        
        foreach (LarvaInteraction larva in transform.GetComponentsInChildren<LarvaInteraction>())
        {
            larva.InitLarva();
        } 
    }
    
    private void ShuffleLarvasOrder(int howMany = 5000)
    {
        if (howMany <= 0) howMany = 5000;

        for (int i = 0; i < (howMany); i++)
        {
            var rd1 = Random.Range(0, larvas.Count);
            var rd2 = Random.Range(0, larvas.Count);
            
            (larvas[rd1], larvas[rd2]) = (larvas[rd2], larvas[rd1]);
        }
    }

    private void HandleTerminatedLarva(LarvaEventInfo eventInfo)
    {
        if (SendInfoToMaster != null)
            SendInfoToMaster.Invoke(eventInfo);
    }

    public void SendRequestToLarvas(GrabbableObjectData requestedObject, float cooldown)
    {
        _index++;
        if (!(_index < larvas.Count) && IsLarvasTerminated != null)
        {
            IsLarvasTerminated.Invoke(true);
            return;
        }
        larvas[_index].StartRequest(requestedObject, cooldown);
            
    }
}
