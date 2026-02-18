using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Abyss : MonoBehaviour
{
    public event Action<bool> OnCorpseEntered;
    
    [SerializeField] private Transform playerSpawningPoint;
    [SerializeField] private Fader fader;
    private bool _busy;

    private Collider _playerCollider;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerCollider = other;
            StartCoroutine(TeleportSequence());
        }
        if (other.CompareTag("Grabbable"))
        {
            // Destroy Object
            GrabbableObjectData objectData = other.gameObject.GetComponent<GrabbableObject>().ObjectData;
            
            if (!objectData) return;
            // Debug.Log("Entered: " + objectData.Name);
            
            Destroy(other.transform.gameObject);
            
            if(OnCorpseEntered != null && objectData.Id == 0) OnCorpseEntered.Invoke(true);
        }
    }

    private IEnumerator TeleportSequence()
    {
        _busy = true;

        // Fade to black (Metallica - Right the Lighting)
        yield return fader.FadeOut();
        // yield return fader.FadeIn();
        
        // Teleport Player
        var cc = _playerCollider.transform.GetComponent<CharacterController>();
        if(cc) cc.enabled = false;
        _playerCollider.gameObject.transform.position = new Vector3(
            playerSpawningPoint.position.x,
            playerSpawningPoint.position.y,
            playerSpawningPoint.position.z
        );
        if(cc) cc.enabled = true; 
        
        // Wait 1 frame
        yield return null;
        
        // Fade Black
        yield return fader.FadeIn();
        // yield return fader.FadeOut();
        
        _busy = false;
    }
    
}
