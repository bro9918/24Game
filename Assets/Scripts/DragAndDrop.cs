using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour {

	private Vector3 mousePos;
	private bool dragging;
	private bool snap;
	private GameObject grabbedObject;
	public float xOffset = 0.38f;
	public float yOffset = 0.35f;
	private static GameObject previousIngredient;
	private static int ingredientCount = 0;
	private float ingredientSlotXPos;
	private float ingredientSlotYPos;
	private Vector3 slotLocation;
	private bool wasOnCuttingBoard;
	private bool locationSet;
	private CuttingBoard cuttingBoardScript;
	private float secondRowYOffset = 0.9f;
	public int maxIngredients = 2;

	public string chosenOperator;

	// Use this for initialization
	void Start () {
		cuttingBoardScript = GameObject.FindGameObjectWithTag("Cutting Board").GetComponent<CuttingBoard>();
	}
	
	// Update is called once per frame
	void Update () {
	

	}

	void OnMouseDown() {
		if(this.tag == "Ingredient")
		{
			grabbedObject = gameObject;
			if(Vector3.Distance(grabbedObject.transform.position, slotLocation) != 0)
			{
				wasOnCuttingBoard = false;
			}
			else
			{
				wasOnCuttingBoard = true;
				ingredientCount--;
				for(int i = 0; i < cuttingBoardScript.ingredients.Length; i++)
				{
					if(cuttingBoardScript.ingredients[i].name == gameObject.name)
					{
						cuttingBoardScript.ingredients[i] = null;
						break;
					}
				}
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
				if(hit.transform.gameObject.tag == "OperatorBox") {
					grabbedObject.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, -1.0f);
					hit.transform.gameObject.GetComponent<OperatorBox>().chosenOperator = this.name;
				}
				if(hit.transform.gameObject.tag == "Cutting Board" && ingredientCount < maxIngredients) {
					ingredientSlotXPos = hit.transform.position.x - hit.collider.bounds.extents.x + grabbedObject.collider.bounds.extents.x + xOffset;
					ingredientSlotYPos = hit.transform.position.y + hit.collider.bounds.extents.y - grabbedObject.collider.bounds.extents.y - yOffset;
					for(int i = 0; i < cuttingBoardScript.ingredients.Length; i++)
					{
						if(cuttingBoardScript.ingredients[i] == null)
							break;
						if(cuttingBoardScript.ingredients[i] != null && i < 3)
							ingredientSlotXPos += cuttingBoardScript.ingredients[i].collider.bounds.size.x;
						if(cuttingBoardScript.ingredients[i] != null && i == 3)
						{
							ingredientSlotXPos = hit.transform.position.x - hit.collider.bounds.extents.x + grabbedObject.collider.bounds.extents.x + xOffset;
							ingredientSlotYPos -= secondRowYOffset;
						}
						if(cuttingBoardScript.ingredients[i] != null && i > 3)
							ingredientSlotXPos += cuttingBoardScript.ingredients[i].collider.bounds.size.x;
					}
					slotLocation = new Vector3(ingredientSlotXPos, ingredientSlotYPos, -1.0f);
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
				for(int i = 0; i < cuttingBoardScript.ingredients.Length; i++)
				{
					if(cuttingBoardScript.ingredients[i] == null && ingredientCount < maxIngredients)
					{
						cuttingBoardScript.ingredients[i] = gameObject;
						break;
					}
				}
				ingredientCount++;
			}
			previousIngredient = gameObject;
			locationSet = false;
		}
	}
}
