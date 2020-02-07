using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Photon.Pun;

public class AlienController : AlienMovement
{
    #region variable-declaration

    // Alters the colour of the alien's vision (red).
    public Color alienVision;

    // Change the material of all of the vents to make it
    // easier for the alien to see and orientate themselves.
    public Material transparentVent;

    // Used by the oxygen regen script.
    public PlayerAttack alienAttack;

    // Toggles the alien's tracker vision on and off.
    private GameObject trackerGO;

    // Whether the tracker is on or off.
    private bool isTrackerOn = false;

    // Whether the alien's current health is less than the emergency health threshold.
    private bool triggeredEmergencyHealing = false;

    private PlayerResources resourcesScript;

    // How much health the alien should have before the health regen kicks in.
    public int emergencyHealingThreshold = 60;

    // How much health the alien should regen every emergency tick count.
    public int emergencyHealingAmount = 10;

    // How often the alien regens when in the emergency healing state.
    public int emergencyHealingTickCount = 10;

    // Used to keep track 
    private int emergencyHealingCurrentTickCount = 0;
    public float emergencyHealingTickDelay = 0.1f;
    private float emergencyHealingDeltaTime = 0.0f;

    // Slows the speed of the alien when in the emergency state.
    public float emergencySpeedMultiplier = 1.0f;

    #endregion

    /// <summary>
    /// Assigns the variables and makes the vents transparent.
    /// </summary>
    private new void Start()
    {
        base.Start();
        
        if (!photonView.IsMine) return;

        SpawnFadeFromBlack.Fade(Color.black, alienVision, 3, this);
        
        alienAttack = GetComponent<PlayerAttack>();
        trackerGO = charCamera.transform.GetChild(0).gameObject;

        // Changes the material of all the vents found in the map.
        GameObject[] vents = GameObject.FindGameObjectsWithTag("Vent");
        foreach (GameObject vent in vents)
        {
            vent.GetComponent<Renderer>().material = transparentVent;
        }

        resourcesScript = gameObject.GetComponent<PlayerAttack>().resourcesScript;
    }

    /// <summary>
    /// Handles the tracker input, player interaction and emergency healing.
    /// </summary>
    private new void Update()
    {
        // If we are not the local client then don't compute any of this.
        if (!photonView.IsMine) return;

        base.Update();
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleTracker();
        }

        if (emergencyHealingCurrentTickCount != emergencyHealingTickCount)
        {
            if (triggeredEmergencyHealing)
            {
                EmergencyHealing();
            }
            else if (resourcesScript.currentHealth < emergencyHealingThreshold)
            {
                triggeredEmergencyHealing = true;
            }
        }
        else
        {
            movementSpeed /= emergencySpeedMultiplier;
        }
    }

    /// <summary>
    /// The method that runs if the alien is emergency healing.
    /// </summary>
    private void EmergencyHealing()
    {
        emergencyHealingDeltaTime += Time.deltaTime;
        if (emergencyHealingDeltaTime > emergencyHealingTickDelay)
        {
            this.movementSpeed *= emergencySpeedMultiplier;
            Debug.LogWarning("check THREE");
            PhotonView photonView = gameObject.GetPhotonView();
            int viewID = photonView.ViewID;
            photonView.RPC("RegenHealth", RpcTarget.All, viewID, -emergencyHealingAmount);
            emergencyHealingDeltaTime = 0;
            emergencyHealingCurrentTickCount++;
        }
    }

    /// <summary>
    /// Passes a negative number to the player damage health script method, so that the player
    /// is healed instead. Uses a PunRPC so that the alien is healed on all clients.
    /// </summary>
    /// <param name="viewID"></param>
    /// <param name="healingAmount"></param>
    [PunRPC]
    protected void RegenHealth(int viewID, int healingAmount)
    {
        GameObject alien = PhotonView.Find(viewID).gameObject;
        alien.GetComponent<PlayerAttack>().resourcesScript.UpdatePlayerResource(PlayerResources.PlayerResource.Health, healingAmount);
        alien.GetComponent<PlayerAttack>().healthSlider.fillAmount = alien.GetComponent<PlayerAttack>().resourcesScript.fillAmount;
    }

    /// <summary>
    /// Changes the lens distortion and vignette intensity of the alien's camera, when their
    /// tracker vision is toggled.
    /// </summary>
    private void ToggleTracker()
    {
        PostProcessVolume ppVolume = GetComponentInChildren<PostProcessVolume>();
        Vignette vignette;
        LensDistortion lensDistortion;
        ppVolume.profile.TryGetSettings(out vignette);
        ppVolume.profile.TryGetSettings(out lensDistortion);

        // When in normal vision, vignette intensity = 0.5, lens distortion = 25
        // When in tracker vision, vignette intensity = 1, lens distortion = 80
        if (isTrackerOn)
        {
            StartCoroutine(FadeValue(result => vignette.intensity.value = result, vignette.intensity.value, 0.5f, 1f));
            StartCoroutine(FadeValue(result => lensDistortion.intensity.value = result, lensDistortion.intensity.value, 25, 1f));
        }
        else
        {
            StartCoroutine(FadeValue(result => vignette.intensity.value = result, vignette.intensity.value, 1, 1f));
            StartCoroutine(FadeValue(result => lensDistortion.intensity.value = result, lensDistortion.intensity.value, 80, 1f));
        }
        
        isTrackerOn = !isTrackerOn;
        trackerGO.SetActive(isTrackerOn);
    }

    /// <summary>
    /// Transitions one value to another, over a certain amount of time. In this case,
    /// it is used to fade the vignette and lens distortion for the tracker vision.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="startingValue"></param>
    /// <param name="endValue"></param>
    /// <param name="fadeDuration"></param>
    /// <returns></returns>
    IEnumerator FadeValue(Action<float> value, float startingValue, float endValue, float fadeDuration)
    {
        float diff = endValue - startingValue;
        float step = diff / fadeDuration;

        for (float t = 0f; t < fadeDuration - Time.deltaTime; t += Time.deltaTime)
        {
            startingValue += step * Time.deltaTime;
            value(startingValue);
            yield return null;
        }
        value(endValue);
    }
}