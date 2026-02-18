using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class InteractorTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableMask;

    private readonly HashSet<IInteractable> _detectedInteractables = new HashSet<IInteractable>();

    private void OnTriggerEnter(Collider other)
    {

        if ((_interactableMask & (1 << other.gameObject.layer)) == 0) return;

        var interactable = other.GetComponent<IInteractable>();
        if (interactable == null) return;

        _detectedInteractables.Add(interactable);

    }

    private void OnTriggerExit(Collider other)
    {
        if ((_interactableMask & (1 << other.gameObject.layer)) == 0) return;
        var interactable = other.GetComponent<IInteractable>();
        if (interactable == null) return;

        _detectedInteractables.Remove(interactable);

    }

    public void RemoveInteractable(IInteractable interactable)
    {
        _detectedInteractables.Remove(interactable);
    }

    public IInteractable GetInteractable()
    {
        foreach (IInteractable interactable in _detectedInteractables)
        {
            Debug.Log("Si");
            return interactable;
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(_interactionTrigger.position, _interactionDistance);
    }
}
