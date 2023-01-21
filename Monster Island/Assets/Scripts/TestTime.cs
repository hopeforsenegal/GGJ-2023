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

    // Update is called once per frame
    void Update()
    {
        var v = new Vector3(0.0f, 0.0f, 0.0f);
        if(Input.GetKeyDown(KeyCode.A)){
            Move(Vector3.left * speed);
            for (var i = 0; i <= monsters.Length - 1; i = i + 1) 
            {
                MonsterMove(i);
                if(IsWithinRange(monsters[i].transform.localPosition, player.transform.localPosition, 1)){
                    Die();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.W)){
            Move(Vector3.up * speed);
            for (var i = 0; i <= monsters.Length - 1; i = i + 1) 
            {
                MonsterMove(i);
                if(IsWithinRange(monsters[i].transform.localPosition, player.transform.localPosition, 1)){
                    Die();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.S)){
            Move(Vector3.down * speed);
            for (var i = 0; i <= monsters.Length - 1; i = i + 1) 
            {
                MonsterMove(i);
                if(IsWithinRange(monsters[i].transform.localPosition, player.transform.localPosition, 1)){
                    Die();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.D)){
            Move(Vector3.right * speed);
            for (var i = 0; i <= monsters.Length - 1; i = i + 1) 
            {
                MonsterMove(i);
                if(IsWithinRange(monsters[i].transform.localPosition, player.transform.localPosition, 1)){
                    Die();
                }
            }
        }
        
    }
 
    void Move(Vector3 velocity) {
        if(!WillCollide(velocity)){
            var pushable = GetPushedBox(velocity);
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
        for (var i = 0; i <= boxes.Length - 1; i = i + 1) 
        {
            var box = boxes[i];
            if(player.OverlapPoint(box.transform.position-velocity)){
                return box;
            }
        }
        return null;
    }

    bool CanPushBox(BoxCollider2D box, Vector3 velocity) {
        for (var i = 0; i <= obstacles.Length - 1; i = i + 1) 
        {
            var obstacle = obstacles[i];
            if(box.OverlapPoint(obstacle.transform.position-velocity))
                return false;
        }
        for (var i = 0; i <= boxes.Length - 1; i = i + 1) 
        {
            var b = boxes[i];
            if(box.OverlapPoint(b.transform.position-velocity))
                return false;
        }
        return true;
    }

    bool WillCollide(Vector3 velocity){
        for (var i = 0; i <= obstacles.Length - 1; i = i + 1) 
        {
            var obstacle = obstacles[i];
            if(player.OverlapPoint(obstacle.transform.position-velocity))
                return true;
        }
        return false;
    }

    bool WillObjectCollide(BoxCollider2D box, Vector3 velocity){
        for (var i = 0; i <= obstacles.Length - 1; i = i + 1) 
        {
            var obstacle = obstacles[i];
            if(box.OverlapPoint(obstacle.transform.position-velocity) && obstacle != box)
                return true;
        }
        for (var i = 0; i <= boxes.Length - 1; i = i + 1) 
        {
            var b = boxes[i];
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
        var vel = dirs[Random.Range(1, 4)];
        var inc = 0;
        var checking = true;
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

    bool IsWithinRange(Vector3 center, Vector3 point, float radius){
        var diff = center - point;
        if(Mathf.Abs(diff.x) <= radius && Mathf.Abs(diff.y) <= radius)
            return true;
        return false;
    }

    void Die() {
        Debug.Log("Monster can kill");
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
