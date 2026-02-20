using UnityEngine;

[CreateAssetMenu(fileName = "FlowerImages", menuName = "Scriptable Objects/FlowerImages")]
public class FlowerImages : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public FlowerState flowerState;
        public Sprite sprite;
    }

    public Entry[] entries;
}