using UnityEngine;

public class MarineController : MarineMovement
{
    // Redundant now?
    public PlayerInteraction marineInteraction;
    private float deltaTime = 0;

    // Accessed by the oxygen regen script.
    public PlayerAttack marineAttack;

    private new void Start()
    {
        base.Start();
        
        if (!photonView.IsMine) { return; }
        
        marineInteraction = new PlayerInteraction();
        marineAttack = GetComponent<PlayerAttack>();
    }

    private new void Update()
    {
        // If we are not the local client then don't compute any of this.
        if (!photonView.IsMine) { return; }

        base.Update();
        if (!inputEnabled) { return; }

        // Redundant now?
        if (Input.GetButton("Interact"))
        {
            deltaTime += Time.deltaTime;
            marineInteraction.ProcessTriggers(deltaTime, true);
        }

        if (Input.GetButtonUp("Interact"))
        {
            deltaTime = 0.0f;
        }
    }
}
