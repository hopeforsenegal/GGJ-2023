
using Spine.Unity;

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

    public static bool HasHitTimeOnce(ref float timeRemaining, float deltaTime)
    {
        // uses operator precedence in order to evaluate to true only once
        return timeRemaining > 0 && HasHitTime(ref timeRemaining, deltaTime);
    }

    public static bool HasHitTime(ref float timeRemaining, float deltaTime)
    {
        timeRemaining -= deltaTime;
        return timeRemaining <= 0;
    }

    public static void LoopAnimation(SkeletonAnimation spineAnimation, string clipName)
    {
        spineAnimation.Skeleton.SetSlotsToSetupPose(); // 2. Make sure it refreshes.
        spineAnimation.AnimationState.Apply(spineAnimation.Skeleton); // 3. Make sure the attachments from your currently playing animation are applied.
        var entry = spineAnimation.AnimationState.SetAnimation(0, clipName, true);
        entry.TimeScale = 1;
    }
}
