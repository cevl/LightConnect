//#if (UNITY_EDITOR) 
//
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class Solver : MonoBehaviour {
//    public int width = 4;
//    public int height = 4;
//    private GameObject[,] board;
//    private List<GameObject> fonts;
//    private List<GameObject> receivers;
//    private List<Vector2> notBlock; //Where the position is empty and we can move a block there.
//
//    public void Start() {
//        board = new GameObject[height, width];
//        GameObject obj;
//        RaycastHit2D[] hit = new RaycastHit2D[1];
//        ContactFilter2D filter = new ContactFilter2D();
//        filter.useTriggers = true;
//        filter.useLayerMask = true;
//        filter.SetLayerMask(LayerMask.NameToLayer("Blocks"));
//
//
//        for(int f = 0; f < height; f++){
//            for(int c = 0; c < width; c++){
//                obj = null;
//
//                if (Physics2D.Raycast(new Vector2((float)c, (float)f), Vector2.zero, filter, hit) == 1)
//                {
//                    obj = hit[0].collider.gameObject;
//                    if(obj.GetComponent<Block>().isFont)
//                    {
//                        fonts.Add(obj);
//                    } else if (obj.GetComponent<Block>().isReceiver)
//                    {
//                        receivers.Add(obj);
//                    }
//                } else
//                {
//                    notBlock.Add(new Vector2((float)c, (float)f));
//                    print("No 1 in (" + width + ", " + height + ").");
//                }
//
//                board[f, c] = obj;
//            }
//        }
//    }
//
//    public void Solve(GameObject[,] board, List<Vector2> notBlock)
//    {
//
//    }
//
//    public bool WinCheck(GameObject[,] board) {
//		bool win = true;
//      	foreach(GameObject font in fonts) {
//        	Propagate(font);
//      }
//		foreach (GameObject receiver in receivers) { //Improve this, don't use objects from the game.
//			if (!receiver.GetComponent<Block>.luz) {
//				win = false;
//			}
//		}
//    }
//
//    public void Propagate(GameObject block) {
//      //this.on = true;
//      foreach(Direction direction in block.direction) {
//        if(board[direction.x, direction.y].CanConnect(block.position())) {
//          Progagate(board[direction.x, direction.y]);
//        }
//      }
//    }
//}
//
//#endif