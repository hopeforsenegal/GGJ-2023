using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoxes : MonoBehaviour
{
    public BoxCollider2D player;
    public BoxCollider2D[] obstacles;
    public BoxCollider2D[] boxes;
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


    //TODO: check if box will collide with obstacle if so, dont move player or box. 
    void Move(Vector3 velocity) {
        if(!WillCollide(velocity)){
            PushBox(velocity);
            player.transform.localPosition += velocity;
            
        }
    }

    void PushBox(Vector3 velocity){
        for (int i = 0; i <= boxes.Length - 1; i = i + 1) 
        {
            BoxCollider2D box = boxes[i];
            if(player.OverlapPoint(box.transform.position-velocity)){
                box.transform.localPosition += velocity;
                return;
            }
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
}
