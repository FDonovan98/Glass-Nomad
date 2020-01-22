using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Photon.Pun;

public class AlienController : AlienMovement
{
    public Color alienVision;
    public PlayerInteraction alienInteraction;
    private PlayerAttack alienAttack;
    private GameObject trackerGO;

    float deltaTime = 0;
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
    }

    private new void FixedUpdate()
    {
        // If we are not the local client then don't compute any of this.
        if (!photonView.IsMine) 
            return;
        base.FixedUpdate();
    }

    [PunRPC]
    protected void RegenHealth(int viewID, float deltaTime)
    {
        GameObject alien = PhotonView.Find(viewID).gameObject;
        alien.GetComponent<PlayerAttack>().healthScript.PlayerHit(-1);
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