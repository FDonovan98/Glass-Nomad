using UnityEngine;

[CreateAssetMenu(fileName = "New Armour", menuName = "Objects/Create New Armour")]

public class Armour : BaseObject
{
    [Tooltip("Will cause player to take x% of damage.")]
    public float damageMultiplier;
    [Tooltip("Acts as a multiplier for walking and sprinting speed.")]
    public float speedMultiplier;
}
