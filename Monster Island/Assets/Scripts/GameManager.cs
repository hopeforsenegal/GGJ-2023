using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public BoxCollider2D[] obstacles;

    void Update()
    {
        // Inputs
        var actions = GetUserActions();
        {
            // Player updates
            if (actions.left) {
                Move(Vector3.left * player.speed);
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

    void Move(Vector3 velocity)
    {
        if (!WillCollide(velocity)) {
            player.transform.localPosition += velocity;
        }
    }

    bool WillCollide(Vector3 velocity)
    {
        for (int i = 0; i <= obstacles.Length - 1; i = i + 1) {
            BoxCollider2D obstacle = obstacles[i];
            if (player.boxCollider.OverlapPoint(obstacle.transform.position - velocity))
                return true;
        }
        return false;
    }
}


public struct Actions
{
    public bool left;
}