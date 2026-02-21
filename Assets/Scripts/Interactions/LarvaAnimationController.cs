using UnityEngine;
using UnityEngine.Audio;

public class LarvaAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip requestClip;
    [SerializeField] private AudioClip acceptClip;
    [SerializeField] private AudioClip refuseClip;

    public void RequestFood()
    {
        animator.SetTrigger("Request");
        audioSource.PlayOneShot(requestClip);
    }

    public void AcceptFood()
    {
        animator.SetTrigger("Liked");
        audioSource.PlayOneShot(acceptClip);
    }

    public void RefuseFood()
    {
        animator.SetTrigger("Disliked");
        audioSource.PlayOneShot(refuseClip);
    }

    public void Reset()
    {
        animator.SetTrigger("Reset");
    }
}
