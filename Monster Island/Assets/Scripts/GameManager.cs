using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const int NumMovesInTimePeriod = 4;

    // inspector
    public Player player;
    public BoxCollider2D[] obstacles;
    public SpriteRenderer night;
    public Camera mainCamera;
    public Transform transforma;
    public Transform transformb;
    public Transform transformc;

    // private
    bool isDayOrNight;
    int movementCount;
    Dictionary<Transform, CameraState> cameraLocations = new Dictionary<Transform, CameraState>();

    void Start()
    {
        player = FindObjectOfType<Player>();
        obstacles = GameObject.Find("Obstacles").GetComponentsInChildren<BoxCollider2D>();
        night = FindObjectOfType<SpriteRenderer>();
        
        cameraLocations.Add(transforma, new CameraState());
        cameraLocations.Add(transformb, new CameraState());
        cameraLocations.Add(transformc, new CameraState());
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
                OnDayNightCycle(ref isDayOrNight, ref movementCount, night);
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
        for (int i = 0; i <= boxCollider2Ds.Length - 1; i++) {
            BoxCollider2D obstacle = boxCollider2Ds[i];
            if (player.OverlapPoint(obstacle.transform.position - velocity))
                return true;
        }
        return false;
    }

    public static void OnDayNightCycle(ref bool isDayOrNight, ref int movementCount, SpriteRenderer night)
    {
        var increment = Util.IncrementLoop(ref movementCount, NumMovesInTimePeriod);
        Debug.Log(increment);
        if (increment == NumMovesInTimePeriod)
        {
            if (Util.Switch(ref isDayOrNight))
            {
                night.color = Color.black;
            }
            else
            {
                night.color = Color.white;
            }
        }
    }

    public static void InterpolateActiveCamera(Transform cameraTransform, Dictionary<Transform, CameraState> cameraLocations, 
        ref float timeElapsed,
        float lerpDuration,
        Vector3 startMarkerPos, Vector3 endMarkerPos)
    {
        foreach (var kv in cameraLocations)
        {
            if (kv.Value.IsAnimating)
            {
                if (timeElapsed < lerpDuration)
                {
                    cameraTransform.position = Vector3.Lerp(startMarkerPos, endMarkerPos, timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                }
                else
                {
                    cameraTransform.position = endMarkerPos;
                    timeElapsed = 0f;
                    kv.Value.IsAnimating = false;
                }
            }
        }
    }

    public static (Vector3 startMarkerPos, Vector3 endMarkerPos) UpdateAnimationToExecute(Transform locationTransform, Transform cameraTransform, Dictionary<Transform, CameraState> cameraLocations)
    {
        foreach (var kv in cameraLocations) {
            kv.Value.IsAnimating = false;
        }
        cameraLocations[locationTransform].IsAnimating = true;
        var startMarkerPos = cameraTransform.position;
        var endMarkerPos = locationTransform.position;
        return (startMarkerPos, endMarkerPos);
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