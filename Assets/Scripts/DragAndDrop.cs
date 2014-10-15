using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour {

	private Vector3 mousePos;
	private bool dragging;
	private bool snap;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

	}

	void OnMouseDrag () {
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if(this.tag == "Operator")
		{
			this.transform.position = new Vector3(mousePos.x, mousePos.y, -1.0f);
			gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			dragging = true;
		}
		else
			dragging = false;

		if(dragging == true)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if(Physics.Raycast (ray, out hit))
			{
				if(hit.transform.gameObject.tag == "OperatorBox")
					this.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, -1.0f);
			}
		}
	}

	void OnMouseUp () {

	}
}
