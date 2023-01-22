using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class WinScreen : MonoBehaviour
{
    public Button returnToMenu;
    public Button exitApplication;

    public event Action ReturnToMenuEvent;
    public event Action ExitApplicationEvent;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        returnToMenu.onClick.AddListener(() =>
        {
            ReturnToMenuEvent?.Invoke();
        });
        exitApplication.onClick.AddListener(() =>
        {
            ExitApplicationEvent?.Invoke();
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
