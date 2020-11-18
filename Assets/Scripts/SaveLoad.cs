using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public static class SaveLoad {

	//it's static so we can call it from anywhere
	public static void Save(bool replaceActual = true) {
        if(replaceActual)
            Game.savedGames.RemoveAll(pdata => pdata.levelName == Game.currentPlayerData.levelName); //Talvez se puede omitir esto.

		Game.savedGames.Add(Game.currentPlayerData);
		BinaryFormatter bf = new BinaryFormatter();
		//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
		FileStream file = File.Create (Application.persistentDataPath + "/player_data.sv");
		bf.Serialize(file, Game.savedGames);
		file.Close();

#if UNITY_EDITOR
        Game.levelsData.RemoveAll(pdata => pdata.levelName == Game.currentPlayerData.levelName); //Talvez se puede omitir esto.
        Game.levelsData.Add(Game.currentLevelData);
        bf = new BinaryFormatter();
        file = File.Create(Application.streamingAssetsPath + "/level_data.data");
        bf.Serialize(file, Game.levelsData);
        file.Close();
#endif
        Debug.Log("Saved.");
	}   

	public static void Load() {
        //Loading Player Data.
		if(File.Exists(Application.persistentDataPath + "/player_data.sv")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/player_data.sv", FileMode.Open);
			Game.savedGames = (List<PlayerData>)bf.Deserialize(file);
			file.Close();

            Game.currentPlayerData = Game.savedGames.Find(dl => dl.levelName == SceneManager.GetActiveScene().name);
		} else
        {
            Game.savedGames = new List<PlayerData>();
        }
        if(Game.currentPlayerData == null)
        {
            Game.currentPlayerData = new PlayerData();
            Game.currentPlayerData.levelName = SceneManager.GetActiveScene().name;
        }

        //Loading Levels Data.
        Debug.Log("Loading levels data");
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!File.Exists(Application.persistentDataPath + "/level_data.data"))
            {
                WWW loadLevelData = new WWW(Application.streamingAssetsPath + "/level_data.data");
                //while (!reader.isDone) //There are people doing this, but don't look like this will be necessary.
                File.WriteAllBytes(Application.persistentDataPath + "/level_data.data", loadLevelData.bytes);
            }
            BinaryFormatter bf2 = new BinaryFormatter();
            FileStream file2 = File.Open(Application.persistentDataPath + "/level_data.data", FileMode.Open);
            Game.levelsData = (List<LevelData>)bf2.Deserialize(file2);
            file2.Close();

            Game.currentLevelData = Game.levelsData.Find(ld => ld.levelName == SceneManager.GetActiveScene().name);

        } else if (File.Exists(Application.streamingAssetsPath + "/level_data.data"))
        {
            Debug.Log("File exist");
            BinaryFormatter bf2 = new BinaryFormatter();
            FileStream file2 = File.Open(Application.streamingAssetsPath + "/level_data.data", FileMode.Open);
            Game.levelsData = (List<LevelData>)bf2.Deserialize(file2);
            file2.Close();
          
            Game.currentLevelData = Game.levelsData.Find(ld => ld.levelName == SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log("File don't exits");
            Game.levelsData = new List<LevelData>();
        }

        if (Game.currentLevelData == null) //This should only happen in the Unity Editor.
        {
            Debug.Log("Current level data is 'null'.");
            Game.currentLevelData = new LevelData();
            Game.currentLevelData.levelName = SceneManager.GetActiveScene().name;
        }
    }
}