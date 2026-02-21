using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GlobalData", menuName = "Scriptable Objects/GlobalData")]
public class GlobalData : ScriptableObject
{
    [SerializeField] private bool firstTalkComplete = false;
    [SerializeField] private bool nurseComplete = false;
    [SerializeField] private bool corpseComplete = false;
    [SerializeField] private bool foragingComplete = false;

    [SerializeField] private bool insideDone = false;
    
    public bool FirstTalkComplete { get => firstTalkComplete; set => firstTalkComplete = value; }
    public bool NurseComplete { get => nurseComplete; set => nurseComplete = value; }
    public bool CorpseComplete { get => corpseComplete; set => corpseComplete = value; }
    public bool ForagingComplete { get => foragingComplete; set => foragingComplete = value; }
    
    public bool InsideDone { get => insideDone; set => insideDone = value; }
}
