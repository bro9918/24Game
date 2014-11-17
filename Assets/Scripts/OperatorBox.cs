using UnityEngine;
using System.Collections;
using System;

public class OperatorBox : MonoBehaviour {

	public event Action<GameObject> OnMouseDownEvent;

	public string chosenOperator;

	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	void OnMouseDown() {
		OnMouseDownEvent(gameObject);
	}
}
