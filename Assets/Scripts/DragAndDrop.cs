using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour {

	private Vector3 mousePos;
	private bool dragging;
	private bool snap;
	public bool onBoard = false;
	private GameObject grabbedObject;
	public float xOffset = 0.38f;
	public float yOffset = 0.35f;
	private static GameObject previousIngredient;

	private Vector3 slotLocation;
	private bool wasOnCuttingBoard;
	private bool locationSet;
	private CuttingBoard cuttingBoardScript;
	private float secondRowYOffset = 0.9f;
	private GameObject showNameObject;
	public TextMesh showName;

	public string chosenOperator;

	// Use this for initialization
	void Start () {
		cuttingBoardScript = GameObject.FindGameObjectWithTag("Cutting Board").GetComponent<CuttingBoard>();

		showNameObject = GameObject.FindGameObjectWithTag("Show Name");
		showName = showNameObject.GetComponent<TextMesh>();
		showName.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	

	}

	void OnMouseDown() {
		if(this.tag == "Ingredient")
		{
			grabbedObject = gameObject;
			if(onBoard)
			{
				cuttingBoardScript.ingredientCount--;
				cuttingBoardScript.ingredients.Remove(grabbedObject);
			}

		}
	}

	void OnMouseDrag () {
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if(this.tag == "Operator" || this.tag == "Ingredient")
		{
			grabbedObject = gameObject;
			grabbedObject.transform.position = new Vector3(mousePos.x, mousePos.y, -1.0f);
			grabbedObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			dragging = true;
		}


		if(dragging == true)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if(Physics.Raycast (ray, out hit))
			{
				if(hit.transform.gameObject.tag == "Cutting Board" && cuttingBoardScript.ingredientCount < cuttingBoardScript.maxIngredients) {
					cuttingBoardScript.currentSlotPos.x = hit.transform.position.x - hit.collider.bounds.extents.x + grabbedObject.collider.bounds.extents.x + xOffset;
					cuttingBoardScript.currentSlotPos.y = hit.transform.position.y + hit.collider.bounds.extents.y - grabbedObject.collider.bounds.extents.y - yOffset;

					for(int i = 0; i < cuttingBoardScript.ingredients.Count; i++)
					{
						if(cuttingBoardScript.ingredients[i] == null)
							break;
						if(i < 3)
							cuttingBoardScript.currentSlotPos.x += (cuttingBoardScript.ingredients[i] as GameObject).collider.bounds.size.x;
						if(i == 3)
						{
							cuttingBoardScript.currentSlotPos.x = hit.transform.position.x - hit.collider.bounds.extents.x + grabbedObject.collider.bounds.extents.x + xOffset;
							cuttingBoardScript.currentSlotPos.y -= secondRowYOffset;
						}
						if(i > 3)
							cuttingBoardScript.currentSlotPos.x += (cuttingBoardScript.ingredients[i] as GameObject).collider.bounds.size.x;
					}
					slotLocation = new Vector3(cuttingBoardScript.currentSlotPos.x, cuttingBoardScript.currentSlotPos.y, -1.0f);
					grabbedObject.transform.position = slotLocation;
				}
			}
		}
	}

	void OnMouseUp () {
		dragging = false;
		grabbedObject.layer = LayerMask.NameToLayer("Default");
		if(this.tag == "Ingredient")
		{
			if(Vector3.Distance(grabbedObject.transform.position, slotLocation) == 0)
			{
				cuttingBoardScript.ingredients.Add(gameObject);
				cuttingBoardScript.ingredientCount++;
				onBoard = true;
			}
			else
			{
				onBoard = false;
			}
			previousIngredient = gameObject;
			locationSet = false;
		}
	}

	void OnMouseOver () {
		if(showName.renderer.enabled != true)
			showName.renderer.enabled = true;
		showName.text = gameObject.name;
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		showName.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
	}

	void OnMouseExit (){
		showName.renderer.enabled = false;
	}
}
