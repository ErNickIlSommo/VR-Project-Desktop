using UnityEngine;

public class LarvaAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

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
