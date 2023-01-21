using UnityEngine;

public class TestNight : MonoBehaviour
{
    public SpriteRenderer night;
    bool isDayOrNight;
    int movementCount;
    public const int NumMovesInTimePeriod = 4;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.OnDayNightCycle(ref isDayOrNight, ref movementCount, night);
        }
    }
}
