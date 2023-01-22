using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    public int killRadius;
    public int stepsToUpdate;
    public int sleepHour;
    public int wakeHour;
    public bool isRandom;

    public bool isSleep;

}
