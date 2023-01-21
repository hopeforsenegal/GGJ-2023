using UnityEngine;

public class TestNight : MonoBehaviour
{
    public SpriteRenderer night;
    bool toggle;
    int count;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            var v = Util.IncrementLoop(ref count, 4);
            Debug.Log(v);
            if (v == 4) {
                night.color = Util.Switch(ref toggle) ? Color.black : Color.white;
            }
        }
    }
}
