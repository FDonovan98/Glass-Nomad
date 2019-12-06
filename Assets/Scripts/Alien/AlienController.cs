using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : AlienMovement
{
    private new void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        // If we are not the local client then don't compute any of this.
        if (!photonView.IsMine) 
            return;
        base.Update();
    }

    private new void FixedUpdate()
    {
        // If we are not the local client then don't compute any of this.
        if (!photonView.IsMine) 
            return;
        base.FixedUpdate();
    }
}