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
                Move(Vector3.left * player.speed);
            }
            if (actions.up) {
                Move(Vector3.up * player.speed);
            }
            if (actions.down) {
                Move(Vector3.down * player.speed);
            }
            if (actions.right) {
                Move(Vector3.right * player.speed);
            }
            // night day udpates
            if (actions.movement) {
                var v = Util.IncrementLoop(ref movementCount, 4);
                Debug.Log(v);
                if (v == 4) {
                    night.color = Util.Switch(ref isDayOrNight) ? Color.black : Color.white;
                }
            }
        }
    }

    Actions GetUserActions()
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


    // TODO Delete these and make a single static function. Make the test class reference this function
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
    public bool up;
    public bool down;
    public bool right;
    public bool movement;
}