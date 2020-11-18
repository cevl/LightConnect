using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Block : MonoBehaviour {

	public bool isFont;
	public bool isReceiver;
	public List<GameObject> childs;
	public float minDistance = 0.05f;
	public static float smoothTime = 0.1f;
	public bool movable;

	private Camera Camera;
	private Text WinText;
	private Text MoveCountText;

	private Vector3 curPos;
	private Vector3 iniMousePosition;
	private Vector3 objIniPos;
	private Vector3 velocity = Vector3.zero;
	private Vector3 moveTo = new Vector3();
	private bool moving = false;
	private bool luz;
	private bool propagate = false;

    public static List<GameObject> fonts = new List<GameObject>();
	public static List<GameObject> receivers = new List<GameObject> ();
    private static bool locked = false;
    private static int moveCount;

	//void Reset() { }

	void Start () {
		Camera = GameObject.FindObjectOfType<Camera> ();
		WinText = GameObject.FindGameObjectWithTag ("WinText").GetComponent<Text>();
		MoveCountText = GameObject.FindGameObjectWithTag ("MoveCountText").GetComponent<Text>();

		if (this.isFont) {
			fonts.Add (this.gameObject);
			Propagate ();
		} else if (this.isReceiver) {
			receivers.Add (this.gameObject);
		}

		this.childs = new List<GameObject>();

		Vector3 moveTo = transform.position;
		if (isFont) {
			luz = true;
		} else {
			luz = false;
		}

        //Restarting static values to avoid problems after change a scene.
        //Estoy reiniciandolo 1 vez por cada bloque, no es lo mas eficiente ni logico.
        moveCount = 0;
        locked = false;
    }
	
	void Update () {
		//This is a test for FindObjectsOfType<Type>()
		if (Input.GetKeyDown(KeyCode.Space)) {
			var foundObjects = FindObjectsOfType<GameObject>();
			print ("Objects count: " + foundObjects.Length);
			foreach (GameObject obj in foundObjects) {
				print ("obj: " + obj + ", name: " + obj.name + ", tag: " + obj.tag + ", layer: " + obj.layer);
			}
		}
	}

	void FixedUpdate() {
		if (moving) {
			transform.position = Vector3.SmoothDamp (transform.position, moveTo, ref velocity, smoothTime);
			if (moveTo == transform.position) {
				moving = false;
				locked = false;
			}
		}
		if (this.propagate)
			this.Propagate ();
	}
		 
	void OnMouseDown () {
		iniMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		print ("OnMouseDown");
	}

	void OnMouseUp () {
		print ("OnMouseUp");
		if (!locked && movable) {	//TODO This should be modified so we can play a sound if false.
			print ("Not locked.");
			Vector3 currMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector3 dire = currMousePosition - iniMousePosition;

			if (Mathf.Abs (dire.x) > minDistance || Mathf.Abs (dire.y) > minDistance) {
				print ("transform.position: " + transform.position.ToString ());
				moveTo = transform.position;	//In case that the new position can't be reached.
				if (Mathf.Abs (dire.x) > Mathf.Abs (dire.y)) {
					print ("x > y");
					if (dire.x > 0) {
						print ("Moving " + this.gameObject.name + " to the right.");
						Vector3 newPosition = transform.position + Vector3.right;

						if (IsPositionClear (newPosition)) {
							moveTo = newPosition;
							print ("moveTo: " + moveTo.ToString ());
						}
					} else {
						print ("Moving " + this.gameObject.name + " to the Left.");
						Vector3 newPosition = transform.position + Vector3.left;

						if (IsPositionClear (newPosition)) {
							moveTo = newPosition;
							print ("moveTo: " + moveTo.ToString ());
						}
					}
				} else {
					if (dire.y > 0) {
						print ("Moving " + this.gameObject.name + " upwards.");
						Vector3 newPosition = transform.position + Vector3.up;

						if (IsPositionClear (newPosition)) {
							moveTo = newPosition;
							print ("moveTo: " + moveTo.ToString ());
						}
					} else {
						print ("Moving " + this.gameObject.name + " down.");
						Vector3 newPosition = transform.position + Vector3.down;

						if (IsPositionClear (newPosition)) {
							moveTo = newPosition;
							print ("moveTo: " + moveTo.ToString ());
						}
					}
				}
				if (moveTo != transform.position) {
					moveCount++;
					MoveCountText.text = "Move: " + moveCount.ToString ();
					moving = true;
					locked = true;
					AudioManager.instance.Play("slip");
				}
			} else {
				print("No se supero la distancia minima para mover los bloques.");
			}
		}
	}
		
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag ("Block")) {
			childs.Add (other.gameObject);
			if (other.gameObject.GetComponent<Block> ().luz == true) {
				propagate = true;
			}
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		childs.Remove (other.gameObject);
		if (this.luz) {
			if (this.moving) {
				if (!this.isFont) {
					this.SetLuz (false);
				} else {
					other.gameObject.GetComponent<Block> ().IsConnected ();
				}
			} else {
				IsConnected ();
			}
		}
	}

	void IsConnected (bool isFirst = true) {
//		visitedBlocks.Add (this.gameObject);
		if (!this.isFont) {
			this.SetLuz(false); //This was true.
			try {
				foreach (GameObject child in childs) {
//					if(!visitedBlocks.Find(obj => obj.GetInstanceID() == child.GetInstanceID()))
					if(child.GetComponent<Block>().luz)
						child.GetComponent<Block> ().IsConnected (false);
				}
				if(isFirst)
					PropagateFonts();
			} catch {
				print ("Don't have childrens");
			}
		}
	}

	void Propagate () {
		this.SetLuz (true);
		print ("Propagating from: " + this.gameObject.name);
		foreach (GameObject child in childs) {
			Block block = child.GetComponent<Block> ();
			if (!block.luz) {
				print ("Propagating from " + this.gameObject.name + " to: " + block.gameObject.name);
				block.Propagate ();
			} else {
				print ("No propagating from " + this.gameObject.name + " to: " + block.gameObject.name + " have luz.");
			}
		}
		if(this.propagate)
			this.propagate = false;
	}

	void PropagateFonts () {
		foreach (GameObject font in fonts)
			font.GetComponent<Block> ().Propagate ();
	}

	void SetLuz(bool isConnected) {
		if (this.luz != isConnected && !this.isFont) {
			this.luz = isConnected;

			if (isConnected) {
				GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite>(gameObject.name + "Active");
				if (this.isReceiver)
					DidIWin ();
			} else {
				GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite>(gameObject.name);
			}
		}
	}

	bool IsPositionClear(Vector3 position) {
		bool isClear = false;

		RaycastHit2D hit = Physics2D.Raycast (position, Vector2.zero);
		if(hit != false) {
			if (hit.collider.CompareTag ("Background")) {
				isClear = true;
//				print ("Raycast hit on Background");
			} else {
				print ("Ratcast don't hit on Background.");
			}
		} else {
			print ("Raycast don't hit anything.");
		}
		Debug.DrawLine(position, Vector2.zero);
		return isClear;
	}

	void DidIWin() {
		bool win = true;
		foreach (GameObject receiver in receivers) {
			if (!receiver.GetComponent<Block> ().luz)
				win = false;
		}
		if(win)
			Win ();
	}

	void Win() {
		Debug.Log ("Ganaste");
		WinText.text = "You win";

        if(Game.currentPlayerData.completed == 0)
        {
            Game.currentPlayerData.recordMoves = moveCount;
            Game.currentPlayerData.completed = 1;
//                        Game.currentPlayerData.recordStars = calStars();
            SaveLoad.Save();
        }
		else if (moveCount < Game.currentPlayerData.recordMoves || Game.currentPlayerData.recordMoves == 0)
        {
            Game.currentPlayerData.recordMoves = moveCount;
//                        Game.currentPlayerData.recordStars = calStars();
            SaveLoad.Save();
        }
	}
}
