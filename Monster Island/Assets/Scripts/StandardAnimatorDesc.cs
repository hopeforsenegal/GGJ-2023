using System;
using UnityEngine;

public class StandardAnimatorDesc
{
    private Action m_AnimationEnd;

    public void FireAnimationEnd()
    {
        m_AnimationEnd?.Invoke();
        m_AnimationEnd = null;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public bool HasError => !string.IsNullOrEmpty(Error);
    // ReSharper disable once MemberCanBePrivate.Global
    public string Error { get; }


    public StandardAnimatorDesc(string error)
    {
        Error = error;
        m_AnimationEnd = null;
    }

    public StandardAnimatorDesc()
    {
    }

    public StandardAnimatorDesc setAssertOnError()
    {
        Debug.Assert(!HasError, Error);
        return this;
    }

    public StandardAnimatorDesc setOnAnimationEnd(Action onAnimationEnd)
    {
        m_AnimationEnd = onAnimationEnd;
        return this;
    }
}
