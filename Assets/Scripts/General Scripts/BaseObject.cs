using UnityEngine;

public class BaseObject : ScriptableObject
{
    public enum ItemType
    {
        Primary,
        Secondary,
        Armour
    }

    public ItemType itemType;
}
