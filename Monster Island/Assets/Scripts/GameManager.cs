using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int TimeInDay = 8;
    private const float CameraLerpDuration = 0.5f;

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
    public WinScreen winScreen;
    public Image sundial;
    public SettingsData settingsData;

    // private
    private float timeElapsed;
    private bool m_IsGameOver;
    private bool m_IsWon;
    private Vector3 endMarkerPos;
    private Vector3 startMarkerPos;
    private float m_TimerDelayShowGameOver;
    private System.Action m_GameOverAction;
    private float m_TimerDelayShowWin;
    private System.Action m_WinAction;
    private readonly Dictionary<CameraTransitionSquare, CameraState> cameraState = new Dictionary<CameraTransitionSquare, CameraState>();
    private int points = 0;

    //HOURS 0-8
    //Start at 1 since we get a free move at start
    //Blobs wakeup at 4 go to sleep at 8
    private int time = 1;

    private void Start()
    {
        mainCamera = Camera.main;
        gameOverScreen = FindObjectOfType<GameOverScreen>();
        winScreen = FindObjectOfType<WinScreen>();
        player = FindObjectOfType<Player>();
        night = FindObjectOfType<DayNight>();
        obstacles = GameObject.Find("Obstacles").GetComponentsInChildren<BoxCollider2D>();
        boxes = GameObject.Find("Boxes").GetComponentsInChildren<BoxCollider2D>();
        monsters = GameObject.Find("Monsters").GetComponentsInChildren<Monster>();
        resources = GameObject.Find("Resources").GetComponentsInChildren<BoxCollider2D>();
        objective = GameObject.Find("Objective").GetComponent<BoxCollider2D>();
        cameraEndLocationTransforms = GetCameraTransitionSquares();

        winScreen.ExitApplicationEvent += MainMenu.ExitApplication;
        winScreen.ReturnToMenuEvent += MainMenu.LoadMainMenu;
        gameOverScreen.RetryEvent += MainMenu.ReloadScene;
        gameOverScreen.ReturnToMenuEvent += MainMenu.LoadMainMenu;

        foreach (var cT in cameraEndLocationTransforms) {
            cameraState.Add(cT, new CameraState());
        }
        winScreen.Visibility = gameOverScreen.Visibility = false;

        player.spineAnimation.Loop(SkinsNames.@default, PlayerAnim.idle);
        foreach (var monster in monsters) {
            monster.spineAnimation.Loop(SkinsNames.@default, MonsterAnim.awake);
        }

        // On start account for the offset
        if (IsNightTime(time - 1)) {
            night.sprite.color = Color.black;
        } else {
            night.sprite.color = Color.white;
        }
        sundial.sprite = settingsData.sundialSprites[time - 1];
    }

    private void Update()
    {
        // Inputs
        var actions = GetUserActions();

        {   // Check if we died/won before processing any more logic
            if (Util.HasHitTimeOnce(ref m_TimerDelayShowGameOver, Time.deltaTime)) {
                m_GameOverAction?.Invoke();
            }
            if (m_IsGameOver)
                return;
            if (Util.HasHitTimeOnce(ref m_TimerDelayShowWin, Time.deltaTime)) {
                m_WinAction?.Invoke();
            }
            if (m_IsWon)
                return;
        }
        {
            var movement = false;
            var hasAddedToInventory = false;
            // Player updates
            if (actions.left) {
                //Debug.Log("left");
                var velocity = Vector3.left * player.playerSpeedX;
                if (!WillCollide(player.boxCollider, velocity, obstacles)) {
                    var pushable = GetPushedBox(player.boxCollider, velocity, boxes);
                    var pushableResource = GetPushedResource(player.boxCollider, velocity, resources);
                    if (pushable != null) {
                        if (CanPushBox(pushable, velocity, obstacles, boxes, resources, Monster.AsBoxColliderArray(monsters), new BoxCollider2D[] { objective })) {
                            player.transform.localPosition += velocity;
                            pushable.transform.localPosition += velocity;
                            movement = true;
                        }
                    } else if (pushableResource != null) {
                        AddToInventory(pushableResource);
                        hasAddedToInventory = true;
                        player.transform.localPosition += velocity;
                        movement = true;

                    } else {
                        player.transform.localPosition += velocity;
                        movement = true;
                    }
                    player.transform.localScale = PlayerScale.Left;
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
                        //Debug.Log("IsPushing");
                        if (CanPushBox(pushable, velocity, obstacles, boxes, resources, Monster.AsBoxColliderArray(monsters), new BoxCollider2D[] { objective })) {
                            player.transform.localPosition += velocity;
                            pushable.transform.localPosition += velocity;
                            movement = true;
                        }
                    } else if (pushableResource != null) {
                        AddToInventory(pushableResource);
                        hasAddedToInventory = true;
                        player.transform.localPosition += velocity;
                        movement = true;
                    } else {
                        player.transform.localPosition += velocity;
                        movement = true;
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
                        if (CanPushBox(pushable, velocity, obstacles, boxes, resources, Monster.AsBoxColliderArray(monsters), new BoxCollider2D[] { objective })) {
                            player.transform.localPosition += velocity;
                            pushable.transform.localPosition += velocity;
                            movement = true;
                        }
                    } else if (pushableResource != null) {
                        AddToInventory(pushableResource);
                        hasAddedToInventory = true;
                        player.transform.localPosition += velocity;
                        movement = true;
                    } else {
                        player.transform.localPosition += velocity;
                        movement = true;
                    }
                }
                var (hasCollided, locationInfo) =
                    WillCollideCameraLocation(player.boxCollider, velocity, cameraEndLocationTransforms);
                if (hasCollided) {
                    (startMarkerPos, endMarkerPos) = UpdateAnimationToExecute(locationInfo, mainCamera.transform, cameraState);
                }
            }
            if (actions.right) {
                //Debug.Log("right");l
                var velocity = Vector3.right * player.playerSpeedX;
                if (!WillCollide(player.boxCollider, velocity, obstacles)) {
                    var pushable = GetPushedBox(player.boxCollider, velocity, boxes);
                    var pushableResource = GetPushedResource(player.boxCollider, velocity, resources);
                    if (pushable != null) {
                        //Debug.Log("IsPushing");
                        if (CanPushBox(pushable, velocity, obstacles, boxes, resources, Monster.AsBoxColliderArray(monsters), new BoxCollider2D[] { objective })) {
                            player.transform.localPosition += velocity;
                            pushable.transform.localPosition += velocity;
                            movement = true;
                        }
                    } else if (pushableResource != null) {
                        AddToInventory(pushableResource);
                        hasAddedToInventory = true;
                        player.transform.localPosition += velocity;
                        movement = true;
                    } else {
                        player.transform.localPosition += velocity;
                        movement = true;
                    }
                    player.transform.localScale = PlayerScale.Right;
                }
                var (hasCollided, locationInfo) =
                    WillCollideCameraLocation(player.boxCollider, velocity, cameraEndLocationTransforms);
                if (hasCollided) {
                    (startMarkerPos, endMarkerPos) = UpdateAnimationToExecute(locationInfo, mainCamera.transform, cameraState);
                }
            }

            // Update the world time and everything only if you were able to actually move
            if (movement) {
                // monster updates
                for (var i = 0; i <= monsters.Length - 1; i++) {
                    var monster = monsters[i];

                    var isRandom = monster.data.navigationType == MonsterData.NavigationType.Random;
                    var isCircular = monster.data.navigationType == MonsterData.NavigationType.Circular;
                    var isHorizontal = monster.data.navigationType == MonsterData.NavigationType.Horizontal;
                    var isVertical = monster.data.navigationType == MonsterData.NavigationType.Vertical;
                    var isLineOfSight = monster.data.navigationType == MonsterData.NavigationType.LineOfSight;

                    var killRadius = monster.data.killRadius;
                    var wakeHour = monster.data.wakeHour;
                    var sleepHour = monster.data.sleepHour;
                    var stepsToUpdate = monster.data.stepsToUpdate;

                    //check if monster is ABOUT to sleep or wakeup
                    if ((time == wakeHour - 1 || (time == TimeInDay - 1 && wakeHour == 0)) && wakeHour != -1)
                        monster.spriteRenderer.enabled = true;
                    else if ((time == sleepHour - 1 || (time == TimeInDay - 1 && sleepHour == 0)) && sleepHour != -1)
                        monster.spriteRenderer.enabled = true;
                    else
                        monster.spriteRenderer.enabled = false;

                    //check if monster is asleep
                    if (time == wakeHour && wakeHour != -1)
                        monster.isSleep = false;
                    if (time == sleepHour && sleepHour != -1)
                        monster.isSleep = true;

                    monster.spineAnimation.Loop(SkinsNames.@default, monster.isSleep ? MonsterAnim.sleep : MonsterAnim.awake);

                    if (monster.isSleep == true) {
                        //Debug.Log("Monster is sleep");
                        continue;   // break out if sleeping
                    }

                    //Handle attack
                    if (IsWithinRange(monsters[i].boxCollider.transform.position, player.transform.position, killRadius)) {
                        Debug.Log($"{monsters[i].data.name} can kill");
                        m_IsGameOver = true;
                        monsters[i].spineAnimation.Play(SkinsNames.@default, MonsterAnim.explode, string.Empty, () =>
                        {
                            m_TimerDelayShowGameOver = 0.1f;
                            m_GameOverAction = () =>
                            {
                                gameOverScreen.Visibility = true;
                            };
                        });
                        player.spineAnimation.Play(SkinsNames.@default, PlayerAnim.hit);
                        return;
                    }

                    //check if can step
                    //Debug.Log($"if can step ${time % stepsToUpdate != 0}");
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
                    if (isLineOfSight)
                        MonsterMoveLineOfSight(player.boxCollider, boxes, obstacles, monsters[i], monsters[i].SpeedX, monsters[i].SpeedY);

                    //Handle attack
                    if (IsWithinRange(monsters[i].boxCollider.transform.localPosition, player.transform.localPosition, killRadius)) {
                        Debug.Log($"{monsters[i].data.name} can kill");
                        m_IsGameOver = true;
                        monsters[i].spineAnimation.Play(SkinsNames.@default, MonsterAnim.explode, string.Empty, () =>
                        {
                            m_TimerDelayShowGameOver = 0.1f;
                            m_GameOverAction = () =>
                            {
                                gameOverScreen.Visibility = true;
                            };
                        });
                        player.spineAnimation.Play(SkinsNames.@default, PlayerAnim.hit);
                        return;
                    }
                }

                // night day updates
                if (IsNightTime(time)) {
                    night.sprite.color = Color.black;
                } else {
                    night.sprite.color = Color.white;
                }
                sundial.sprite = settingsData.sundialSprites[time];

                // increment time
                time = IncrementTime(time);
                player.spineAnimation.Play(SkinsNames.@default, hasAddedToInventory ? PlayerAnim.item_collect : PlayerAnim.move, PlayerAnim.idle);

                //Debug.Log($"Current time ${time}");

                // Determine if they won
                var pointsNeededToWin = resources.Length;
                if (IsOverlapping(player.boxCollider, objective) && points >= pointsNeededToWin) {
                    Debug.Log("Win");
                    m_IsWon = true;
                    player.spineAnimation.Play(SkinsNames.@default, PlayerAnim.victory, string.Empty, () =>
                    {
                        m_TimerDelayShowWin = 0.1f;
                        m_WinAction = () =>
                        {
                            winScreen.Visibility = true;
                            var audio = FindObjectOfType<AudioSource>();
                            audio.clip = settingsData.winAudio;
                            audio.Play();
                        };
                    });
                }
                //Debug.Log($"Current points ${points}");
            }

            // Camera updates
            InterpolateActiveCamera(mainCamera.transform, cameraState, ref timeElapsed, CameraLerpDuration, startMarkerPos, endMarkerPos);
        }
    }

    private void AddToInventory(BoxCollider2D box)
    {
        box.gameObject.SetActive(false);
        box.enabled = false;
        resources = resources.Where(e => e != box).ToArray();
        points += 1;
    }

    public static int IncrementTime(int time)
    {
        time += 1;
        if (time >= TimeInDay) time = 0;
        return time;
    }

    private static Actions GetUserActions()
    {
        return new Actions
        {
            left = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow),
            up = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow),
            down = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow),
            right = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow),
        };
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
    public static (bool, CameraTransitionSquare) WillCollideCameraLocation(BoxCollider2D playerCollider,
                                                                           Vector3 velocity,
                                                                           CameraTransitionSquare[] cameraSquares)
    {
        foreach (var cameraSquare in cameraSquares) {
            if (playerCollider.OverlapPoint(cameraSquare.transform.position - velocity))
                return (true, cameraSquare);
        }
        return (false, null);
    }

    public static BoxCollider2D GetPushedBox(BoxCollider2D player, Vector3 velocity, BoxCollider2D[] boxCollider2Ds)
    {
        for (var i = 0; i <= boxCollider2Ds.Length - 1; i++) {
            var box = boxCollider2Ds[i];
            if (player.OverlapPoint(box.transform.position - velocity)) {
                return box;
            }
        }
        return null;
    }

    public static BoxCollider2D GetPushedResource(BoxCollider2D player, Vector3 velocity, BoxCollider2D[] resources)
    {
        for (var i = 0; i <= resources.Length - 1; i++) {
            var box = resources[i];
            if (player.OverlapPoint(box.transform.position - velocity) && box.enabled) {
                return box;
            }
        }
        return null;
    }

    public static bool CanPushBox(BoxCollider2D boxCollider, Vector3 velocity, params IEnumerable<BoxCollider2D>[] colliderListsThatStopBoxMovement)
    {
        foreach (var stoppingColliderList in colliderListsThatStopBoxMovement) {
            foreach (var stoppingCollider in stoppingColliderList) {
                if (boxCollider.OverlapPoint(stoppingCollider.transform.position - velocity))
                    return false;
            }
        }

        return true;
    }

    public static bool CanPushResource(BoxCollider2D resource, Vector3 velocity, BoxCollider2D[] obstacles, BoxCollider2D[] boxes, Monster[] monsters, BoxCollider2D[] resources)
    {
        for (var i = 0; i <= obstacles.Length - 1; i++) {
            var obstacle = obstacles[i];
            if (resource.OverlapPoint(obstacle.transform.position - velocity))
                return false;
        }
        for (var i = 0; i <= boxes.Length - 1; i++) {
            var b = boxes[i];
            if (resource.OverlapPoint(b.transform.position - velocity))
                return false;
        }
        for (var i = 0; i <= monsters.Length - 1; i++) {
            var b = monsters[i];
            if (resource.OverlapPoint(b.transform.position - velocity))
                return false;
        }

        for (var i = 0; i <= resources.Length - 1; i++) {
            var b = resources[i];
            if (resource.OverlapPoint(b.transform.position - velocity) && resource != b && b.enabled == true)
                return false;
        }
        return true;
    }

    public static void InterpolateActiveCamera(Transform cameraTransform,
                                               Dictionary<CameraTransitionSquare, CameraState> cameraLocations,
                                               ref float timeElapsed,
                                               float lerpDuration,
                                               Vector3 startMarkerPos,
                                               Vector3 endMarkerPos)
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
        var endMarkerPos = cameraTransitionSquare.roomCenter.position;
        endMarkerPos.z = -10;   // Camera always needs to be at -10
        return (startMarkerPos, endMarkerPos);
    }

    private static bool WillObjectCollide(BoxCollider2D box, Vector3 velocity, BoxCollider2D[] obstacleBoxCollider2Ds, BoxCollider2D[] boxBoxCollider2Ds, BoxCollider2D playerBoxCollider2D)
    {
        for (var i = 0; i <= obstacleBoxCollider2Ds.Length - 1; i++) {
            var obstacle = obstacleBoxCollider2Ds[i];
            if (box.OverlapPoint(obstacle.transform.position - velocity) && obstacle != box)
                return true;
        }
        for (var i = 0; i <= boxBoxCollider2Ds.Length - 1; i++) {
            var b = boxBoxCollider2Ds[i];
            if (box.OverlapPoint(b.transform.position - velocity) && b != box)
                return true;
        }

        if (box.OverlapPoint(playerBoxCollider2D.transform.position - velocity))
            return true;

        return false;
    }

    private static bool WillMonsterCollide(BoxCollider2D monsterCollider, Vector3 velocity, params IEnumerable<BoxCollider2D>[] colliderListsThatInteractWithMonster)
    {
        foreach (var interactableColliderList in colliderListsThatInteractWithMonster) {
            foreach (var interactingCollider in interactableColliderList) {
                if (monsterCollider.OverlapPoint(interactingCollider.transform.position - velocity) && interactingCollider != monsterCollider)
                    return true;
            }
        }
        return false;
    }

    private static bool IsOverlapping(BoxCollider2D a, BoxCollider2D b)
    {
        return a.OverlapPoint(b.transform.position) || b.OverlapPoint(a.transform.position);
    }

    public static void MonsterMoveRandom(BoxCollider2D playerBoxCollider2D, BoxCollider2D[] boxBoxCollider2Ds, BoxCollider2D[] obstacleBoxCollider2Ds, Monster monster)
    {
        Vector3[] dirs = {
            Vector3.up,
            Vector3.right,
            Vector3.left,
            Vector3.down,
        };
        var inc = 0;
        var checking = true;
        while (checking && inc < 5) {
            //Debug.Log("Checking moves");
            var vel = dirs[Random.Range(1, 4)];
            if (!WillObjectCollide(monster.boxCollider, vel, obstacleBoxCollider2Ds, boxBoxCollider2Ds, playerBoxCollider2D)) {
                monster.transform.localPosition += vel * monster.data.speed;
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
            Vector3.down,
        };

        var inc = (monster.lastDirectionMovedIndex + 1) % dirs.Length - 1;
        var vel = dirs[inc];
        if (!WillObjectCollide(monster.boxCollider, vel, obstacleBoxCollider2Ds, boxBoxCollider2Ds, playerBoxCollider2D)) {
            monster.transform.localPosition += vel;
            monster.lastDirectionMovedIndex = inc;
        }
    }

    public static void MonsterMoveHorizontal(BoxCollider2D playerBoxCollider2D, BoxCollider2D[] boxBoxCollider2Ds, BoxCollider2D[] obstacleBoxCollider2Ds, Monster monster)
    {
        Vector3[] dirs = {
            Vector3.right,
            Vector3.left,
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

    public static void MonsterMoveVertical(BoxCollider2D playerBoxCollider2D, BoxCollider2D[] boxBoxCollider2Ds, BoxCollider2D[] obstacleBoxCollider2Ds, Monster monster)
    {
        Vector3[] dirs = {
            Vector3.up,
            Vector3.down,
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

    public static void MonsterMoveLineOfSight(BoxCollider2D playerBoxCollider2D, BoxCollider2D[] boxBoxCollider2Ds, BoxCollider2D[] obstacleBoxCollider2Ds, Monster monster, float speedX, float speedY)
    {
        var vel = Vector3.zero;
        var playerX = playerBoxCollider2D.transform.localPosition.x;
        var monsterX = monster.boxCollider.transform.localPosition.x;
        var playerY = playerBoxCollider2D.transform.localPosition.y;
        var monsterY = monster.boxCollider.transform.localPosition.y;

        // This is making sure you are on the same column
        if (Mathf.Abs(playerX - monsterX) <= 0.5) {
            // Then check above or below
            if (monsterY > playerY) {
                //go down
                vel = Vector3.down * speedY;
            } else {
                //go up
                vel = Vector3.up * speedY;
            }
            // This is making sure you are on the same row
        } else if (Mathf.Abs(playerY - monsterY) <= 0.5) {
            // Then check left or right
            if (monsterX > playerX) {
                //go left
                vel = Vector3.left * speedX;
            } else {
                //go right
                vel = Vector3.right * speedX;
            }
        }

        //Debug.Log($"{monster.data.name} playerx {playerX} monsterx{monsterX}");
        //Debug.Log($"{monster.data.name} playery {playerY} monstery{monsterY}");
        //Debug.Log($"line of sight vel ${vel}");

        if (!WillMonsterCollide(monster.boxCollider, vel, obstacleBoxCollider2Ds, boxBoxCollider2Ds, new BoxCollider2D[] { playerBoxCollider2D })) {
            monster.transform.localPosition += vel;
        }
    }

    public static bool IsWithinRange(Vector3 center, Vector3 point, float radius)
    {
        var diff = center - point;
        return Mathf.Abs(diff.x) <= radius && Mathf.Abs(diff.y) <= radius;
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
}