using MoonlitSystem.Animators;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Monster : MonoBehaviour
{
    public MonsterData data;
    public int lastDirectionMovedIndex;

    public BoxCollider2D boxCollider;
    public StandardAnimator spineAnimation;
    public bool isSleep;

    protected void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
}
