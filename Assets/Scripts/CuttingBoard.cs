using UnityEngine;
using System.Collections;

public class CuttingBoard : MonoBehaviour {

	public GameObject[] ingredients = new GameObject[8];

	// Use this for initialization
	void Start () {
		if(ingredients == null)
			ingredients = new GameObject[8];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp () {
	
	}
}
