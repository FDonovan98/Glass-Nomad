using UnityEngine;

public class BaseObject : ScriptableObject
{
    public enum ItemType
    {
        Primary,
        Secondary,
        Armour,
        Material
    }

    public ItemType itemType;
}
