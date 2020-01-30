using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Photon.Pun;
using Photon.Realtime;

public class AlienController : AlienMovement
{
    #region variables

    // Used to alter the colour of the alien's vision (red).
    public Color alienVision;

    public PlayerInteraction alienInteraction;

    // Used to change the material of all of the vents to make it
    // easier for the alien to see and orientate themselves.
    public Material transparentVent;

    // Used by the oxygen regen script.
    public PlayerAttack alienAttack;

    // Used to toggle the alien's tracker vision on and off.
    private GameObject trackerGO;

    // Used to keep track of whether the tracker is on or off.
    private bool isTrackerOn = false;

    // Used to determine whether the alien's current health is
    // less than the emergency health threshold.
    private bool triggeredEmergencyHealing = false;

    // Used to check against the emergency health threshold
    // and regen the alien's health.
    private PlayerHealth healthScript;

    // Used to determine how much health the alien should have
    // before the health regen kicks in.
    public int emergencyHealingThreshold = 60;

    // Used to determine how much health the alien should regen
    // every emergency tick count.
    public int emergencyHealingAmount = 10;

    // Used to track how often the alien regens when in the
    // emergency healing state.
    public int emergencyHealingTickCount = 10;

    // Used to keep track 
    private int emergencyHealingCurrentTickCount = 0;
    public float emergencyHealingTickDelay = 0.1f;
    private float emergencyHealingDeltaTime = 0.0f;

    // Used to slow the speed of the alien when in the
    // emergency state.
    public float emergencySpeedMultiplier = 1.0f;

    // Used by the interaction script.
    private float deltaTime = 0;

    #endregion

    private new void Start()
    {
        // Uses the Start method from its parent class (alien movement),
        // which inherits from player movement.
        base.Start();
        
        // If the local player is not the alien, then we don't
        // need to process anything below.
        if (!photonView.IsMine)
        {
            return;
        }

        // Changes the colour tint of the camera.
        RenderSettings.ambientLight = alienVision;
        
        // Variable assigning.
        alienInteraction = new PlayerInteraction();
        alienAttack = GetComponent<PlayerAttack>();
        healthScript = gameObject.GetComponent<PlayerAttack>().healthScript;
        trackerGO = charCamera.transform.GetChild(0).gameObject;

        // Changes the material of all the vents found in the map.
        GameObject[] vents = GameObject.FindGameObjectsWithTag("Vent");
        foreach (GameObject vent in vents)
        {
            vent.GetComponent<Renderer>().material = transparentVent;
        }
    }

    private new void Update()
    {
        // If we are not the local client then don't compute any of this.
        if (!photonView.IsMine) 
            return;

        // Activate the tracker if the 'F' is pressed.
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleTracker();
        }

        base.Update();

        if (Input.GetButton("Interact"))
        {
            deltaTime += Time.deltaTime;
            alienInteraction.ProcessTriggers(deltaTime, false);
        }

        if (Input.GetButtonUp("Interact"))
        {
            deltaTime = 0.0f;
        }

        if (emergencyHealingCurrentTickCount != emergencyHealingTickCount)
        {
            EmergencyHealth();
        }
        else
        {
            this.movementSpeed /= emergencySpeedMultiplier;
        }
    }
    
    private new void FixedUpdate()
    {
        // If we are not the local client then don't compute any of this.
        if (!photonView.IsMine) 
            return;
        base.FixedUpdate();
    }

    private void EmergencyHealth()
    {
        if (triggeredEmergencyHealing)
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
        else if (healthScript.currentHealth < emergencyHealingThreshold)
        {
            triggeredEmergencyHealing = true;
        }
    }

    [PunRPC]
    protected void RegenHealth(int viewID, int healingAmount)
    {
        GameObject alien = PhotonView.Find(viewID).gameObject;
        healthScript.PlayerHit(healingAmount);
        alien.GetComponent<PlayerAttack>().healthSlider.fillAmount = healthScript.fillAmount;
    }

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