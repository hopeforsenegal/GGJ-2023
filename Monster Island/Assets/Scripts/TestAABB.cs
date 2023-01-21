using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAABB : MonoBehaviour
{
    public BoxCollider2D a;
    public BoxCollider2D b;
    
    // Start is called before the first frame update
    void Start()
    {  
    
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)){
            var isCollision = IsAABB(a, b);
            Debug.Log($"is collided: {isCollision}");
        }
        
    }

    bool IsAABB(BoxCollider2D a, BoxCollider2D b){
        return a.OverlapPoint(b.transform.position);
    }
}
