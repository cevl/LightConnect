//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;
//
//public class MyScriptableObject : ScriptableObject {
//
//	public int x = 4;
//	public int y = 4;
//	public LevelData level;
//
//	private List<GameObject> objects;
//
//	// Use this for initialization
//	public void Start () {
//		Debug.Log ("MyScriptable");
//
//		//Initializing level.isEmpty. //With the last modification I thin that it is not necesary to inizializate because I think that it is null by default.
//		for (int i = 0; i < x; i++) {
//			for (int j = 0; j < y; j++) {
//				level.isEmpty [x] [y] = null;
//			}
//		}
//
//		GameObject[] objs =  (GameObject[]) FindObjectsOfType<GameObject>().
//			Where(obj => obj.tag == "block"); //Cannot cast from source type to destination type.
//		foreach (GameObject obj in objs) {
//			level.isEmpty [(int) obj.transform.position.x] [(int) obj.transform.position.y] = obj.name;
//
//			if (obj.GetComponent<Block>().isFont)
//				level.fonts.Add (obj);
//			if (obj.GetComponent<Block>().isReceiver)
//				level.receivers.Add (obj);
//		}
//		Debug.Log(level.isEmpty);
//	}
//}
