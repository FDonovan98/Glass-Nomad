using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarineController : MarineMovement
{
    public PlayerInteraction marineInteraction;
    private PlayerAttack marineAttack;

    float deltaTime = 0;

    public float maxOxygenAmountSeconds = 300f;
    public float oxygenAmountSeconds;
    private float oxygenDamageTime = 0f;

    private new void Start()
    {
        base.Start();
        
        if (!photonView.IsMine)
        {
            return;
        }
        
        marineInteraction = new PlayerInteraction();
        marineAttack = GetComponent<PlayerAttack>();
        oxygenAmountSeconds = maxOxygenAmountSeconds;
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

        if (oxygenAmountSeconds > 0)
        {
            oxygenAmountSeconds -= Time.deltaTime;
        }
        if (oxygenAmountSeconds == 0)
        {
            if (oxygenDamageTime >= 0.2f)
            {
                gameObject.GetComponent<PlayerAttack>().healthScript.PlayerHit(1);
                oxygenDamageTime = 0f;
            }
            else
            {
                oxygenDamageTime += Time.deltaTime;
            }
        }
    }
}
