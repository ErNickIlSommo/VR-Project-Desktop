using UnityEngine;

public class Exit : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; 
        
        Debug.Log("Player Used Entered in exit");
    }
}
