using System.Collections.Generic;
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

    public static BoxCollider2D[] AsBoxColliderArray(IEnumerable<Monster> array)
    {
        var colliders = new List<BoxCollider2D>();
        foreach (var square in array) {
            colliders.Add(square.boxCollider);
        }
        return colliders.ToArray();
    }
}