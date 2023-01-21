using UnityEngine;

public class TestAABB : MonoBehaviour
{
    public BoxCollider2D player;
    public BoxCollider2D[] obstacles;
    public float speed = 1;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            var velocity = Vector3.left * speed;
            if (!GameManager.WillCollide(player, velocity, obstacles)) {
                player.transform.localPosition += velocity;
            }
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            var velocity = Vector3.up * speed;
            if (!GameManager.WillCollide(player, velocity, obstacles)) {
                player.transform.localPosition += velocity;
            }
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            var velocity = Vector3.down * speed;
            if (!GameManager.WillCollide(player, velocity, obstacles)) {
                player.transform.localPosition += velocity;
            }
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            var velocity = Vector3.right * speed;
            if (!GameManager.WillCollide(player, velocity, obstacles)) {
                player.transform.localPosition += velocity;
            }
        }
    }
}
