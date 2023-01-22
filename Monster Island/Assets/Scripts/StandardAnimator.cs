
using System;
using Spine.Unity;
using UnityEngine;
using Object = UnityEngine.Object;

[DisallowMultipleComponent]
[RequireComponent(typeof(SkeletonAnimation))]
[RequireComponent(typeof(MeshRenderer))]
public class StandardAnimator : MonoBehaviour
{
    public float TimeScale
    {
        get => m_SkeletonAnimation.AnimationState.TimeScale;
        set => m_TimeScale = value;
    }

    public string Skin
    {
        get => m_SkeletonAnimation.Skeleton.Skin.Name;
        set
        {
            m_SkeletonAnimation.initialSkinName = value;
            m_SkeletonAnimation.Skeleton.SetSkin(value);
        }
    }

    public string CurrentAnimation => m_CurrentAnimation;

    private SkeletonAnimation m_SkeletonAnimation;
    private MeshRenderer m_MeshRenderer;
    private string m_CurrentAnimation;
    private float m_TimeScale = 1;

    protected void Awake()
    {
        m_SkeletonAnimation = GetComponent<SkeletonAnimation>();
        m_MeshRenderer = GetComponent<MeshRenderer>();

        Debug.Assert(m_MeshRenderer != null);
        Debug.Assert(m_SkeletonAnimation != null);
    }

    public StandardAnimatorDesc Play(string skin, string play)
    {
        //Debug.Log($"[StandardAnimator::Play] skin:'{skin}' play:'{play}'");

        if (!string.IsNullOrWhiteSpace(skin)) {
            Skin = skin;
        }
        var desc = new StandardAnimatorDesc();
        m_SkeletonAnimation.Skeleton.SetSlotsToSetupPose(); // 2. Make sure it refreshes.
        m_SkeletonAnimation.AnimationState.Apply(m_SkeletonAnimation.Skeleton); // 3. Make sure the attachments from your currently playing animation are applied.
        m_CurrentAnimation = play;
        var entry = m_SkeletonAnimation.AnimationState.SetAnimation(0, m_CurrentAnimation, false);
        entry.TimeScale = m_TimeScale;
        entry.Complete += _ =>
        {
            desc.FireAnimationEnd();
            m_CurrentAnimation = string.Empty;
        };
        m_SkeletonAnimation.AnimationState.TimeScale = m_TimeScale;
        m_SkeletonAnimation.AnimationState.Update(0);
        m_SkeletonAnimation.Update(0); // Do we lose interpolation when we want with this?
        return desc;
    }

    public void Loop(string skin, string loop)
    {
        if (m_CurrentAnimation != loop) {
            //Debug.Log($"[StandardAnimator::Loop] skin:'{skin}' loop:'{loop}'");

            if (!string.IsNullOrWhiteSpace(skin)) {
                Skin = skin;
            }
            m_SkeletonAnimation.Skeleton.SetSlotsToSetupPose(); // 2. Make sure it refreshes.
            m_SkeletonAnimation.AnimationState.Apply(m_SkeletonAnimation.Skeleton); // 3. Make sure the attachments from your currently playing animation are applied.
            m_CurrentAnimation = loop;
            var entry = m_SkeletonAnimation.AnimationState.SetAnimation(0, m_CurrentAnimation, true);
            entry.TimeScale = m_TimeScale;
            m_SkeletonAnimation.AnimationState.TimeScale = m_TimeScale;
        }
    }

    public void Still(string still, string skin = "")
    {
        //Debug.LogFormat("[StandardAnimator::Still] skin:'{0}' still:'{1}'", skin, still);

        if (!string.IsNullOrWhiteSpace(skin)) {
            Skin = skin;
        }
        m_SkeletonAnimation.Skeleton.SetSlotsToSetupPose(); // 2. Make sure it refreshes.
        m_SkeletonAnimation.AnimationState.Apply(m_SkeletonAnimation.Skeleton); // 3. Make sure the attachments from your currently playing animation are applied.
        var entry = m_SkeletonAnimation.AnimationState.SetAnimation(0, still, true);
        entry.TimeScale = 0;
        m_SkeletonAnimation.AnimationState.TimeScale = 1;
    }

    // Animation not clearing properly? Could be the mix duration.
    public void Clear(bool doHardClear = true)
    {
        //Debug.LogFormat("[StandardAnimator::Clear]");

        m_CurrentAnimation = string.Empty;
        if (doHardClear) {
            m_SkeletonAnimation.Initialize(true);
            m_SkeletonAnimation.AnimationState.SetEmptyAnimations(0);
            m_SkeletonAnimation.AnimationState.Update(0);
            m_SkeletonAnimation.AnimationState.TimeScale = 1;
        }
    }

    public static StandardAnimator GetComponentOrAssert(Object context, GameObject gameObject)
    {
        var component = gameObject.GetComponent<StandardAnimator>();

        Debug.Assert(component != null, context);

        return component;
    }
}