using UnityEngine;

public class Teleport : MonoBehaviour
{
   [SerializeField] private Transform gate;

   private void OnTriggerEnter(Collider other)
   {
      if (!other.CompareTag("Player")) return;
      
        Transform root = other.transform.root; // <-- importante
        var cc = root.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        root.SetPositionAndRotation(gate.position, gate.rotation);

        if (cc) cc.enabled = true;
   }
}
