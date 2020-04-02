using UnityEngine.UI;

public static class ReticleProgress
{
    public static void UpdateReticleProgress(float currProgress, Image outerReticle)
    {
        outerReticle.fillAmount = (currProgress / 100);
        if (currProgress >= 100)
        {
            outerReticle.fillAmount = 0;
        }
    }
}
