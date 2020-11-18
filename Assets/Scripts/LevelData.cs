using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelData {

    public string levelName;
    public int difficulty;
    public int levelNumber;
	public int target = 0;
	public int unlocked = 0;    //Si biene desbloqueado desde el comienzo.

    //public List<string> targetPath;

}
