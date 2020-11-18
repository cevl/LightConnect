//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//
//[CustomEditor (typeof(Block))]
//public class BlocksComponentsHider : Editor {
//
//	void OnEnabled() {
//		var castedTarget = (target as Block);
//		castedTarget.GetComponent<BoxCollider2D>().hideFlags = HideFlags.HideInInspector;
//	}
//
//	void OnSceneGuid() {
//		Handles.PositionHandle (Vector3.zero, new Quaternion());
//	}
//}
