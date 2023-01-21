using UnityEngine;

public class TestAABB : MonoBehaviour
{
    public BoxCollider2D player;
    public BoxCollider2D[] obstacles;
    public float speed = 1;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)){
            GameManager.MovePlayer(player, Vector3.left * speed, obstacles);    
        }
        if(Input.GetKeyDown(KeyCode.W)){
            GameManager.MovePlayer(player, Vector3.up * speed, obstacles);
        }
        if(Input.GetKeyDown(KeyCode.S)){
            GameManager.MovePlayer(player, Vector3.down * speed, obstacles);
        }
        if(Input.GetKeyDown(KeyCode.D)){
            GameManager.MovePlayer(player, Vector3.right * speed, obstacles);
        }
    }
}
