using MoonlitSystem.Util;
using UnityEngine;

public class TestNight : MonoBehaviour
{
    public SpriteRenderer night;
    bool toggle;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            night.color = InlineToggle.Switch(ref toggle) ? Color.black : Color.white;
        }
    }
}
