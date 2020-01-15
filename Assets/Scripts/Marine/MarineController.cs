using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarineController : MarineMovement
{
    public PlayerInteraction playerInteraction;

    float deltaTime = 0;
    private new void Start()
    {
        base.Start();
        
        if (!photonView.IsMine)
        {
            return;
        }
        
        playerInteraction = new PlayerInteraction();
    }

    private new void Update()
    {
        // If we are not the local client then don't compute any of this.
        if (!photonView.IsMine) 
            return;

        base.Update();

        if (Input.GetButton("Interact"))
        {
            deltaTime += Time.deltaTime;
            playerInteraction.ProcessTriggers(deltaTime, false);
        }

        if (Input.GetButtonUp("Interact"))
        {
            deltaTime = 0.0f;
        }
    }
}
