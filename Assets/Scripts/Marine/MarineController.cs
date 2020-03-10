using UnityEngine;

public class MarineController : MarineMovement
{
    // Accessed by the oxygen regen script.
    public PlayerAttack marineAttack;

    private float oxygenDamageTime = 0f;

    private Light flashlight;


<<<<<<< HEAD
    float deltaTime = 0;

    public float maxOxygenAmountSeconds = 300f;
    public float oxygenAmountSeconds;
    private float oxygenDamageTime = 0f;

=======
>>>>>>> master
    private new void Start()
    {
        base.Start();
        
        if (!photonView.IsMine) return;

        SpawnFadeFromBlack.Fade(Color.black, Color.clear, 3, this);

        marineAttack = GetComponent<PlayerAttack>();
<<<<<<< HEAD
        oxygenAmountSeconds = maxOxygenAmountSeconds;
=======

        flashlight = GetComponentInChildren<Light>();
>>>>>>> master
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
