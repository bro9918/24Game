using UnityEngine;
using System.Collections;

public class SelectIngredients : MonoBehaviour {

	public int maxIngredients = 2;
	private GameObject cuttingBoard;
	private DragAndDrop dragAndDrop;

	// Use this for initialization
	void Start () {
		cuttingBoard = GameObject.FindGameObjectWithTag("Cutting Board");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp () {

	}
}
