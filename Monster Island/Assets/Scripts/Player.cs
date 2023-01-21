using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public float playerSpeed = 1;
    public BoxCollider2D boxCollider;

    protected void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
}
