using System;
using UnityEngine;

public class CrafterSpawner : MonoBehaviour, IInteractable
{
    [SerializeField] private GrabbableObjectData _targetIngredient;
    
    [SerializeField] private GrabbableObjectData[] _ingredients;
    [SerializeField] private bool[] _isIngredientReceived;

    private int _receivedIngredientIndex = -1;

    private void Awake()
    {
        _isIngredientReceived = new bool[_ingredients.Length];
        for (int i = 0; i < _ingredients.Length; i++)
            _isIngredientReceived[i] = false;
        
    }
    
    public bool Interact(Interactor interactor)
    {
        if (!interactor.PlayerInteractionStatus.HasGrabbed ||
            !IsCorrectIngredient(interactor) ||
            IsIngredientAlreadyInserted(interactor))
        {
            RefuseIngredient();
            return false;
        }
        
        AddIngredient(interactor);

        return true;
    }

    private bool IsCorrectIngredient(Interactor interactor)
    {
        int objectDataId = interactor.PlayerInteractionStatus.ObjectData.Id;
        
        // Debug.Log("Checking object data with id: " + objectDataId);

        bool result = false;
        for (int i = 0; i < _ingredients.Length; i++)
        {
            // Debug.Log("Checking ingredient: " + _ingredients[i].Id);
            
            if (_ingredients[i].Id != objectDataId) continue;
            
            // Debug.Log("Matching with: " + _ingredients[i].Id);
            
            result = true;
            _receivedIngredientIndex = i;
            break;
        }
        
        return result;
    }

    private bool IsIngredientAlreadyInserted(Interactor interactor)
    {
       int objectDataId = interactor.PlayerInteractionStatus.ObjectData.Id;

       for (int i = 0; i < _ingredients.Length; i++)
       {
           if (_ingredients[i].Id == objectDataId && _isIngredientReceived[i]) return true;
       }
       
       return false;
    }

    private void RefuseIngredient()
    {
        Debug.Log("Refusing");
        _receivedIngredientIndex = -1;
    }

    private void AddIngredient(Interactor interactor)
    {
        // Debug.Log("Adding Ingredient");
        
        // Update ingredients status
        _isIngredientReceived[_receivedIngredientIndex] = true;
        
        // Force drop of Grabbable object
        interactor.PlayerInteractionStatus.GrabbableObject.ForceDropAndDestroy(interactor);

        if (!CheckIfIngredientListIsComplete()) return;
        
        // Recepie satisfied, generating target ingredient
        GameObject spawnedObject = 
            Instantiate(_targetIngredient.Object, interactor.PlayerInteractionStatus.GrabbedSpotPoint);
        
        GrabbableObject grabbableObject = spawnedObject.GetComponent<GrabbableObject>();
        grabbableObject.ForceGrab(interactor);

        ResetRecepie();
    }
    
    private bool CheckIfIngredientListIsComplete()
    {
        int counter = 0;
        for (int i = 0; i < _ingredients.Length; i++)
        {
            if (!_isIngredientReceived[i]) continue;
            counter++;
        }

        return counter == _ingredients.Length;
    }

    private void ResetRecepie()
    {
        for (int i = 0; i < _ingredients.Length; i++)
        {
            _isIngredientReceived[i] = false;
        }
    }
    
}
