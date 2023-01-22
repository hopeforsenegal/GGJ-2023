using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "ScriptableObjects/SettingsData", order = 1)]
public class SettingsData : ScriptableObject
{
    // from day to night
    public Sprite[] sundialSprites;

    public AudioClip winAudio;
}
