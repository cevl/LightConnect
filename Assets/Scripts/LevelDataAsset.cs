﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelDataAsset : ScriptableObject
{
    public List<LevelData> levelsData;
}