using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    public int killRadius;
    public int stepsToUpdate;
    public int sleepHour;
    public int wakeHour;
    public int speed;
    
    //create selection list for types of navigation
    public enum NavigationType
    {
        Random = 0,
        Circular = 1,
        Horizontal = 2,
        Vertical = 3,
        LineOfSight = 4,
    }
    public NavigationType navigationType;
}
