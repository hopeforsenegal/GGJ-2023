using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;

    void Update()
    {
        // Inputs
        var actions = GetActions();
        {
            // Player updates
            if (actions.left) {
                transform.localPosition += Vector3.left * player.speed;
            }
        }
    }

    Actions GetActions()
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