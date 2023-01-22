using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class GameOverScreen : MonoBehaviour
{
    public Button retryIsland;
    public Button quit;

    public event Action RetryEvent;
    public event Action QuitEvent;

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
            QuitEvent?.Invoke();
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
            canvasGroup.alpha = value ? 1f : 0f;
        }
    }
}
