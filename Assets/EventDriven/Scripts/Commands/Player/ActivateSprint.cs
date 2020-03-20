using UnityEngine;

[CreateAssetMenu(fileName = "DefaultActivateSprint", menuName = "Commands/Activate Sprint")]
public class ActivateSprint : CommandObject
{
    public MovementValues movementValues;
    public override void Execute(GameObject agent, MovementValues movementValues)
    {
        if (movementValues.sprintingIsAToggle)
        {
            if (Input.GetKeyDown(keycode))
            {
                movementValues.isSprinting = !movementValues.isSprinting;
            }
        }
        else
        {
            if (Input.GetKeyDown(keycode))
            {
                movementValues.isSprinting = true;
            }

            if (Input.GetKeyUp(keycode))
            {
                movementValues.isSprinting = false;
            }
        }
        
        
    }
}
