using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraTransitionSquare : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public BoxCollider2D roomCenter;    // TODO make this a Transform

    protected void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
}
