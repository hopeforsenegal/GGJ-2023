using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

        
    }

    float speed = 1;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)){
            transform.localPosition += Vector3.left * speed;
        }
        if(Input.GetKeyDown(KeyCode.W)){
            transform.localPosition += Vector3.up * speed;
        }
        if(Input.GetKeyDown(KeyCode.S)){
            transform.localPosition += Vector3.down * speed;
        }
        if(Input.GetKeyDown(KeyCode.D)){
            transform.localPosition += Vector3.right * speed;
        }

    }
}
