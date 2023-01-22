using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Monster : MonoBehaviour
{
    public MonsterData data;
    public BoxCollider2D boxCollider;
    public SkeletonAnimation spineAnimation;
    public bool isSleep;

    protected void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
}
