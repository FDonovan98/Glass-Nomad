using UnityEngine;

public class OxygenRegenScript : MonoBehaviour
{
    public int PercentageOxygenRegenPerSecond = 10;

    private void OnTriggerStay(Collider other)
    {
        // Removed functionality as it is now handled by Harry's new system (see 'ManageOxygen' for more info)
    }
}
