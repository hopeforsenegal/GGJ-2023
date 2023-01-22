using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    public int killRadius;
    public int stepsToUpdate;
    public int sleepHour;
    public int wakeHour;
    public string attack;
    public string moveUpDown;
    public string moveLeftRight;

    //create selection list for types of navigation
    public enum NavigationType
    {
        None = 0,
        Random = 1,
        Circular = 2,
        Horizontal = 3,
        Vertical = 4,
        LineOfSight = 5,
    }
    public NavigationType navigationType;
}
