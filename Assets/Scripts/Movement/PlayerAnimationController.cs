using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float longIdleDelay = 5f;
    private PlayerState lastState;
    private float idleTimer;
    private bool longIdleTriggered = false;

    public void UpdateAnimations(PlayerState currentState, Vector2 moveValue)
    {
        //if (currentState != lastState)
        //{
            switch (currentState)
            {
                case PlayerState.Idle:
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsFlying", false);
                    break;
                case PlayerState.Walk:
                    animator.SetBool("IsWalking", true);
                    animator.SetBool("IsFlying", false);
                    break;
                case PlayerState.Fly:
                    animator.SetBool("IsFlying", true);
                    break;
            }
        //}

        if (currentState != PlayerState.Idle)
        {
            idleTimer = 0f;
            longIdleTriggered = false;
            return;
        }

        idleTimer += Time.deltaTime;

        // Trigger una volta sola quando superi la soglia
        if (!longIdleTriggered && idleTimer >= longIdleDelay)
        {
  
            animator.SetTrigger("LongIdle1");
            longIdleTriggered = true;
        }

        lastState = currentState;
    }
}
