using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public float playerSpeedX = 2.2f;
    public float playerSpeedY = 2;
    public BoxCollider2D boxCollider;
    public StandardAnimator spineAnimation;

    protected void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
}
