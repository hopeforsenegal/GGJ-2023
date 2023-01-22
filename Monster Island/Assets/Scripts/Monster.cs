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
    public float SpeedX = 2.2f;
    public float SpeedY = 2;

    protected void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
}
