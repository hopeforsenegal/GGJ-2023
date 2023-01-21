using UnityEngine;

public class TestAABB : MonoBehaviour
{
    public BoxCollider2D player;
    public BoxCollider2D[] obstacles;
    public float speed = 1;

    // Update is called once per frame
    void Update()
    {
        Vector3 v = new Vector3(0.0f, 0.0f, 0.0f);
        if(Input.GetKeyDown(KeyCode.A)){
            GameManager.Move(obstacles, player, Vector3.left * speed);    
        }
        if(Input.GetKeyDown(KeyCode.W)){
            GameManager.Move(obstacles, player, Vector3.up * speed);
        }
        if(Input.GetKeyDown(KeyCode.S)){
            GameManager.Move(obstacles, player, Vector3.down * speed);
        }
        if(Input.GetKeyDown(KeyCode.D)){
            GameManager.Move(obstacles, player, Vector3.right * speed);
        }
    }
}
