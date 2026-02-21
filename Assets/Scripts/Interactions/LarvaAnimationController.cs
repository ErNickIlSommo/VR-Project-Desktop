using UnityEngine;

public class LarvaAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource source;

    [SerializeField] private AudioClip requestClip;
    [SerializeField] private AudioClip acceptClip;
    [SerializeField] private AudioClip refuseClip;

    public void RequestFood()
    {
        animator.SetTrigger("Request");
    }

    public void AcceptFood()
    {
        animator.SetTrigger("Liked");
    }

    public void RefuseFood()
    {
        animator.SetTrigger("Disliked");
    }

    public void Reset()
    {
        animator.SetTrigger("Reset");
    }
}
