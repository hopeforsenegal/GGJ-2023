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
            BoxCollider2D pushable = GetPushedBox(velocity);
            if(pushable != null){
                Debug.Log("IsPushing");
                if(CanPushBox(pushable, velocity)){
                    player.transform.localPosition += velocity;
                    pushable.transform.localPosition += velocity;
                }
            } 
            else {
                player.transform.localPosition += velocity;
            }
            
        }
    }

    BoxCollider2D GetPushedBox(Vector3 velocity){
        for (int i = 0; i <= boxes.Length - 1; i = i + 1) 
        {
            BoxCollider2D box = boxes[i];
            if(player.OverlapPoint(box.transform.position-velocity)){
                return box;
            }
        }
        return null;
    }

    bool CanPushBox(BoxCollider2D box, Vector3 velocity) {
        for (int i = 0; i <= obstacles.Length - 1; i = i + 1) 
        {
            BoxCollider2D obstacle = obstacles[i];
            if(box.OverlapPoint(obstacle.transform.position-velocity))
                return false;
        }
        for (int i = 0; i <= boxes.Length - 1; i = i + 1) 
        {
            BoxCollider2D b = boxes[i];
            if(box.OverlapPoint(b.transform.position-velocity))
                return false;
        }
        return true;
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

    // void HandleTime(int delta) {
    //     for (int i = 0; i <= obstacles.Length - 1; i = i + 1) 
    //     {
    //         timeObject.HandleTime(int delta)
    //     }
    // }
}
