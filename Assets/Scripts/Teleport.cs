using UnityEngine;

public class Teleport : MonoBehaviour
{
   [SerializeField] private Transform gate;

   private void OnTriggerEnter(Collider other)
   {
      if (!other.CompareTag("Player")) return;
      
      var cc = other.transform.GetComponent<CharacterController>();
      
      if (cc) cc.enabled = false;
      other.gameObject.transform.position = new Vector3(
         gate.position.x,
         gate.position.y,
         gate.position.z
      );

      // other.gameObject.transform.forward = -other.gameObject.transform.forward;
      // other.gameObject.transform.Rotate(0f, 180f, 0f);
      
      if (cc) cc.enabled = true;
   }
}
