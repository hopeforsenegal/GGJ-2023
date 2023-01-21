using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTime : MonoBehaviour
{
    public BoxCollider2D player;
    public BoxCollider2D[] obstacles;
    public BoxCollider2D[] boxes;
    public BoxCollider2D[] monsters;
    public float speed = 1;

    
    // Start is called before the first frame update
    void Start()
    {  
        //timeEntities = GetComponents<TimeEntity>();
        //Debug.Log($"initing timeEntities ${timeEntities}");
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = new Vector3(0.0f, 0.0f, 0.0f);
        if(Input.GetKeyDown(KeyCode.A)){
            Move(Vector3.left * speed);
            HandleTime(1); 
        }
        if(Input.GetKeyDown(KeyCode.W)){
            Move(Vector3.up * speed);
            HandleTime(1);
        }
        if(Input.GetKeyDown(KeyCode.S)){
            Move(Vector3.down * speed);
            HandleTime(1); 
        }
        if(Input.GetKeyDown(KeyCode.D)){
            Move(Vector3.right * speed);
            HandleTime(1);
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

    bool WillObjectCollide(BoxCollider2D box, Vector3 velocity){
        for (int i = 0; i <= obstacles.Length - 1; i = i + 1) 
        {
            BoxCollider2D obstacle = obstacles[i];
            if(box.OverlapPoint(obstacle.transform.position-velocity) && obstacle != box)
                return true;
        }
        for (int i = 0; i <= boxes.Length - 1; i = i + 1) 
        {
            BoxCollider2D b = boxes[i];
            if(box.OverlapPoint(b.transform.position-velocity) && b != box)
                return true;
        }

        if(box.OverlapPoint(player.transform.position - velocity))
            return true;

        return false;
    }
    void MonsterMove(int index){
         Vector3[] dirs = {
            Vector3.up,
            Vector3.right,
            Vector3.left,
            Vector3.down
        };
        Vector3 vel = dirs[Random.Range(1, 4)];
        int inc = 0;
        bool checking = true;
        while(checking && inc < 5){
            Debug.Log("Checking moves");
            vel = dirs[Random.Range(1, 4)];
            if(!WillObjectCollide(monsters[index], vel)){
                monsters[index].transform.localPosition += vel;
                Debug.Log("moving monster");
                checking = false;
            }
            inc ++;
        }
        
    }

    void HandleTime(int delta) {
        for (int i = 0; i <= monsters.Length - 1; i = i + 1) 
        {
            MonsterMove(i);
        }
    }
}
