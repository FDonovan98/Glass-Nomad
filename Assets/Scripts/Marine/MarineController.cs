using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarineController : MarineMovement
{
    public PlayerInteraction marineInteraction;
    private PlayerAttack marineAttack;

    float deltaTime = 0;
    private new void Start()
    {
        base.Start();
        
        if (!photonView.IsMine)
        {
            return;
        }
        
        marineInteraction = new PlayerInteraction();
        marineAttack = GetComponent<PlayerAttack>();

        // Starts the user with a rifle.
        //marineAttack.weaponDict.TryGetValue("default", out marineAttack.currentWeapon);        
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
            marineInteraction.ProcessTriggers(deltaTime, true);
        }

        if (Input.GetButtonUp("Interact"))
        {
            deltaTime = 0.0f;
        }
    }
}
