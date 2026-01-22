using UnityEngine;


public enum ItemType
{
    PowerUp,
    Points,
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public int value;


    
}
