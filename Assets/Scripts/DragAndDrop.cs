using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour {

	private Vector3 mousePos;
	private bool dragging;
	private bool snap;
	private GameObject grabbedObject;
	public float xOffset = 0.38f;
	public float yOffset = 0.35f;
	private GameObject[] previousIngredients;
	private int ingredientCount;
	private bool differentIngredient = true;
	public GameObject cuttingBoard;
	private float ingredientSlotXPos;
	private float ingredientSlotYPos;
	private Vector3 slotLocation;

	public string chosenOperator;

	// Use this for initialization
	void Start () {
	//	previousIngredients = new GameObject[5]();
	}
	
	// Update is called once per frame
	void Update () {
	

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
				if(hit.transform.gameObject.tag == "Cutting Board") {
					ingredientSlotXPos = hit.transform.position.x - hit.collider.bounds.extents.x + grabbedObject.collider.bounds.extents.x + xOffset;
					ingredientSlotYPos = hit.transform.position.y + hit.collider.bounds.extents.y - grabbedObject.collider.bounds.extents.y - yOffset;
					if(differentIngredient == true)
					{
//						ingredientSlotXPos = previousIngredients[ingredientCount].transform.position.x + grabbedObject.collider.bounds.extents.x;
//						ingredientSlotYPos = previousIngredients[ingredientCount].transform.position.y + grabbedObject.collider.bounds.extents.y;
						differentIngredient = false;
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
		differentIngredient = true;
		if(Vector3.Distance(grabbedObject.transform.position, slotLocation) == 0)
		{

			ingredientCount++;
			Debug.Log(ingredientCount);
		}
		else
		{
			ingredientCount--;
			Debug.Log(ingredientCount);
		}
	}
}
