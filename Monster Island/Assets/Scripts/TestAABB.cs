using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAABB : MonoBehaviour
{
    public BoxCollider2D player;
    public BoxCollider2D[] obstacles;
    public float speed = 1;

    
    // Start is called before the first frame update
    void Start()
    {  
    
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = new Vector3(0.0f, 0.0f, 0.0f);
        if(Input.GetKeyDown(KeyCode.A)){
            Move(Vector3.left * speed);    
        }
        if(Input.GetKeyDown(KeyCode.W)){
            Move(Vector3.up * speed);
        }
        if(Input.GetKeyDown(KeyCode.S)){
            Move(Vector3.down * speed);
        }
        if(Input.GetKeyDown(KeyCode.D)){
            Move(Vector3.right * speed);
        }
        
    }

    void Move(Vector3 velocity) {
        if(!WillCollide(velocity)){
            player.transform.localPosition += velocity;
        }
    }

    bool WillCollide(Vector3 velocity){
        for (int i = 0; i <= obstacles.Length - 1; i = i + 1) 
        {
            BoxCollider2D obstacle = obstacles[i];
            if(player.OverlapPoint(obstacle.transform.position-velocity))
                return true;
        }
        return false;
    }

    bool IsAABB(BoxCollider2D a, BoxCollider2D b){
        return a.OverlapPoint(b.transform.position);
    }
}
