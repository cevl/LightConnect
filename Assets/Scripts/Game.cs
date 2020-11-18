using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
	public static LevelData currentLevelData;
    public static PlayerData currentPlayerData;
    public static List<LevelData> levelsData;
    public static List<PlayerData> savedGames;

    private Text RecordText;
    private Text TargetText;
    private Text RecordStarsText;
    private Text LevelNameText;

	// Use this for initialization
	void Start () {
        SaveLoad.Load();

        RecordText = GameObject.FindGameObjectWithTag("RecordText").GetComponent<Text>();
        TargetText = GameObject.FindGameObjectWithTag("TargetText").GetComponent<Text>();
        RecordStarsText = GameObject.FindGameObjectWithTag("RecordStars").GetComponent<Text>();
        LevelNameText = GameObject.FindGameObjectWithTag("LevelNameText").GetComponent<Text>();

        RecordText.text = "Record: " + currentPlayerData.recordMoves.ToString();
        TargetText.text = "Target: " + currentLevelData.target.ToString();
        RecordStarsText.text = "Stars: " + currentPlayerData.recordStars.ToString();
        LevelNameText.text = "Level name: " + currentLevelData.levelName;
	}
}
