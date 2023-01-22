using UnityEngine;

public class TestBoxes : MonoBehaviour
{
    public BoxCollider2D player;
    public BoxCollider2D[] obstacles;
    public BoxCollider2D[] boxes;
    public float speed = 1;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            var velocity = Vector3.left * speed;
            if (!GameManager.WillCollide(player, velocity, obstacles)) {
                var pushable = GameManager.GetPushedBox(player, velocity, boxes);
                if(pushable != null){
                    Debug.Log("IsPushing");
                    // if(GameManager.CanPushBox(pushable, velocity, obstacles, boxes)){
                    //     player.transform.localPosition += velocity;
                    //     pushable.transform.localPosition += velocity;
                    // }
                } 
                else {
                    player.transform.localPosition += velocity;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            var velocity = Vector3.up * speed;
            if (!GameManager.WillCollide(player, velocity, obstacles)) {
                var pushable = GameManager.GetPushedBox(player, velocity, boxes);
                if(pushable != null){
                    Debug.Log("IsPushing");
                    // if(GameManager.CanPushBox(pushable, velocity, obstacles, boxes)){
                    //     player.transform.localPosition += velocity;
                    //     pushable.transform.localPosition += velocity;
                    // }
                } 
                else {
                    player.transform.localPosition += velocity;
                }
            
            }
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            var velocity = Vector3.down * speed;
            if (!GameManager.WillCollide(player, velocity, obstacles)) {
                var pushable = GameManager.GetPushedBox(player, velocity, boxes);
                if(pushable != null){
                    Debug.Log("IsPushing");
                    // if(GameManager.CanPushBox(pushable, velocity, obstacles, boxes)){
                    //     player.transform.localPosition += velocity;
                    //     pushable.transform.localPosition += velocity;
                    // }
                } 
                else {
                    player.transform.localPosition += velocity;
                }
            
            }
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            var velocity = Vector3.right * speed;
            if (!GameManager.WillCollide(player, velocity, obstacles)) {
                var pushable = GameManager.GetPushedBox(player, velocity, boxes);
                if(pushable != null){
                    Debug.Log("IsPushing");
                    // if(GameManager.CanPushBox(pushable, velocity, obstacles, boxes)){
                    //     player.transform.localPosition += velocity;
                    //     pushable.transform.localPosition += velocity;
                    // }
                } 
                else {
                    player.transform.localPosition += velocity;
                }
            
            }
        }
        
    }
}
