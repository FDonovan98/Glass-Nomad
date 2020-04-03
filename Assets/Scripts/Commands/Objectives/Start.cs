using UnityEngine;

[CreateAssetMenu(fileName = "Start", menuName = "Commands/Objectives/Start")]
public class Start : BaseObjective
{
    protected override void RunCommandOnStart(ObjectiveHandler objectiveHandler)
    {
        objectiveHandler.runCommandOnCompleted += RunCommandOnCompleted();
    }

    protected override void RunCommandOnTriggerEnter()
    {

    }

    protected override void RunCommandOnTriggerStay()
    {

    }

    protected override void RunCommandOnTriggerExit()
    {

    }

    protected override void RunCommandOnCompleted()
    {
        
    }
}