using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MacGuffin : MonoBehaviour
{
    public BoxCollider2D boxCollider;

    protected void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
}
