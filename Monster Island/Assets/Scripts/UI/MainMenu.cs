using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button play;
    public Button quit;

    void Start()
    {
        play.onClick.AddListener(LoadFirstIsland);
        quit.onClick.AddListener(ExitApplication);
    }

    protected void Update()
    {
        var hitEnterKey = Input.GetKey(KeyCode.KeypadEnter)
                          || Input.GetKey(KeyCode.Return)
                          || Input.GetKey((KeyCode.Space));

        var hitLetter = false;
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode))) {
            if (keyCode >= KeyCode.A && keyCode <= KeyCode.Z && Input.GetKeyDown(keyCode)) {
                Debug.Log("KeyCode down: " + keyCode);
                hitLetter = true;
            }
        }

        if (hitEnterKey || hitLetter) {
            LoadFirstIsland();
        } else if (Input.GetKey(KeyCode.Escape)) {
            ExitApplication();
        }
    }

    public static void LoadFirstIsland()
    {
        Debug.Log($"{nameof(LoadFirstIsland)}");
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNames.Island_one);
    }

    public static void LoadMainMenu()
    {
        Debug.Log($"{nameof(LoadMainMenu)}");
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNames.MainMenu);
    }

    public static void ReloadScene()
    {
        Debug.Log($"{nameof(ReloadScene)}");
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public static void ExitApplication()
    {
        Debug.Log($"{nameof(ExitApplication)}");
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
}
