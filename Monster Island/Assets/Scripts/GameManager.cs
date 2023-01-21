using UnityEngine;

public class GameManager : MonoBehaviour
{
    // inspector
    public Player player;
    public BoxCollider2D[] obstacles;
    public SpriteRenderer night;

    // private
    bool isDayOrNight;
    int movementCount;

    void Start()
    {
        player = FindObjectOfType<Player>();
        obstacles = FindObjectsOfType<BoxCollider2D>();
        night = FindObjectOfType<SpriteRenderer>();
    }

    void Update()
    {
        // Inputs
        var actions = GetUserActions();
        {
            // Player updates
            if (actions.left) {
                MovePlayer(player.boxCollider, Vector3.left * player.speed, obstacles);
            }
            if (actions.up) {
                MovePlayer(player.boxCollider, Vector3.up * player.speed, obstacles);
            }
            if (actions.down) {
                MovePlayer(player.boxCollider, Vector3.down * player.speed, obstacles);
            }
            if (actions.right) {
                MovePlayer(player.boxCollider, Vector3.right * player.speed, obstacles);
            }
            // night day updates
            if (actions.movement) {
                var v = Util.IncrementLoop(ref movementCount, 4);
                Debug.Log(v);
                if (v == 4) {
                    night.color = Util.Switch(ref isDayOrNight) ? Color.black : Color.white;
                }
            }
        }
    }

    private static Actions GetUserActions()
    {
        var action = new Actions
        {
            left = Input.GetKeyDown(KeyCode.A),
            up = Input.GetKeyDown(KeyCode.W),
            down = Input.GetKeyDown(KeyCode.S),
            right = Input.GetKeyDown(KeyCode.D),
        };
        action.movement = action.left || action.right || action.up || action.down;
        return action;
    }

    public static void MovePlayer(BoxCollider2D player, Vector3 velocity, BoxCollider2D[] boxCollider2Ds)
    {
        if (!WillCollide(player, velocity, boxCollider2Ds)) {
            player.transform.localPosition += velocity;
        }
    }

    public static bool WillCollide(BoxCollider2D player, Vector3 velocity, BoxCollider2D[] boxCollider2Ds)
    {
        for (int i = 0; i <= boxCollider2Ds.Length - 1; i = i + 1) {
            BoxCollider2D obstacle = boxCollider2Ds[i];
            if (player.OverlapPoint(obstacle.transform.position - velocity))
                return true;
        }
        return false;
    }
}

public struct Actions
{
    public bool left;
    public bool up;
    public bool down;
    public bool right;
    public bool movement;
}