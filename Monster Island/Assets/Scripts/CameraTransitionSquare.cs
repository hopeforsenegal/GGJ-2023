using System.Collections.Generic;
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

    public static BoxCollider2D[] AsBoxColliderArray(CameraTransitionSquare[] cameraTransitionSquares)
    {
        var colliders = new List<BoxCollider2D>();
        foreach (var square in cameraTransitionSquares) {
            colliders.Add(square.boxCollider);
        }
        return colliders.ToArray();
    }
}
