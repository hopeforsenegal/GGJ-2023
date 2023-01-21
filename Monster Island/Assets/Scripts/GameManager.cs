using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;

    void Update()
    {
        // Inputs
        var actions = GetUserActions();
        {
            // Player updates
            if (actions.left) {
                transform.localPosition += Vector3.left * player.speed;
            }
        }
    }

    Actions GetUserActions()
    {
        return new Actions
        {
            left = Input.GetKeyDown(KeyCode.A),
        };
    }
}


public struct Actions {
    public bool left;
}