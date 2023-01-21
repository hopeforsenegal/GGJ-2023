using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraTransitionSquare : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public BoxCollider2D roomCenter;
    protected void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        //roomCenter = GetComponent<BoxCollider2D>();
    }
}
