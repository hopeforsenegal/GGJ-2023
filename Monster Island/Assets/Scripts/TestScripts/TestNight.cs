using UnityEngine;

public class TestNight : MonoBehaviour
{
    public SpriteRenderer night;

    //HOURS 0- 24
    //Start at 6 am
    //Blobs wakup at 9am go to sleep at 10pm
    int time = 6;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            // night day updates
            if (time < 12) {
                night.color = Color.black;
            } else {
                night.color = Color.white;
            }

            // increment time
            time += 1;
            if (time >= 24) time = 0;
        }
    }
}
