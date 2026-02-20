using UnityEngine;

[CreateAssetMenu(fileName = "LarvaImages", menuName = "Scriptable Objects/LarvaImages")]
public class LarvaImages : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public LarvaSituation larvaSituation;
        public Sprite sprite;
    }
    
    public Entry[] entries;
}
