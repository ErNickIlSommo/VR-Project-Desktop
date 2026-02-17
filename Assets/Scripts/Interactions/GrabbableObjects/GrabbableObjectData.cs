using UnityEngine;

[CreateAssetMenu(fileName = "GrabbableObjectData", menuName = "Scriptable Objects/GrabbableObjectData")]
public class GrabbableObjectData : ScriptableObject
{
    [SerializeField] private int id;
    public int Id => id;

    [SerializeField] private string _name;
    public string Name => _name;
    
    [SerializeField] private float _cooldown;
    public float Cooldown => _cooldown;
    
    [SerializeField] private GameObject _object;
    public GameObject Object => _object;
}
