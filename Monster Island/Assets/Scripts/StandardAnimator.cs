using System;
using UnityEngine;
using Spine;
using Spine.Unity;

[DisallowMultipleComponent]
[RequireComponent(typeof(SkeletonAnimation))]
[RequireComponent(typeof(MeshRenderer))]
public class StandardAnimator : MonoBehaviour
{
    #region Enums and Constants

    #endregion

    #region Events

    #endregion

    #region Properties

    #endregion

    #region Inspectables

    #endregion

    #region Private Member Variables

    private SkeletonAnimation m_SkeletonAnimation;
    private MeshRenderer m_MeshRenderer;
    private Action m_OnEndEvent;
    private string m_LoopAnimation;
    private string m_CurrentAnimation;

    #endregion

    #region Monobehaviours

    protected void Awake()
    {
        m_SkeletonAnimation = GetComponent<SkeletonAnimation>();
        m_MeshRenderer = GetComponent<MeshRenderer>();

        Debug.Assert(m_MeshRenderer != null);
        Debug.Assert(m_SkeletonAnimation != null);
    }

    #endregion

    #region Public Methods

    public static StandardAnimator AddStandardAnimatorComponent(GameObject gameObject, SkeletonDataAsset skeletonDataAsset)
    {
        var skeleton = SkeletonAnimation.AddToGameObject(gameObject, skeletonDataAsset);
        var animator = skeleton.gameObject.AddComponent<StandardAnimator>();
        return animator;
    }

    public void Play(string skin, string introOrDefault, Action endAnimationEvent = null)
    {
        Clear();
        //Debug.LogFormat("[StandardAnimator::Play] skin:'{0}' introOrDefault:'{1}' loop:'{2}'", skin, introOrDefault, loop);

        SetSkin(skin);
        m_SkeletonAnimation.Skeleton.SetSlotsToSetupPose(); // 2. Make sure it refreshes.
        m_SkeletonAnimation.AnimationState.Apply(m_SkeletonAnimation.Skeleton); // 3. Make sure the attachments from your currently playing animation are applied.
        m_SkeletonAnimation.AnimationState.TimeScale = 1;
        m_MeshRenderer.enabled = true;

        var entry = m_SkeletonAnimation.AnimationState.SetAnimation(0, introOrDefault, false);
        entry.TimeScale = 1;
        entry.Complete += _ =>
        {
            endAnimationEvent?.Invoke();
        };
    }

    public void Loop(string skin, string loop)
    {
        m_MeshRenderer.enabled = true;
        if (m_CurrentAnimation != loop) {
            //Debug.LogFormat("[StandardAnimator::Loop] skin:'{0}' loop:'{1}'", skin, loop);
            m_CurrentAnimation = loop;

            SetSkin(skin);
            m_SkeletonAnimation.Skeleton.SetSlotsToSetupPose(); // 2. Make sure it refreshes.
            m_SkeletonAnimation.AnimationState.Apply(m_SkeletonAnimation.Skeleton); // 3. Make sure the attachments from your currently playing animation are applied.
            var entry = m_SkeletonAnimation.AnimationState.SetAnimation(0, loop, true);
            entry.TimeScale = 1;
            m_SkeletonAnimation.AnimationState.TimeScale = 1;
        }
    }

    public void Still(string skin, string still)
    {
        //Debug.LogFormat("[StandardAnimator::Still] skin:'{0}' still:'{1}'", skin, still);

        m_MeshRenderer.enabled = true;
        SetSkin(skin);
        m_SkeletonAnimation.Skeleton.SetSlotsToSetupPose(); // 2. Make sure it refreshes.
        m_SkeletonAnimation.AnimationState.Apply(m_SkeletonAnimation.Skeleton); // 3. Make sure the attachments from your currently playing animation are applied.
        var entry = m_SkeletonAnimation.AnimationState.SetAnimation(0, still, true);
        entry.TimeScale = 0;
        m_SkeletonAnimation.AnimationState.TimeScale = 1;
    }

    public void Clear(bool doHardClear = true)
    {
        //Debug.LogFormat("[StandardAnimator::Clear]");

        m_MeshRenderer.enabled = false;
        if (doHardClear) {
            m_SkeletonAnimation.Initialize(true);
            m_SkeletonAnimation.AnimationState.SetEmptyAnimations(0);
            m_SkeletonAnimation.AnimationState.Update(0);
            m_SkeletonAnimation.AnimationState.TimeScale = 1;
        }
    }

    #endregion

    #region Private Methods

    private void SetSkin(string skin)
    {
        if (!string.IsNullOrEmpty(skin)) {
            m_SkeletonAnimation.initialSkinName = skin;
            m_SkeletonAnimation.Skeleton.SetSkin(skin);
        }
    }

    #endregion
}