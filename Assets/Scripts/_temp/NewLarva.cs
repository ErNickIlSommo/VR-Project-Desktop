using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewLarva: MonoBehaviour, IInteractable
{
    [SerializeField] private EntityUI ui;
    
    private bool _canInteract = false;
    private GrabbableObjectData _requestedFood;

    private LarvaAnimationController animationController;
    
    public enum RequestStatus
    {
        STARTED,
        CORRECT,
        WRONG
    }
    
    private RequestStatus _requestStatus;
    public RequestStatus Status => _requestStatus;

    private void Awake()
    {
        ui = GetComponent<EntityUI>();
        animationController = GetComponent<LarvaAnimationController>();
    }

    private void Start()
    {
        
    }

    public void EnableInteraction()
    {
        _canInteract = true;
    }

    public void DisableInteraction()
    {
        _canInteract = false;
    }

    public bool Interact(Interactor interactor)
    {
        if (!_canInteract) return false;
        if (!interactor.PlayerInteractionStatus.HasGrabbed) return false;
        if (!interactor.PlayerInteractionStatus.ObjectData) return false;

        Debug.Log("Larva " + gameObject.name + " Interaction");
        
        int objectDataId = interactor.PlayerInteractionStatus.ObjectData.Id;
        
        //BlockMovement(interactor);
        interactor.PlayerInteractionStatus.GrabbableObject.ForceDropAndDestroy(interactor);
        //UnlockMovement(interactor);
        
        if (_requestedFood.Id == objectDataId)
        {
            _requestStatus = RequestStatus.CORRECT;
            // Update UI
            ui.UpdateImage(LarvaSituation.Correct);
            animationController.AcceptFood();
            return true;
        }

        _requestStatus = RequestStatus.WRONG;
        // Update UI
        ui.UpdateImage(LarvaSituation.Wrong);
        animationController.RefuseFood();
        _canInteract = false;
        return true;
    }

    public IEnumerator SendFoodRequest(float cooldown, GrabbableObjectData food)
    {
        Debug.Log("Larva request food: " + gameObject.name + " food: " + food.Name);
        _requestedFood = food;
        _requestStatus = RequestStatus.STARTED;
        
        // Update UI
        if (_requestedFood.Id == 0) ui.UpdateImage(LarvaSituation.RoyalJelly);
        if (_requestedFood.Id == 1) ui.UpdateImage(LarvaSituation.Beebread);
        if (_requestedFood.Id == 5) ui.UpdateImage(LarvaSituation.Water);
        
        ui.Show();
        animationController.RequestFood();

        float timer = 0f;
        while (timer < cooldown)
        {
            if (_requestStatus != RequestStatus.STARTED) break; 
            
            timer += Time.deltaTime;
            yield return null;
        }
        
        if (_requestStatus == RequestStatus.STARTED)
        {
            _requestStatus = RequestStatus.WRONG;
            ui.UpdateImage(LarvaSituation.Wrong);
            animationController.RefuseFood();

        }
        yield return new WaitForSeconds(2f); 
        ui.Hide();
    }

    public void BlockMovement(Interactor interactor)
    {
        InputActionMap map = interactor.InputActions.FindActionMap("Controls", true);
        map.FindAction("Move", true).Disable();  
    }
    public void UnlockMovement(Interactor interactor)
    {
        InputActionMap map = interactor.InputActions.FindActionMap("Controls", true);
        map.FindAction("Move", true).Enable();  
    }
}