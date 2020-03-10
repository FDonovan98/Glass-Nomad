using UnityEngine;

public class GeneratorObjective : TriggerInteractionScript
{
    protected override void InteractionComplete(GameObject player)
    {
        base.InteractionComplete(player);

        if (!Objectives.IsObjectiveComplete(objectiveName)) return;

        // Unlocks everything requiring power.
        GameObject[] powered = GameObject.FindGameObjectsWithTag("NeedsPower");
        foreach (GameObject element in powered)
        {
            if (element.GetComponent<Teleporter>() != null)
            {
                element.GetComponent<Teleporter>().powered = true;
            }
        }
    }
}
