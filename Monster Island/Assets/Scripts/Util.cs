using UnityEngine;

public static class Util
{
    // Simply switches on every call
    public static bool Switch(ref bool @switch)
    {
        @switch = !@switch;
        return @switch;
    }

    // Increments on every call than start back at zero
    public static int IncrementLoop(ref int val, int max)
    {
        val++;
        try {
            return val;
        }
        finally {
            if (val == max) {
                val = 0;
            }
        }
    }

    public static bool Toggle(CanvasGroup canvasGroup, bool enabled)
    {
        canvasGroup.alpha = enabled ? 1f : 0f;
        canvasGroup.interactable = enabled;
        canvasGroup.blocksRaycasts = enabled;
        return enabled;
    }

    public static bool HasHitTimeOnce(ref float timeRemaining, float deltaTime)
    {
        // uses operator precedence in order to evaluate to true only once
        return timeRemaining > 0 && HasHitTime(ref timeRemaining, deltaTime);
    }

    private static bool HasHitTime(ref float timeRemaining, float deltaTime)
    {
        timeRemaining -= deltaTime;
        return timeRemaining <= 0;
    }
}
