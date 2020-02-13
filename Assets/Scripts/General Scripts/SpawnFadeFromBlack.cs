using System.Collections;
using UnityEngine;

public static class SpawnFadeFromBlack
{
    public static void Fade(Color fromColor, Color toColor, float duration, MonoBehaviour instance)
    {
        instance.StartCoroutine(FadeScreen(fromColor, toColor, duration));

    }

    public static IEnumerator FadeScreen(Color fromColor, Color toColor, float duration)
    {
        for (float currTime = 0f; currTime < duration; currTime += Time.deltaTime)
        {
            RenderSettings.ambientLight = Color.Lerp(fromColor, toColor, currTime / duration);
            yield return null;
        }
    }
}
