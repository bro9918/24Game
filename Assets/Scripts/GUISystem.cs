using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GUISystem : MonoBehaviour {
	public TextAsset instructions;

	public GUISkin ourSkin;

	private GUIState currentState;

	// Use this for initialization
	void Awake() {
		InstructionState.SetInstructions(instructions.text);
		ChangeGUIState(new MenuState(this));
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	void OnGUI() {
		currentState.OnGUI();
	}

	internal void ChangeGUIState(GUIState newState, Action callback = null) {
		const float speed = 24;
		currentState = newState;
		StartCoroutine(EnumeratorAppend(MoveCamera(Camera.main, newState.CameraPosition, speed), callback));
	}

	IEnumerator EnumeratorAppend(IEnumerator enumerator, Action callback) {
		do {
			yield return enumerator.Current;
		} while(enumerator.MoveNext());
		if (callback != null) {
			callback();
		}
		yield return null;
	}

	IEnumerator MoveCamera(Camera camera, Vector3 destination, float speed) {
		const float updatesPerSecond = 30;
		Vector3 path = destination - camera.transform.position;
		for (int i = 0; i < path.magnitude * updatesPerSecond / speed; ++i) {
			camera.transform.position += path.normalized * speed / updatesPerSecond;
			yield return new WaitForSeconds(1 / updatesPerSecond);
		}
	}
}

interface GUIState {
	void OnGUI();
	Vector3 CameraPosition { get; }
}

class MenuState : GUIState {
	private GUISystem guiSystem;

	public Vector3 CameraPosition { get; private set; }

	public MenuState(GUISystem system) {
		guiSystem = system;
		CameraPosition = GameObject.Find("MenuCameraPosition").transform.position;
	}

	public void OnGUI() {
		GUILayout.BeginArea(new Rect(Camera.main.pixelWidth / 4, Camera.main.pixelHeight * .6f, Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 3));
		if (GUILayout.Button("Start", guiSystem.ourSkin.button, GUILayout.ExpandHeight(true))) {
			guiSystem.ChangeGUIState(new IngredientsState(guiSystem));
		}
		if (GUILayout.Button("Instructions", guiSystem.ourSkin.button, GUILayout.ExpandHeight(true))) {
			guiSystem.ChangeGUIState(new InstructionState(guiSystem));
		}
		GUILayout.EndArea();
	}
}

class InstructionState : GUIState {
	private static string instructions;

	static InstructionState() {
		instructions = null;
	}
	
	public static void SetInstructions(string newInstructions) {
		if (instructions == null) {
			instructions = newInstructions;
		}
	}

	private GUISystem guiSystem;

	public Vector3 CameraPosition { get; private set; }

	public InstructionState(GUISystem system) {
		guiSystem = system;
		CameraPosition = GameObject.Find("MenuCameraPosition").transform.position;
	}

	public void OnGUI() {
		GUILayout.BeginArea(new Rect(Camera.main.pixelWidth / 4, Camera.main.pixelHeight / 3, Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 3));
		GUILayout.TextArea(instructions, GUILayout.ExpandHeight(true));
		if (GUILayout.Button("Back", guiSystem.ourSkin.button, GUILayout.ExpandHeight(true))) {
			guiSystem.ChangeGUIState(new MenuState(guiSystem));
		}
		GUILayout.EndArea();
	}
}

class IngredientsState : GUIState {
	private GUISystem guiSystem;

	private bool validIngredients;

	public Vector3 CameraPosition { get; private set; }

	public IngredientsState(GUISystem system) {
		guiSystem = system;
		CameraPosition = GameObject.Find("IngredientsCameraPosition").transform.position;
		validIngredients = false;
		CuttingBoard cuttingBoard = GameObject.FindGameObjectWithTag("Cutting Board").GetComponent<CuttingBoard>();
		foreach (DragAndDrop dad in GameObject.Find("Ingredients").GetComponentsInChildren<DragAndDrop>()) {
			dad.IngredientsChanged += (list, count) => {
				validIngredients = count == cuttingBoard.maxIngredients;
			};
		}
	}

	public void OnGUI() {
		GUILayout.BeginArea(new Rect(Camera.main.pixelWidth * .54f, Camera.main.pixelHeight * .71f, Camera.main.pixelWidth / 3, Camera.main.pixelWidth / 5));
		GUI.enabled = validIngredients;
		if (GUILayout.Button("Cook!", guiSystem.ourSkin.button, GUILayout.ExpandHeight(true))) {
			GameObject cuttingBoard = GameObject.FindGameObjectWithTag("Cutting Board");
			Transform[] moveUs = cuttingBoard.GetComponent<CuttingBoard>().ingredients.Cast<GameObject>().Concat(Enumerable.Repeat(cuttingBoard, 1)).Select(x => x.transform).ToArray();
			Dictionary<Transform, Transform> origParent = moveUs.ToDictionary(x => x, x => x.parent);
			foreach (Transform moveMe in moveUs) {
				moveMe.parent = Camera.main.transform;
			}
			guiSystem.ChangeGUIState(new GameState(guiSystem), () => {
				foreach (var kvp in origParent) {
					kvp.Key.parent = kvp.Value;
				}
			});
		}
		GUI.enabled = true;
		if (GUILayout.Button("Back", guiSystem.ourSkin.button, GUILayout.ExpandHeight(true))) {
			guiSystem.ChangeGUIState(new MenuState(guiSystem));
		}
		GUILayout.EndArea();
	}

	IEnumerator MoveCuttingBoard() {
		const float updatesPerSecond = 30;
		const float speed = 24;
		GameObject cuttingBoard = GameObject.FindGameObjectWithTag("Cutting Board");
		Vector3 path = Vector3.right * 12;
		for (int i = 0; i < path.magnitude * updatesPerSecond / speed; ++i) {
			foreach (GameObject go in cuttingBoard.GetComponent<CuttingBoard>().ingredients.Cast<GameObject>().Concat(Enumerable.Repeat(cuttingBoard, 1))) {
				go.transform.position += path.normalized * speed / updatesPerSecond;
				yield return new WaitForSeconds(1 / updatesPerSecond);
			}
		}
	}
}

class GameState : GUIState {
	private GUISystem guiSystem;

	private bool gameWon;

	public Vector3 CameraPosition { get; private set; }

	public GameState(GUISystem system) {
		guiSystem = system;
		CameraPosition = GameObject.Find("GameCameraPosition").transform.position;
		gameWon = false;
	}

	public void OnGUI() {
		if (gameWon) {

		}
	}
}