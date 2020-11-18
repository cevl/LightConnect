using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BlockPlacer : Editor {
    private List<GameObject> blocks;

    private void OnSceneGUI()
    {
        blocks = new List<GameObject> (GameObject.FindGameObjectsWithTag("Block"));
        foreach(GameObject block in blocks)
        {
            Transform transform = block.GetComponent<Transform>();
            transform.position = new Vector3 (Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        }
    }
}
