using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    static readonly string game_scene = "Island_one";

    public Button play;
    public Button quit;

    void Start()
    {
        play.onClick.AddListener(() =>
        {
            PlayGame();
        });
        quit.onClick.AddListener(() =>
        {
            ExitGame();
        });
    }

    protected void Update()
    {

        var hitEnterKey = Input.GetKey(KeyCode.KeypadEnter)
            || Input.GetKey(KeyCode.Return)
                               || Input.GetKey((KeyCode.Space));

        var hitLetter = false;
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode))) {
            if (kcode >= KeyCode.A && kcode <= KeyCode.Z && Input.GetKeyDown(kcode)) {
                Debug.Log("KeyCode down: " + kcode);
                hitLetter = true;
            }
        }

        if (hitEnterKey || hitLetter) {
            PlayGame();
        } else if (Input.GetKey(KeyCode.Escape)) {
            ExitGame();
        }
    }

    private void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(game_scene);
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
}
