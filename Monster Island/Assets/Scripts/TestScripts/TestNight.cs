using UnityEngine;

public class TestNight : MonoBehaviour
{
    public SpriteRenderer night;
    bool isDayOrNight;
    int movementCount;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            // TODO Turn this into a single static function and move it to game manager
            var increment = Util.IncrementLoop(ref movementCount, 4);
            Debug.Log(increment);
            if (increment == 4) {
                night.color = Util.Switch(ref isDayOrNight) ? Color.black : Color.white;
            }
        }
    }
}
