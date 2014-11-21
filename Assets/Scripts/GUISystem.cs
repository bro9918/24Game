using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GUISystem : MonoBehaviour {
	public TextAsset instructions;
	public GameObject genericFusionPrefab;
	public GUISkin ourSkin;

	private GUIState currentState;
	public Dictionary<GameObject, Vector3> IngredientPositions { get; private set; }

	// Use this for initialization
	void Start() {
		InstructionState.SetInstructions(instructions.text);
		GameState.SetGenericFusionPrefab(genericFusionPrefab);
		ChangeGUIState(new MenuState(this));
		IngredientPositions = GameObject.FindGameObjectsWithTag("Ingredient").ToDictionary(x => x, x => x.transform.position);
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	void OnGUI() {
		currentState.OnGUI();
	}

	void OnMouseDown() {
		currentState.OnMouseDown();
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
		const float updatesPerSecond = 60;
		Vector3 path = destination - camera.transform.position;
		for (int i = 0; i < path.magnitude * updatesPerSecond / speed; ++i) {
			camera.transform.position += path.normalized * speed / updatesPerSecond;
			yield return new WaitForSeconds(1 / updatesPerSecond);
		}
	}

	public void ResetGame() {
		foreach(var kvp in IngredientPositions) {
			kvp.Key.transform.position = kvp.Value;
		}
	}
}

public abstract class GUIState {
	public abstract Vector3 CameraPosition { get; set; }
	public virtual void OnGUI() {
	}
	public virtual void OnMouseDown() {
	}
}

class MenuState : GUIState {
	private GUISystem guiSystem;

	public override Vector3 CameraPosition { get; set; }

	public MenuState(GUISystem system) {
		guiSystem = system;
		CameraPosition = GameObject.Find("MenuCameraPosition").transform.position;
	}

	public override void OnGUI() {
		GUILayout.BeginArea(new Rect(Camera.main.pixelWidth / 4, Camera.main.pixelHeight * .6f, Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 3));
		if (GUILayout.Button("Start", guiSystem.ourSkin.button, GUILayout.ExpandHeight(true))) {
			IngredientsState next = new IngredientsState(guiSystem);
			guiSystem.ChangeGUIState(next, next.ShowGUI);
		}
		if (GUILayout.Button("Instructions", guiSystem.ourSkin.button, GUILayout.ExpandHeight(true))) {
			guiSystem.ChangeGUIState(new InstructionState(guiSystem));
		}
		GUILayout.EndArea();
	}
}

class InstructionState : GUIState {
	private static string[] paragraphs;

	static InstructionState() {
		paragraphs = null;
	}

	public static void SetInstructions(string instructions) {
		if (paragraphs == null) {
			paragraphs = instructions.Replace("\r", "").Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
		}
	}

	private GUISystem guiSystem;
	private int currentPageNumber;
	private Vector2 scrollPos;

	public override Vector3 CameraPosition { get; set; }

	public InstructionState(GUISystem system) {
		guiSystem = system;
		currentPageNumber = 0;
		scrollPos = new Vector2(Camera.main.pixelWidth * .7f, Camera.main.pixelHeight * .6f);
		CameraPosition = GameObject.Find("MenuCameraPosition").transform.position;
	}

	public override void OnGUI() {
		GUILayout.BeginArea(new Rect(Camera.main.pixelWidth / 4, Camera.main.pixelHeight * .6f, Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 3));
		scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(Camera.main.pixelHeight / 4));
		GUILayout.TextArea(paragraphs[currentPageNumber], guiSystem.ourSkin.textArea, GUILayout.ExpandHeight(true));
		GUILayout.EndScrollView();
		GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
		Action backButton = () => {
			if (GUILayout.Button("Main Menu", guiSystem.ourSkin.button, GUILayout.ExpandHeight(true), GUILayout.Width(Camera.main.pixelWidth / 4))) {
				guiSystem.ChangeGUIState(new MenuState(guiSystem));
			}
		};
		Action<bool> pageMove = next => {
			if (GUILayout.Button(next ? ">" : "<", guiSystem.ourSkin.button, GUILayout.ExpandHeight(true), GUILayout.Width(Camera.main.pixelWidth / 4))) {
				currentPageNumber += next ? 1 : -1;
				scrollPos = new Vector2(Camera.main.pixelWidth * .7f, Camera.main.pixelHeight * .6f);
			}
		};
		if (currentPageNumber == 0) {
			backButton();
		} else {
			pageMove(false);
		}
		if (currentPageNumber == paragraphs.Length - 1) {
			backButton();
		} else {
			pageMove(true);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}

class IngredientsState : GUIState {
	private GUISystem guiSystem;

	private bool validIngredients;

	private bool showGUI;

	public override Vector3 CameraPosition { get; set; }

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
		showGUI = false;
	}

	public void ShowGUI() {
		showGUI = true;
	}

	public override void OnGUI() {
		if(!showGUI) {
			return;
		}
		GUILayout.BeginArea(new Rect(Camera.main.pixelWidth * .54f, Camera.main.pixelHeight * .71f, Camera.main.pixelWidth / 3, Camera.main.pixelWidth / 5));
		GUI.enabled = validIngredients;
		if (GUILayout.Button("Cook!", guiSystem.ourSkin.button, GUILayout.ExpandHeight(true))) {
			Transform[] moveUs = GameObject.FindGameObjectWithTag("Cutting Board").GetComponent<CuttingBoard>().ingredients.Cast<GameObject>().Select(x => x.transform).ToArray();
			Dictionary<Transform, Transform> origParent = moveUs.ToDictionary(x => x, x => x.parent);
			foreach (Transform moveMe in moveUs) {
				moveMe.parent = Camera.main.transform;
				moveMe.gameObject.GetComponent<DragAndDrop>().onBoard = false;
			}
			var next = new GameState(guiSystem);
			guiSystem.ChangeGUIState(next, () => {
				foreach (var kvp in origParent) {
					kvp.Key.parent = kvp.Value;
				}
				next.OriginalPositions = next.Ingredients.ToDictionary(x => x.transform, x => x.transform.position);
				next.ShowGUI();
			});
		}
		GUI.enabled = true;
		if (GUILayout.Button("Back", guiSystem.ourSkin.button, GUILayout.ExpandHeight(true))) {
			guiSystem.ChangeGUIState(new MenuState(guiSystem));
		}
		GUILayout.EndArea();
	}
}

public class GameState : GUIState {
	private static GameObject genericFusionPrefab;

	private static Dictionary<string, Func<int, int, int?>> operations = new Dictionary<string, Func<int, int, int?>> {
		{"Add", (x, y) => x + y},
		{"Subtract", (x, y) => Math.Abs(x - y)},
		{"Multiply", (x, y) => x * y},
		{"Divide", (x, y) => {
				Debug.Log(x + " ; " + y); 
				int max = Math.Max(x, y);
				int min = Math.Min(x, y);
				if(max % min == 0) {
					return max / min;
				}
				return null;
		}}
	};

	static GameState() {
		genericFusionPrefab = null;
	}

	public static void SetGenericFusionPrefab(GameObject prefab) {
		genericFusionPrefab = prefab;
	}

	private GUISystem guiSystem;

	private bool gameWon;

	private bool showGUI;

	public Dictionary<Transform, Vector3> OriginalPositions { get; set; }

	public HashSet<GameObject> Ingredients { get; set; }

	public Dictionary<GameObject, GameObject> foodBoxToIngredients;

	public Dictionary<GameObject, int> ingredientsToNums;

	public HashSet<GameObject> genericFusions;

	public int activeIngredients;

	public override Vector3 CameraPosition { get; set; }

	public GameState(GUISystem system) {
		guiSystem = system;
		CameraPosition = GameObject.Find("GameCameraPosition").transform.position;
		gameWon = false;
		foodBoxToIngredients = new Dictionary<GameObject, GameObject>();
		genericFusions = new HashSet<GameObject>();
		ingredientsToNums = new Dictionary<GameObject, int>();
		Ingredients = new HashSet<GameObject>(GameObject.FindGameObjectWithTag("Cutting Board").GetComponent<CuttingBoard>().ingredients.Cast<GameObject>());
		activeIngredients = Ingredients.Count;
		int i = 0;
		//TODO: Remember to reset the event when resetting level.
		foreach (GameObject ingredient in Ingredients) {
			var ingredient2 = ingredient;
			ingredientsToNums[ingredient] = (int)ManageMath.instance.numberList[i++];
			ingredient2.GetComponent<DragAndDrop>().FoodBoxDrop += foodBox => {
				foodBoxToIngredients[foodBox] = ingredient2;
			};
		}
		foreach (OperatorBox op1 in GameObject.Find("Operators").GetComponentsInChildren<OperatorBox>()) {
			op1.OnMouseDownEvent += opBox => {
				//Debug.Log("Sdlfjskdfjsdkfjlsk");
				if (foodBoxToIngredients.Count != 2) {
			//		Debug.Log("Sdlfk");
					return;
				}
				GameObject[] ingredients = foodBoxToIngredients.Values.ToArray();
				Debug.Log(ingredients.Length);
				Func<int, int, int?> op;
				int? answer;
				if (operations.TryGetValue(opBox.name, out op) && (answer = op(ingredientsToNums[ingredients[0]], ingredientsToNums[ingredients[1]])).HasValue) {
					if(activeIngredients == 2 && answer.Value == GameObject.FindGameObjectWithTag("Cutting Board").GetComponent<ManageMath>().TargetNumber) {
						gameWon = true;
					}
					GameObject fusion = (GameObject)GameObject.Instantiate(genericFusionPrefab);
					do {
						fusion.transform.position = new Vector3(UnityEngine.Random.Range(20f, 26f), UnityEngine.Random.Range(2f, -4f), -1);
					} while(fusion.transform.position.x > 24 && fusion.transform.position.y < 0);
					fusion.name = ingredients[0].name + "\n" + ingredients[1].name;
					genericFusions.Add(fusion);
					ingredientsToNums[fusion] = answer.Value;
					foreach (GameObject ingredient in foodBoxToIngredients.Values) {
						ingredient.transform.position += Vector3.up * 1000;
					}
					foodBoxToIngredients.Clear();
					fusion.GetComponent<DragAndDrop>().FoodBoxDrop += foodBox => {
						foodBoxToIngredients[foodBox] = fusion;
					};
					--activeIngredients;
				}
			};
		}
		showGUI = false;
	}

	public void ShowGUI() {
		showGUI = true;
	}

	public override void OnGUI() {
		if(!showGUI) {
			return;
		}
		GUILayout.BeginArea(new Rect(Camera.main.pixelWidth * .55f, Camera.main.pixelHeight / 6, Camera.main.pixelWidth / 4, Camera.main.pixelHeight / 4));
		if (GUILayout.Button("Reset", guiSystem.ourSkin.button, GUILayout.ExpandHeight(true))) {
			foreach (var kvp in OriginalPositions) {
				kvp.Key.position = kvp.Value;
			}
			foodBoxToIngredients.Clear();
			foreach(var gf in genericFusions) {
				GameObject.Destroy(gf);
				ingredientsToNums.Remove(gf);
			}
			genericFusions.Clear();
			activeIngredients = Ingredients.Count;
			gameWon = false;
			//Destroy(genericFusionPrefab);
		}
		GUI.enabled = gameWon;
		if (GUILayout.Button(gameWon ? "Continue" : string.Empty, gameWon ? guiSystem.ourSkin.button : guiSystem.ourSkin.box, GUILayout.ExpandHeight(true))) {
			foreach(var ingredient in Ingredients) {
				ingredient.GetComponent<DragAndDrop>().FoodBoxDrop = null;
			}
			NutritionState next = new NutritionState(guiSystem);
			guiSystem.ChangeGUIState(next, next.ShowGUI);
		}
		GUI.enabled = true;
		GUILayout.EndArea();
		foreach (var kvp in ingredientsToNums) {
			Vector3 screenPoint = Camera.main.WorldToScreenPoint(kvp.Key.transform.position);
			GUILayout.BeginArea(new Rect(screenPoint.x, Camera.main.pixelHeight - screenPoint.y, 200, 100));
			GUI.Label(new Rect(0, 0, 200, 100), kvp.Value.ToString(), guiSystem.ourSkin.label);
			GUILayout.EndArea();
		}
	}

	/*public override void OnMouseDown() {
		Debug.Log("Sdlfjskdfjsdkfjlsk");
		if (foodBoxToIngredients.Count != 2) {
			Debug.Log("Sdlfk");
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Debug.Log("Ray: " + ray + "; mp: " + Input.mousePosition); 
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			GameObject[] ingredients = foodBoxToIngredients.Values.ToArray();
			Func<int, int, int?> op;
			int? answer;
			if (operations.TryGetValue(hit.transform.name, out op) && (answer = op(ingredientsToNums[ingredients[0]], ingredientsToNums[ingredients[1]])).HasValue) {
				GameObject fusion = (GameObject)GameObject.Instantiate(genericFusionPrefab);
				genericFusions.Add(fusion);
				ingredientsToNums[fusion] = answer.Value;
				foreach (GameObject ingredient in foodBoxToIngredients.Values) {
					ingredient.transform.position += Vector3.up * 1000;
				}
				foodBoxToIngredients.Clear();
				fusion.GetComponent<DragAndDrop>().FoodBoxDrop += foodBox => {
					foodBoxToIngredients[foodBox] = fusion;
				};
			}
		}
	}*/
}

class NutritionState : GUIState {
	private GUISystem guiSystem;

	private bool showGUI;

	public override Vector3 CameraPosition { get; set; }

	public NutritionState(GUISystem system) {
		guiSystem = system;
		CameraPosition = GameObject.Find("NutritionCameraPosition").transform.position;
		showGUI = false;
	}

	public void ShowGUI() {
		showGUI = true;
	}

	private T Trace<T>(T x) {
		Debug.Log(x.ToString());
		return x;
	}

	public override void OnGUI() {
		if(!showGUI) {
			//Debug.Log("WHYYYYYY?");
			return;
		}
		GUILayout.BeginArea(Trace(new Rect(Camera.main.pixelWidth * .525f, Camera.main.pixelHeight * .4f, Camera.main.pixelWidth * .3f, Camera.main.pixelHeight * .1f)));
		if(GUILayout.Button("Main Menu", guiSystem.ourSkin.button, GUILayout.ExpandHeight(true))) {
			Camera.main.transform.position = GameObject.Find("MenuCameraPosition").transform.position;
			guiSystem.ChangeGUIState(new MenuState(guiSystem), guiSystem.ResetGame);
			GameObject.FindGameObjectWithTag("Cutting Board").GetComponent<CuttingBoard>().ingredients.Clear();
			GameObject.FindGameObjectWithTag("Cutting Board").GetComponent<ManageMath>().GenerateMath();
//			GameState gs = new GameState();
//			Ingredients.Clear();
//			foodBoxToIngredients.Clear();
//			ingredientsToNums.Clear();
//			genericFusions.Clear();
		}
		GUILayout.EndArea();
	}
}