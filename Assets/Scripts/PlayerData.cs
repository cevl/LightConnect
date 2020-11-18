using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string levelName;
    public int recordMoves = 0;
    public int recordStars = 0;
    public int unlocked = 0;
    public int completed = 0;

    //public List<string> recordsPaths;
}
