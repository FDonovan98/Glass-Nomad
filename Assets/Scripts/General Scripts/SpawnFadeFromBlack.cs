using System.Collections;
using UnityEngine;

/// <summary>
/// SpawnFadeFromBlack is used to gradually transition the colour of the screen from one colour to another.
/// It is a static class so that it can be accessed from anywhere without a reference.
/// </summary>
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
