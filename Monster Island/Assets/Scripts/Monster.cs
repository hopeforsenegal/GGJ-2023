using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Monster : MonoBehaviour
{
    public MonsterData data;

    public BoxCollider2D boxCollider;
    public StandardAnimator spineAnimation;
    public SpriteRenderer spriteRenderer;
    public int lastDirectionMovedIndex;
    public bool isSleep;

    protected void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
}
