using UnityEngine;

[CreateAssetMenu(fileName = "GlobalData", menuName = "Scriptable Objects/GlobalData")]
public class GlobalData : ScriptableObject
{
    [SerializeField] private bool firstTalkComplete = false;
    [SerializeField] private bool nurseComplete = false;
    [SerializeField] private bool corpseComplete = false;
    [SerializeField] private bool foragingComplete = false;

    public bool FirstTalkComplete { get => firstTalkComplete; set => firstTalkComplete = value; }
    public bool NurseComplete { get => nurseComplete; set => nurseComplete = value; }
    public bool CorpseComplete { get => corpseComplete; set => corpseComplete = value; }
    public bool ForagingComplete { get => foragingComplete; set => foragingComplete = value; }
}
