using UnityEngine;

public class TestNight : MonoBehaviour
{
    public SpriteRenderer night;
    bool isDayOrNight;
    int movementCount;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.OnDayNightCycle(ref isDayOrNight, ref movementCount, night);
        }
    }
}
