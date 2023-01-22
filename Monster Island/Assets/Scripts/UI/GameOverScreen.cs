using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class GameOverScreen : MonoBehaviour
{
    public Button retryIsland;
    public Button quit;

    public event Action RetryEvent;
    public event Action ReturnToMenuEvent;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        retryIsland.onClick.AddListener(() =>
        {
            RetryEvent?.Invoke();
        });
        quit.onClick.AddListener(() =>
        {
            ReturnToMenuEvent?.Invoke();
        });
    }

    public bool Visibility
    {
        get
        {
            return Math.Abs(canvasGroup.alpha - 1f) < 0.05f;
        }
        set
        {
            Util.Toggle(canvasGroup, value);
        }
    }
}
