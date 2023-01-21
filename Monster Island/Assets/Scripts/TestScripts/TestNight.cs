using UnityEngine;

public class TestNight : MonoBehaviour
{
    public SpriteRenderer night;

    //HOURS 0-8
    //Start at 0am
    //Blobs wakup at 4  go to sleep at 8
    int time = 1;

    void Start()
    {
        if (GameManager.IsNightTime(time)) {
            night.color = Color.black;
        } else {
            night.color = Color.white;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            // night day updates
            if (GameManager.IsNightTime(time)) {
                night.color = Color.black;
            } else {
                night.color = Color.white;
            }

            // increment time
            time = GameManager.IncrementTime(time);
        }
    }
}
