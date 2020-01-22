using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Photon.Pun;
using Photon.Realtime;

public class AlienController : AlienMovement
{
    public Color alienVision;
    public PlayerInteraction alienInteraction;
    public Material transparentVent;
    private PlayerAttack alienAttack;
    private GameObject trackerGO;

    private bool triggeredEmergencyHealing = false;
    private bool usingEmergencyHealing = true;
    private PlayerHealth healthScript;
    public int emergencyHealingThreshold = 60;
    public int emergencyHealingAmount = 10;
    public int emergencyHealingTickCount = 10;
    private int emergencyHealingCurrentTickCount = 0;
    public float emergencyHealingTickDelay = 0.1f;
    private float emergencyHealingDeltaTime = 0.0f;

    private float deltaTime = 0;
    private new void Start()
    {
        base.Start();
        
        if (!photonView.IsMine)
        {
            return;
        }

        RenderSettings.ambientLight = alienVision;
        
        alienInteraction = new PlayerInteraction();
        alienAttack = GetComponent<PlayerAttack>();
        trackerGO = charCamera.transform.GetChild(0).gameObject;
        GameObject[] vents = GameObject.FindGameObjectsWithTag("Vent");
        Debug.Log(vents.Length);
        foreach (GameObject vent in vents)
        {
            vent.GetComponent<Renderer>().material = transparentVent;
        }

        healthScript = gameObject.GetComponent<PlayerAttack>().healthScript;
    }

    private new void Update()
    {
        // If we are not the local client then don't compute any of this.
        if (!photonView.IsMine) 
            return;

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
            if (triggeredEmergencyHealing)
            {
                emergencyHealingDeltaTime += Time.deltaTime;
                if (emergencyHealingDeltaTime > emergencyHealingTickDelay)
                {
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

    }

    private new void FixedUpdate()
    {
        // If we are not the local client then don't compute any of this.
        if (!photonView.IsMine) 
            return;
        base.FixedUpdate();
    }

    [PunRPC]
    protected void RegenHealth(int viewID, int healingAmount)
    {
        GameObject alien = PhotonView.Find(viewID).gameObject;
        alien.GetComponent<PlayerAttack>().healthScript.PlayerHit(healingAmount);
        alien.GetComponent<PlayerAttack>().healthSlider.fillAmount = alien.GetComponent<PlayerAttack>().healthScript.fillAmount;
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
        if (trackerGO.activeSelf)
        {
            StartCoroutine(FadeValue(result => vignette.intensity.value = result, vignette.intensity.value, 0.5f, 1f));
            StartCoroutine(FadeValue(result => lensDistortion.intensity.value = result, lensDistortion.intensity.value, 25, 1f));
        }
        else
        {
            StartCoroutine(FadeValue(result => vignette.intensity.value = result, vignette.intensity.value, 1, 1f));
            StartCoroutine(FadeValue(result => lensDistortion.intensity.value = result, lensDistortion.intensity.value, 80, 1f));
        }
        trackerGO.SetActive(!trackerGO.activeSelf);
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