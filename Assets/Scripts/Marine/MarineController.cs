using UnityEngine;

public class MarineController : MarineMovement
{
    // Accessed by the oxygen regen script.
    public PlayerAttack marineAttack;

    private float oxygenDamageTime = 0f;

    private Light flashlight;

    private float movementAngle;
    private float animSpeed;


    private new void Start()
    {
        base.Start();
        
        if (!photonView.IsMine) return;

        SpawnFadeFromBlack.Fade(Color.black, Color.clear, 3, this);

        marineAttack = GetComponent<PlayerAttack>();

        flashlight = GetComponentInChildren<Light>();
    }

    private new void Update()
    {
        // If we are not the local client then don't compute any of this.
        if (!photonView.IsMine) return;

        base.Update();

        if (marineAttack.resourcesScript.oxygenAmountSeconds == 0)
        {
            if (oxygenDamageTime >= 0.2f)
            {
                marineAttack.resourcesScript.UpdatePlayerResource(PlayerResources.PlayerResource.Health, -1);
                oxygenDamageTime = 0f;
            }
            else
            {
                oxygenDamageTime += Time.fixedDeltaTime;
            }
        }
        
        if (!inputEnabled || Cursor.lockState == CursorLockMode.None) return;

        if (Input.GetButtonDown("Flashlight"))
        {
            flashlight.enabled = !flashlight.enabled;
        }

        movementAngle = Vector3.SignedAngle(charRigidbody.velocity, transform.forward, Vector3.down);
        anim.SetFloat("runningDirection", movementAngle);
        animSpeed = charRigidbody.velocity.magnitude;
        anim.SetFloat("speed", animSpeed);
    }
}
