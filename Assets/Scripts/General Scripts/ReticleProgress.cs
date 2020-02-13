using UnityEngine;
using UnityEngine.UI;

public static class ReticleProgress
{
    public static void UpdateReticleProgress(float currProgress, Image outerReticle)
    {
        outerReticle.fillAmount = (currProgress / 100);
        // Debug.Log("CURR PROGRESS: " + outerReticle.fillAmount);
        if (currProgress >= 100)
        {
            outerReticle.fillAmount = 0;
        }
    }
}
