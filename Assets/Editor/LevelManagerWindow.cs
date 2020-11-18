//#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LevelManagerWindow : EditorWindow {
    private List<GameObject> blocks;
	private bool arrange = true;
	private GameObject[,] board;
	private GameObject Background;
    private bool autoChildren = true; //This make the blocks to be childs of a GameObject with tag "BlocksParent" for organization.

	public int width = 3;
	public int height = 3;

    [MenuItem ("Window/Level manager")]
	public static void ShowWindow () {
		GetWindow<LevelManagerWindow> ("Level manager");
	}

	void OnGUI() {
        if (Game.currentLevelData == null)
        {
            SaveLoad.Load();
            Debug.Log("current == null, loaded.");
        }
        if (Game.currentLevelData.levelName != SceneManager.GetActiveScene().name) {
            SaveLoad.Load();
            Debug.Log("levelName != activeScene.name, loaded.");
        }

        Background = GameObject.FindGameObjectWithTag("Background");
        if (Background != null)
        {
            width = (int)Background.transform.localScale.x;
            height = (int)Background.transform.localScale.y;
        }

        GUILayout.Label("Level data", EditorStyles.boldLabel);
        Game.currentLevelData.levelName = EditorGUILayout.TextField("Name", Game.currentLevelData.levelName);
        Game.currentLevelData.levelNumber = int.Parse(EditorGUILayout.TextField("Number", Game.currentLevelData.levelNumber.ToString()));
        Game.currentLevelData.difficulty = int.Parse(EditorGUILayout.TextField("Difficulty", Game.currentLevelData.difficulty.ToString()));
		Game.currentLevelData.target = int.Parse(EditorGUILayout.TextField ("Target", Game.currentLevelData.target.ToString()));

        GUILayout.Label("Player data", EditorStyles.boldLabel);
		Game.currentPlayerData.recordMoves = int.Parse(EditorGUILayout.TextField ("Record moves", Game.currentPlayerData.recordMoves.ToString()));
		Game.currentPlayerData.recordStars = int.Parse(EditorGUILayout.TextField ("Record stars", Game.currentPlayerData.recordStars.ToString()));

		if(GUILayout.Button("Save")) {
            //LevelDataAssetCreator.CreateLevelDataAsset(Game.levelsData);
            SaveLoad.Save ();
			SaveLoad.Load ();
		}

		GUILayout.Label ("Level Editor", EditorStyles.boldLabel);
		arrange = EditorGUILayout.Toggle ("Arrange blocks", arrange);
        autoChildren = EditorGUILayout.Toggle("Make blocks childlren", autoChildren);
		width = EditorGUILayout.IntField ("Board width", width);
		height = EditorGUILayout.IntField ("Board height", height);
		if (GUILayout.Button ("Rotate board to the left"))
			RotateBoardToLeft();

		//There is not a RotateBoardToRight for now...
		if (GUILayout.Button ("Rotate board to the right")) {
			RotateBoardToLeft ();
			RotateBoardToLeft ();
			RotateBoardToLeft ();
		}

		if (arrange) {
			blocks = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Block"));
			foreach (GameObject block in blocks) {
				Transform transform = block.GetComponent<Transform> ();
				transform.position = new Vector3 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y), 0);
			}
		}

        if (autoChildren)
        {
            List<GameObject> blocks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Block"));
            GameObject parent = GameObject.FindGameObjectWithTag("BlocksParent");

            foreach (GameObject block in blocks)
                block.transform.parent = parent.transform;
        }

        AdjustBoard(width, height);
    }
		
    public void ReadBoard() {
        board = new GameObject[height, width];
        GameObject obj;
        RaycastHit2D[] hit = new RaycastHit2D[1];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
//      filter.useLayerMask = true;
//		filter.SetLayerMask(LayerMask.NameToLayer("Blocks"));


        for(int x = 0; x < height; x++){
            for(int y = 0; y < width; y++){
                obj = null;

				if (Physics2D.Raycast(new Vector2((float)x, (float)y), Vector2.zero, filter, hit) == 1 && hit[0].collider.gameObject.name != "Background")
                {
                    obj = hit[0].collider.gameObject;
					board[x, y] = obj;
                }
            }
        }
    }

    public void AdjustBoard(int w, int h)
    {
        float x = (float)w;
        float y = (float)h;

        if (Background == null)
        {
            Background = GameObject.FindGameObjectWithTag("Background");
            if (Background == null)
            {
                string backgroundPath = "Assets/Prefabs/Background.prefab";
                Background = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath(backgroundPath, typeof(GameObject)));
                Debug.Log("Background instantiated: " + backgroundPath);
            }
        }
        Background.transform.localScale = new Vector3(x, y, 0);
        Background.transform.position = new Vector3((x - 1) / 2, (y - 1) / 2, 1);
    }

	public void RotateBoardToLeft() {
		int blockRotation;
		int newRotation;
		string rotatedBlock;
		GameObject newBlock;
		float newX;
		float newY;

		ReadBoard ();
		for(int x = 0; x < width; x++) {
			for(int y = 0; y < height; y++) {
				if (board [x, y] != null) {
					blockRotation = board [x, y].name [1] - '0'; // - '0' para pasarlo a int.
					if (blockRotation == 0) {
						newRotation = blockRotation;
					} else if (blockRotation == 4) {
						newRotation = 1;
					} else {
						newRotation = blockRotation + 1;
					}

					rotatedBlock = board [x, y].name.Replace (blockRotation.ToString(), newRotation.ToString());
					DestroyImmediate (board [x, y]);
						
					newBlock = (GameObject) PrefabUtility.InstantiatePrefab (Resources.Load ("Blocks/" + rotatedBlock));

					if (newBlock != null) {
						newX = (height - 1) - y;
						newY = x;
						newBlock.transform.position = new Vector3 (newX, newY, 0);
					}
				}
			}
		}
	}
}

//#endif