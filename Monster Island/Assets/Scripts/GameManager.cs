using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static class AnimNames
    {
        public static readonly string idle = $"{nameof(idle)}";
    }

    // inspector
    public Player player;
    public BoxCollider2D[] obstacles;
    public BoxCollider2D[] boxes;
    public BoxCollider2D[] resources;
    public BoxCollider2D objective;
    public Monster[] monsters;
    public DayNight night;
    public Camera mainCamera;
    public CameraTransitionSquare[] cameraEndLocationTransforms;
    public GameOverScreen gameOverScreen;

    // private
    private const float LerpDuration = 0.5f;
    float timeElapsed;
    private Vector3 endMarkerPos;
    private Vector3 startMarkerPos;
    private float m_TimerDelayShowDeath;
    private System.Action m_DeathAction;
    readonly Dictionary<CameraTransitionSquare, CameraState> cameraState = new Dictionary<CameraTransitionSquare, CameraState>();
    int points = 0;

    //HOURS 0-8
    //Start at 1 since we get a free move at start
    //Blobs wakup at 4 go to sleep at 8
    int time = 1;
    public const int TimeInDay = 8;

    void Start()
    {
        mainCamera = Camera.main;
        gameOverScreen = FindObjectOfType<GameOverScreen>();
        player = FindObjectOfType<Player>();
        night = FindObjectOfType<DayNight>();
        obstacles = GameObject.Find("Obstacles").GetComponentsInChildren<BoxCollider2D>();
        boxes = GameObject.Find("Boxes").GetComponentsInChildren<BoxCollider2D>();
        monsters = GameObject.Find("Monsters").GetComponentsInChildren<Monster>();
        resources = GameObject.Find("Resources").GetComponentsInChildren<BoxCollider2D>();
        objective = GameObject.Find("Objective").GetComponent<BoxCollider2D>();
        cameraEndLocationTransforms = GetCameraTransitionSquares();

        Debug.Log($"resources: {resources.Length}");

        gameOverScreen.RetryEvent += MainMenu.ReLoadScene;
        gameOverScreen.QuitEvent += MainMenu.LoadMainMenu;
        foreach (var cT in cameraEndLocationTransforms) {
            cameraState.Add(cT, new CameraState());
        }

        gameOverScreen.Visibility = false;

        Util.LoopAnimation(player.spineAnimation, AnimNames.idle);

        if (IsNightTime(time)) {
            night.sprite.color = Color.black;
        } else {
            night.sprite.color = Color.white;
        }
    }

    void Update()
    {
        // Inputs
        var actions = GetUserActions();
        {
            // Player updates
            if (actions.left) {
                //Debug.Log("left");
                var velocity = Vector3.left * player.playerSpeedX;
                if (!WillCollide(player.boxCollider, velocity, obstacles)) {
                    var pushable = GetPushedBox(player.boxCollider, velocity, boxes);
                    var pushableResource = GetPushedResource(player.boxCollider, velocity, resources);
                    if (pushable != null) {
                        Debug.Log("IsPushing");
                        if (CanPushBox(pushable, velocity, obstacles, boxes)) {
                            player.transform.localPosition += velocity;
                            pushable.transform.localPosition += velocity;
                        }
                        
                    } 
                    else if (pushableResource != null) {
                        if (CanPushResource(pushableResource, velocity, obstacles, boxes, monsters, resources)) {
                            player.transform.localPosition += velocity;
                            pushableResource.transform.localPosition += velocity;
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
            if (actions.up) {
                //Debug.Log("up");
                var velocity = Vector3.up * player.playerSpeedY;
                if (!WillCollide(player.boxCollider, velocity, obstacles)) {
                    var pushable = GetPushedBox(player.boxCollider, velocity, boxes);
                    var pushableResource = GetPushedResource(player.boxCollider, velocity, resources);
                    if (pushable != null) {
                        Debug.Log("IsPushing");
                        if (CanPushBox(pushable, velocity, obstacles, boxes)) {
                            player.transform.localPosition += velocity;
                            pushable.transform.localPosition += velocity;
                        }
                        
                    } 
                    else if(pushableResource != null){
                        if (CanPushResource(pushableResource, velocity, obstacles, boxes, monsters, resources)) {
                            player.transform.localPosition += velocity;
                            pushableResource.transform.localPosition += velocity;
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
            if (actions.down) {
                //Debug.Log("down");
                var velocity = Vector3.down * player.playerSpeedY;
                if (!WillCollide(player.boxCollider, velocity, obstacles)) {
                    var pushable = GetPushedBox(player.boxCollider, velocity, boxes);
                    var pushableResource = GetPushedResource(player.boxCollider, velocity, resources);
                    if (pushable != null) {
                        //Debug.Log("IsPushing");
                        if (CanPushBox(pushable, velocity, obstacles, boxes)) {
                            player.transform.localPosition += velocity;
                            pushable.transform.localPosition += velocity;
                        }
                        
                    } 
                    else if(pushableResource != null){
                        if (CanPushResource(pushableResource, velocity, obstacles, boxes, monsters, resources)) {
                            player.transform.localPosition += velocity;
                            pushableResource.transform.localPosition += velocity;
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
                //Debug.Log("right");l
                var velocity = Vector3.right * player.playerSpeedX;
                if (!WillCollide(player.boxCollider, velocity, obstacles)) {
                    var pushable = GetPushedBox(player.boxCollider, velocity, boxes);
                    var pushableResource = GetPushedResource(player.boxCollider, velocity, resources);
                    if (pushable != null) {
                        //Debug.Log("IsPushing");
                        if (CanPushBox(pushable, velocity, obstacles, boxes)) {
                            player.transform.localPosition += velocity;
                            pushable.transform.localPosition += velocity;
                        }
                    } 
                    else if(pushableResource != null){
                        if (CanPushResource(pushableResource, velocity, obstacles, boxes, monsters, resources)) {
                            player.transform.localPosition += velocity;
                            pushableResource.transform.localPosition += velocity;
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
            if (actions.movement) {
                // monster updates
                for (var i = 0; i <= monsters.Length - 1; i += 1) {
                    Monster monster = monsters[i];

                    bool isRandom = monster.data.navigationType == MonsterData.NavigationType.Random;
                    bool isCircular = monster.data.navigationType == MonsterData.NavigationType.Circular;
                    bool isHorizontal = monster.data.navigationType == MonsterData.NavigationType.Horizontal;
                    bool isVertical = monster.data.navigationType == MonsterData.NavigationType.Vertical;

                    int killRadius = monster.data.killRadius;
                    int wakeHour = monster.data.wakeHour;
                    int sleepHour = monster.data.sleepHour;
                    int stepsToUpdate = monster.data.stepsToUpdate;

                    //check if monster is asleep
                    if (time == wakeHour && wakeHour != -1)
                        monster.data.isSleep = false;
                    if (time == sleepHour && sleepHour != -1)
                        monster.data.isSleep = true;
                    if (monster.data.isSleep == true) {
                        Debug.Log("Monster is sleep");
                        continue;
                    }

                    //check if can step
                    Debug.Log($"if can step ${time % stepsToUpdate != 0}");
                    if (time % stepsToUpdate != 0)
                        continue;

                    //handle moves
                    if (isRandom)
                        MonsterMoveRandom(player.boxCollider, boxes, obstacles, monsters[i]);
                    if (isCircular)
                        MonsterMoveCircular(player.boxCollider, boxes, obstacles, monsters[i]);
                    if (isHorizontal)
                        MonsterMoveHorizontal(player.boxCollider, boxes, obstacles, monsters[i]);
                    if (isVertical)
                        MonsterMoveVertical(player.boxCollider, boxes, obstacles, monsters[i]);


                    //Handle attack
                    if (IsWithinRange(monsters[i].boxCollider.transform.localPosition, player.transform.localPosition, killRadius)) {
                        Debug.Log("Monster can kill");
                        m_TimerDelayShowDeath = 0.1f;
                        m_DeathAction = () =>
                        {
                            gameOverScreen.Visibility = true;
                        };
                        return;
                    }
                }

                // night day updates
                if (IsNightTime(time)) {
                    night.sprite.color = Color.black;
                } else {
                    night.sprite.color = Color.white;
                }

                // increment time
                time = IncrementTime(time);

                Debug.Log($"Current time ${time}");
                CheckPoints(resources, objective, ref points);
            }
            if (Util.HasHitTimeOnce(ref m_TimerDelayShowDeath, Time.deltaTime)) {
                m_DeathAction?.Invoke();
            }
            // Camera updates
            InterpolateActiveCamera(mainCamera.transform, cameraState, ref timeElapsed, LerpDuration, startMarkerPos, endMarkerPos);
        }
    }

    public static void CheckPoints(BoxCollider2D[] resources, BoxCollider2D objective, ref int points){
        for(var i = 0;  i <= resources.Length - 1; i++){
            if(IsOverlapping(resources[i], objective) && resources[i].enabled == true){
                //remove resource
                resources[i].gameObject.SetActive(false);
                //and its collider
                resources[i].enabled = false;
                //remove from array
                resources = resources.Where((source, index) => index != i).ToArray();
                //add points
                points += 1;
            }
        }
        Debug.Log($"Current points ${points}");
    }

    public static int IncrementTime(int time)
    {
        time += 1;
        if (time >= TimeInDay) time = 0;
        return time;
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
            if (player.OverlapPoint(obstacle.transform.position))
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

    public static BoxCollider2D GetPushedResource(BoxCollider2D player, Vector3 velocity, BoxCollider2D[] resources)
    {
        for (var i = 0; i <= resources.Length - 1; i = i + 1) {
            var box = resources[i];
            if (player.OverlapPoint(box.transform.position - velocity) && box.enabled == true) {
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

    public static bool CanPushResource(BoxCollider2D resource, Vector3 velocity, BoxCollider2D[] obstacles , BoxCollider2D[] boxes, Monster[] monsters, BoxCollider2D[] resources)
    {
        for (var i = 0; i <= obstacles.Length - 1; i = i + 1) {
            var obstacle = obstacles[i];
            if (resource.OverlapPoint(obstacle.transform.position - velocity))
                return false;
        }
        for (var i = 0; i <= boxes.Length - 1; i = i + 1) {
            var b = boxes[i];
            if (resource.OverlapPoint(b.transform.position - velocity))
                return false;
        }
        for (var i = 0; i <= monsters.Length - 1; i = i + 1) {
            var b = monsters[i];
            if (resource.OverlapPoint(b.transform.position - velocity))
                return false;
        }

        for (var i = 0; i <= resources.Length - 1; i = i + 1) {
            var b = resources[i];
            if (resource.OverlapPoint(b.transform.position - velocity) && resource != b && b.enabled == true)
                return false;
        }
        return true;
    }

    // public static void OnDayNightCycle(ref bool isDayOrNight, ref int movementCount, SpriteRenderer night)
    // {
    //     var increment = Util.IncrementLoop(ref movementCount, NumMovesInTimePeriod);
    //     //Debug.Log(increment);
    //     if (increment == NumMovesInTimePeriod) {
    //         if (Util.Switch(ref isDayOrNight)) {
    //             night.color = Color.black;
    //         } else {
    //             night.color = Color.white;
    //         }
    //     }
    // }

    public static void InterpolateActiveCamera(Transform cameraTransform, Dictionary<CameraTransitionSquare, CameraState> cameraLocations,
        ref float timeElapsed,
        float lerpDuration,
        Vector3 startMarkerPos, Vector3 endMarkerPos)
    {
        foreach (var kv in cameraLocations) {
            if (kv.Value.IsAnimating) {
                if (timeElapsed < lerpDuration) {
                    cameraTransform.transform.position = Vector3.Lerp(startMarkerPos, endMarkerPos, timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                } else {
                    cameraTransform.transform.position = endMarkerPos;
                    timeElapsed = 0f;
                    kv.Value.IsAnimating = false;
                }
            }
        }
    }

    public static (Vector3 startMarkerPos, Vector3 endMarkerPos) UpdateAnimationToExecute(CameraTransitionSquare cameraTransitionSquare,
                                                                                          Transform cameraTransform,
                                                                                          Dictionary<CameraTransitionSquare, CameraState> cameraLocations)
    {
        foreach (var kv in cameraLocations) {
            kv.Value.IsAnimating = false;
        }
        cameraLocations[cameraTransitionSquare].IsAnimating = true;
        var startMarkerPos = cameraTransform.position;
        var endMarkerPos = cameraTransitionSquare.roomCenter.transform.position;
        endMarkerPos.z = -10;   // Camera always needs to be at -10
        return (startMarkerPos, endMarkerPos);
    }

    public static void Die()
    {
        //Debug.Log("Monster can kill");
        Application.Quit();
        //EditorApplication.isPlaying = false;
        UnityEditor.EditorApplication.isPlaying = false;

    }

    public static void Win(){
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    private static bool WillObjectCollide(BoxCollider2D box, Vector3 velocity, BoxCollider2D[] obstacleBoxCollider2Ds, BoxCollider2D[] boxBoxCollider2Ds, BoxCollider2D playerBoxCollider2D)
    {
        for (var i = 0; i <= obstacleBoxCollider2Ds.Length - 1; i += 1) {
            var obstacle = obstacleBoxCollider2Ds[i];
            if (box.OverlapPoint(obstacle.transform.position - velocity) && obstacle != box)
                return true;
        }
        for (var i = 0; i <= boxBoxCollider2Ds.Length - 1; i += 1) {
            var b = boxBoxCollider2Ds[i];
            if (box.OverlapPoint(b.transform.position - velocity) && b != box)
                return true;
        }

        if (box.OverlapPoint(playerBoxCollider2D.transform.position - velocity))
            return true;

        return false;
    }

    private static bool IsOverlapping(BoxCollider2D b1, BoxCollider2D b2)
    {
        if (b1.OverlapPoint(b2.transform.position))
            return true;
        if (b2.OverlapPoint(b1.transform.position))
            return true;
        return false;
    }

    public static void MonsterMoveRandom(BoxCollider2D playerBoxCollider2D, BoxCollider2D[] boxBoxCollider2Ds, BoxCollider2D[] obstacleBoxCollider2Ds, Monster monster)
    {
        Vector3[] dirs = {
            Vector3.up,
            Vector3.right,
            Vector3.left,
            Vector3.down
        };
        var inc = 0;
        var checking = true;
        while (checking && inc < 5) {
            //Debug.Log("Checking moves");
            var vel = dirs[Random.Range(1, 4)];
            if (!WillObjectCollide(monster.boxCollider, vel, obstacleBoxCollider2Ds, boxBoxCollider2Ds, playerBoxCollider2D)) {
                monster.transform.localPosition += vel;
                //Debug.Log($"moving monster {monster.transform.localPosition}");
                checking = false;
            }
            inc++;
        }

    }

    public static void MonsterMoveCircular(BoxCollider2D playerBoxCollider2D, BoxCollider2D[] boxBoxCollider2Ds, BoxCollider2D[] obstacleBoxCollider2Ds, Monster monster)
    {
        Vector3[] dirs = {
            Vector3.up,
            Vector3.right,
            Vector3.left,
            Vector3.down
        };
        
        var inc = monster.lastDirectionMovedIndex != null ? (monster.lastDirectionMovedIndex + 1) % dirs.Length-1: 0;
        var vel = dirs[inc];
        if (!WillObjectCollide(monster.boxCollider, vel, obstacleBoxCollider2Ds, boxBoxCollider2Ds, playerBoxCollider2D)) {
            monster.transform.localPosition += vel;
            monster.lastDirectionMovedIndex = inc;
        }

    }

    //Move monster Horizontal
    public static void MonsterMoveHorizontal(BoxCollider2D playerBoxCollider2D, BoxCollider2D[] boxBoxCollider2Ds, BoxCollider2D[] obstacleBoxCollider2Ds, Monster monster)
    {
        Vector3[] dirs = {
            Vector3.right,
            Vector3.left
        };
        var inc = 0;
        var checking = true;
        while (checking && inc < 5) {
            //Debug.Log("Checking moves");
            var vel = dirs[Random.Range(1, 2)];
            if (!WillObjectCollide(monster.boxCollider, vel, obstacleBoxCollider2Ds, boxBoxCollider2Ds, playerBoxCollider2D)) {
                monster.transform.localPosition += vel;
                //Debug.Log($"moving monster {monster.transform.localPosition}");
                checking = false;
            }
            inc++;
        }

    }

    //Move monster Vertical
    public static void MonsterMoveVertical(BoxCollider2D playerBoxCollider2D, BoxCollider2D[] boxBoxCollider2Ds, BoxCollider2D[] obstacleBoxCollider2Ds, Monster monster)
    {
        Vector3[] dirs = {
            Vector3.up,
            Vector3.down
        };
        var inc = 0;
        var checking = true;
        while (checking && inc < 5) {
            //Debug.Log("Checking moves");
            var vel = dirs[Random.Range(1, 2)];
            if (!WillObjectCollide(monster.boxCollider, vel, obstacleBoxCollider2Ds, boxBoxCollider2Ds, playerBoxCollider2D)) {
                monster.transform.localPosition += vel;
                //Debug.Log($"moving monster {monster.transform.localPosition}");
                checking = false;
            }
            inc++;
        }

    }

    public static bool IsWithinRange(Vector3 center, Vector3 point, float radius)
    {
        var diff = center - point;
        if (Mathf.Abs(diff.x) <= radius && Mathf.Abs(diff.y) <= radius)
            return true;
        return false;
    }

    public static bool IsNightTime(int time)
    {
        return time >= (TimeInDay / 2);
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