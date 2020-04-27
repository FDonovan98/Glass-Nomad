using UnityEngine;

[CreateAssetMenu(fileName = "New Armour", menuName = "Objects/Create New Armour")]

public class Armour : BaseObject
{
    [Tooltip("Will cause player to take x% of damage.")]
    public int healthModifier;
    [Tooltip("Acts as a multiplier for walking and sprinting speed.")]
    [Range(0.1f, 10f)]
    public float speedModifier;
}
