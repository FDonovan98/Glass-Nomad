using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : AlienMovement
{
    public Color alienVision;
    public PlayerInteraction playerInteraction;

    float deltaTime = 0;
    private new void Start()
    {
        base.Start();
        
        if (!photonView.IsMine)
        {
            return;
        }

        RenderSettings.ambientLight = alienVision;
        
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
    }

    private new void FixedUpdate()
    {
        // If we are not the local client then don't compute any of this.
        if (!photonView.IsMine) 
            return;
        base.FixedUpdate();
    }
}