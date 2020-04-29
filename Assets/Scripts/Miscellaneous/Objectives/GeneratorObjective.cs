using UnityEngine;

public class GeneratorObjective : ObjectiveInteraction
{
    /// <summary>
    /// If the objective is successfully marked as completed, then it unlocks
    /// everything requiring power.
    /// </summary>
    /// <param name="player"></param>
    protected override void ObjectiveComplete()
    {
        GameObject[] powered = GameObject.FindGameObjectsWithTag("NeedsPower");
        foreach (GameObject element in powered)
        {
            if (element.GetComponent<Teleporter>() != null)
            {
                element.GetComponent<Teleporter>().Power();
            }
        }
    }
}
