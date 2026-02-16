using UnityEngine;

public class DialogueTester : MonoBehaviour
{
    [SerializeField] private DialogueNPCInside _dialogue;

    [SerializeField] private bool _activity1Flag = false;
    [SerializeField] private bool _activity2Flag = false;

    private bool _nurseGuard = false;
    private bool _diggerGuard = false;

    private void Awake()
    {
        _dialogue = GetComponent<DialogueNPCInside>();
    }

    private void Update()
    {
        if (!_nurseGuard && _activity1Flag)
        {
            _dialogue.HasCompletedActivity1 = true;
            _nurseGuard = true;
        }

        if (!_diggerGuard && _activity2Flag)
        {
            _dialogue.HasCompletedActivity2 = true;
            _diggerGuard = true;
        }
    } 
}
