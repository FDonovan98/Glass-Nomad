using UnityEngine;

[CreateAssetMenu(fileName = "New Armour", menuName = "Objects/Create New Armour")]

public class Armour : BaseObject
{
    [Tooltip("Integer modifier. Positive value will add that much max health, negative will remove max health.")]
    public int healthModifier;
    [Tooltip("Floating point modifier. A value of 1 is base speed. Values over 1 are faster. Values between 0 and 1 are slower than base speed.")]
    [Range(0.1f, 10f)]
    public float speedModifier;
}
