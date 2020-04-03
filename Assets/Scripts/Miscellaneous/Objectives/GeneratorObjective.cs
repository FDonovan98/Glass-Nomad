using UnityEngine;

public class GeneratorObjective : TriggerInteractionScript
{
    /// <summary>
    /// If the objective is successfully marked as completed, then it unlocks
    /// everything requiring power.
    /// </summary>
    /// <param name="player"></param>
    protected override void InteractionComplete(GameObject player)
    {
        base.InteractionComplete(player);

        if (!Objectives.IsObjectiveComplete(objectiveName)) return;

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
