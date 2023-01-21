using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DayNight : MonoBehaviour
{
    public SpriteRenderer sprite;

    protected void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
}
