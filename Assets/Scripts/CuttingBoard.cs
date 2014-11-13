using UnityEngine;
using System.Collections;

public class CuttingBoard : MonoBehaviour {

	public GameObject[] ingredients = new GameObject[8];
	public int maxIngredients = 2;
	public TextMesh maxIngredientsDisplay;

	// Use this for initialization
	void Start () {
		if(ingredients == null)
			ingredients = new GameObject[8];
		
		maxIngredientsDisplay.text = maxIngredients.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
