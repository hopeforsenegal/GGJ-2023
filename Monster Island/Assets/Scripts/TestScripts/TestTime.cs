using UnityEditor;
using UnityEngine;

public class TestTime : MonoBehaviour
{
    public BoxCollider2D player;
    public BoxCollider2D[] obstacles;
    public BoxCollider2D[] boxes;
    public BoxCollider2D[] monsters;
    public float speed = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            var velocity = Vector3.left * speed;
            if (!GameManager.WillCollide(player, velocity, obstacles)) {
                var pushable = GameManager.GetPushedBox(player, velocity, boxes);
                if (pushable != null) {
                    Debug.Log("IsPushing");
                    // if (GameManager.CanPushBox(pushable, velocity, obstacles, boxes)) {
                    //     player.transform.localPosition += velocity;
                    //     pushable.transform.localPosition += velocity;
                    // }
                } else {
                    player.transform.localPosition += velocity;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            var velocity = Vector3.up * speed;
            if (!GameManager.WillCollide(player, velocity, obstacles)) {
                var pushable = GameManager.GetPushedBox(player, velocity, boxes);
                if (pushable != null) {
                    Debug.Log("IsPushing");
                    // if (GameManager.CanPushBox(pushable, velocity, obstacles, boxes)) {
                    //     player.transform.localPosition += velocity;
                    //     pushable.transform.localPosition += velocity;
                    // }
                } else {
                    player.transform.localPosition += velocity;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            var velocity = Vector3.down * speed;
            if (!GameManager.WillCollide(player, velocity, obstacles)) {
                var pushable = GameManager.GetPushedBox(player, velocity, boxes);
                if (pushable != null) {
                    Debug.Log("IsPushing");
                    // if (GameManager.CanPushBox(pushable, velocity, obstacles, boxes)) {
                    //     player.transform.localPosition += velocity;
                    //     pushable.transform.localPosition += velocity;
                    // }
                } else {
                    player.transform.localPosition += velocity;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            var velocity = Vector3.right * speed;
            if (!GameManager.WillCollide(player, velocity, obstacles)) {
                var pushable = GameManager.GetPushedBox(player, velocity, boxes);
                if (pushable != null) {
                    Debug.Log("IsPushing");
                    // if (GameManager.CanPushBox(pushable, velocity, obstacles, boxes)) {
                    //     player.transform.localPosition += velocity;
                    //     pushable.transform.localPosition += velocity;
                    // }
                } else {
                    player.transform.localPosition += velocity;
                }
            }
        }

        if (Input.anyKeyDown) {
            for (var i = 0; i <= monsters.Length - 1; i++) {
                // GameManager.MonsterMoveRandom(player, boxes, obstacles, monsters[i]);
                // if (GameManager.IsWithinRange(monsters[i].transform.localPosition, player.transform.localPosition, 1)) {
                //     Application.Quit();
                //     EditorApplication.isPlaying = false;
                // }
            }
        }
    }
}