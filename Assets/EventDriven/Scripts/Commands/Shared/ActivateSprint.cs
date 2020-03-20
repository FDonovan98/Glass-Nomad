using UnityEngine;

[CreateAssetMenu(fileName = "DefaultActivateSprint", menuName = "Commands/Activate Sprint")]
public class ActivateSprint : CommandObject
{
    [SerializeField]
    private KeyCode sprintKeyCode = KeyCode.LeftShift;

    protected override void OnEnable()
    {
        keyTable.Add("Sprint", sprintKeyCode);
    }

    public override void Execute(GameObject agent, MovementValues movementValues)
    {
        if (movementValues.sprintingIsAToggle)
        {
            if (Input.GetKeyDown(sprintKeyCode))
            {
                movementValues.isSprinting = !movementValues.isSprinting;
            }
        }
        else
        {
            if (Input.GetKeyDown(sprintKeyCode))
            {
                movementValues.isSprinting = true;
            }

            if (Input.GetKeyUp(sprintKeyCode))
            {
                movementValues.isSprinting = false;
            }
        }


    }
}
