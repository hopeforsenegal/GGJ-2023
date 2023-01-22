using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Monster : MonoBehaviour
{
    public MonsterData data;
    public int lastDirectionMovedIndex;

    public BoxCollider2D boxCollider;

    protected void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
}
