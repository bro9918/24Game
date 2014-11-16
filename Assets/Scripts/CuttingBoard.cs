using UnityEngine;
using System.Collections;

public class CuttingBoard : MonoBehaviour {

	public ArrayList ingredients = new ArrayList();
	public int maxIngredients = 2;
	public TextMesh maxIngredientsDisplay;
	public int ingredientCount = 0;
	public Vector2 currentSlotPos;

	// Use this for initialization
	void Start () {
		if(ingredients == null)
			ingredients = new ArrayList();
		
		maxIngredientsDisplay.text = maxIngredients.ToString();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		maxIngredientsDisplay.text = (maxIngredients - ingredientCount).ToString();
	}

}
