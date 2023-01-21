using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const int NumMovesInTimePeriod = 4;

    // inspector
    public Player player;
    public BoxCollider2D[] obstacles;
    public BoxCollider2D[] boxes;
    public DayNight night;
    public Camera mainCamera;
    public CameraTransitionSquare[] cameraEndLocationTransforms;

    // private
    bool isDayOrNight;
    int movementCount;
    private const float LerpDuration = 0.5f;
    float timeElapsed;
    private Vector3 endMarkerPos;
    private Vector3 startMarkerPos;
    Dictionary<CameraTransitionSquare, CameraState> cameraState = new Dictionary<CameraTransitionSquare, CameraState>();

    void Start()
    {
        mainCamera = Camera.main;
        player = FindObjectOfType<Player>();
        night = FindObjectOfType<DayNight>();
        obstacles = GameObject.Find("Obstacles").GetComponentsInChildren<BoxCollider2D>();
        boxes = GameObject.Find("Boxes").GetComponentsInChildren<BoxCollider2D>();
        cameraEndLocationTransforms = GetCameraTransitionSquares();

        foreach (var cT in cameraEndLocationTransforms) {
            cameraState.Add(cT, new CameraState());
        }
    }

    void Update()
    {
        // Inputs
        var actions = GetUserActions();
        {
            // Player updates
            if (actions.left)
            {
                var velocity = Vector3.left * player.playerSpeed;
                if (!WillCollide(player.boxCollider, velocity, obstacles)) {
                    var pushable = GetPushedBox(player.boxCollider, velocity, boxes);
                    if(pushable != null){
                        Debug.Log("IsPushing");
                        if(CanPushBox(pushable, velocity, obstacles, boxes)){
                            player.transform.localPosition += velocity;
                            pushable.transform.localPosition += velocity;
                        }
                    } 
                    else {
                        player.transform.localPosition += velocity;
                    }
                }
                var (hasCollided, locationInfo) =
                    WillCollideCameraLocation(player.boxCollider, velocity, cameraEndLocationTransforms);
                if (hasCollided) {
                    (startMarkerPos, endMarkerPos) = UpdateAnimationToExecute(locationInfo, mainCamera.transform, cameraState);
                }
            }
            if (actions.up)
            {
                var velocity = Vector3.up * player.playerSpeed;
                if (!WillCollide(player.boxCollider, velocity, obstacles)) {
                    var pushable = GetPushedBox(player.boxCollider, velocity, boxes);
                    if(pushable != null){
                        Debug.Log("IsPushing");
                        if(CanPushBox(pushable, velocity, obstacles, boxes)){
                            player.transform.localPosition += velocity;
                            pushable.transform.localPosition += velocity;
                        }
                    } 
                    else {
                        player.transform.localPosition += velocity;
                    }
                }
                var (hasCollided, locationInfo) =
                    WillCollideCameraLocation(player.boxCollider, velocity, cameraEndLocationTransforms);
                if (hasCollided) {
                    (startMarkerPos, endMarkerPos) = UpdateAnimationToExecute(locationInfo, mainCamera.transform, cameraState);
                }
            }
            if (actions.down)
            {
                var velocity = Vector3.down * player.playerSpeed;
                if (!WillCollide(player.boxCollider, velocity, obstacles)) {
                    var pushable = GetPushedBox(player.boxCollider, velocity, boxes);
                    if(pushable != null){
                        Debug.Log("IsPushing");
                        if(CanPushBox(pushable, velocity, obstacles, boxes)){
                            player.transform.localPosition += velocity;
                            pushable.transform.localPosition += velocity;
                        }
                    } 
                    else {
                        player.transform.localPosition += velocity;
                    }
                }
                var (hasCollided, locationInfo) =
                    WillCollideCameraLocation(player.boxCollider, velocity, cameraEndLocationTransforms);
                if (hasCollided) {
                    (startMarkerPos, endMarkerPos) = UpdateAnimationToExecute(locationInfo, mainCamera.transform, cameraState);
                }
            }
            if (actions.right)
            {
                var velocity = Vector3.right * player.playerSpeed;
                if (!WillCollide(player.boxCollider, velocity, obstacles)) {
                    var pushable = GetPushedBox(player.boxCollider, velocity, boxes);
                    if(pushable != null){
                        Debug.Log("IsPushing");
                        if(CanPushBox(pushable, velocity, obstacles, boxes)){
                            player.transform.localPosition += velocity;
                            pushable.transform.localPosition += velocity;
                        }
                    } 
                    else {
                        player.transform.localPosition += velocity;
                    }
                }
                var (hasCollided, locationInfo) =
                    WillCollideCameraLocation(player.boxCollider, velocity, cameraEndLocationTransforms);
                if (hasCollided) {
                    (startMarkerPos, endMarkerPos) = UpdateAnimationToExecute(locationInfo, mainCamera.transform, cameraState);
                }
            }
            // night day updates
            if (actions.movement) {
                OnDayNightCycle(ref isDayOrNight, ref movementCount, night.sprite);
            }
            // Camera updates
            InterpolateActiveCamera(mainCamera.transform, cameraState, ref timeElapsed, LerpDuration, startMarkerPos, endMarkerPos);
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

    public static bool WillCollide(BoxCollider2D player, Vector3 velocity, BoxCollider2D[] boxCollider2Ds)
    {
        for (var i = 0; i <= boxCollider2Ds.Length - 1; i++) {
            var obstacle = boxCollider2Ds[i];
            if (player.OverlapPoint(obstacle.transform.position - velocity))
                return true;
        }
        return false;
    }

    // this takes into account your last step btw
    public static (bool, CameraTransitionSquare) WillCollideCameraLocation(BoxCollider2D player, Vector3 velocity, CameraTransitionSquare[] boxCollider2Ds)
    {
        for (var i = 0; i <= boxCollider2Ds.Length - 1; i++) {
            var obstacle = boxCollider2Ds[i].boxCollider;
            if (player.OverlapPoint(obstacle.transform.position - velocity))
                return (true, boxCollider2Ds[i]);
        }
        return (false, null);
    }

    public static BoxCollider2D GetPushedBox(BoxCollider2D player, Vector3 velocity, BoxCollider2D[] boxCollider2Ds)
    {
        for (var i = 0; i <= boxCollider2Ds.Length - 1; i = i + 1) {
            var box = boxCollider2Ds[i];
            if (player.OverlapPoint(box.transform.position - velocity)) {
                return box;
            }
        }
        return null;
    }

    public static bool CanPushBox(BoxCollider2D box, Vector3 velocity, BoxCollider2D[] obstacleBoxCollider2Ds, BoxCollider2D[] boxBoxCollider2Ds)
    {
        for (var i = 0; i <= obstacleBoxCollider2Ds.Length - 1; i = i + 1) {
            var obstacle = obstacleBoxCollider2Ds[i];
            if (box.OverlapPoint(obstacle.transform.position - velocity))
                return false;
        }
        for (var i = 0; i <= boxBoxCollider2Ds.Length - 1; i = i + 1) {
            var b = boxBoxCollider2Ds[i];
            if (box.OverlapPoint(b.transform.position - velocity))
                return false;
        }
        return true;
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

    public static void InterpolateActiveCamera(Transform cameraTransform, Dictionary<CameraTransitionSquare, CameraState> cameraLocations, 
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
                    cameraTransform.transform.position = Vector3.Lerp(startMarkerPos, endMarkerPos, timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                }
                else
                {
                    cameraTransform.transform.position = endMarkerPos;
                    timeElapsed = 0f;
                    kv.Value.IsAnimating = false;
                }
            }
        }
    }

    public static (Vector3 startMarkerPos, Vector3 endMarkerPos) UpdateAnimationToExecute(CameraTransitionSquare cameraTransitionSquare, Transform cameraTransform, Dictionary<CameraTransitionSquare, CameraState> cameraLocations)
    {
        foreach (var kv in cameraLocations) {
            kv.Value.IsAnimating = false;
        }
        cameraLocations[cameraTransitionSquare].IsAnimating = true;
        var startMarkerPos = cameraTransform.position;
        var endMarkerPos = cameraTransitionSquare.transform.position;
        endMarkerPos.z = -10;   // Camera always needs to be at -10
        return (startMarkerPos, endMarkerPos);
    }




    public static CameraTransitionSquare[] GetCameraTransitionSquares()
    {
        var locationsGameObject = GameObject.Find("CameraLocations");
        Debug.Assert(locationsGameObject != null);
        var cameraEndLocationTransforms = locationsGameObject.GetComponentsInChildren<CameraTransitionSquare>();
        Debug.Assert(cameraEndLocationTransforms.Any() && cameraEndLocationTransforms[0] != null);
        return cameraEndLocationTransforms;
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