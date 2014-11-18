using UnityEngine;
using System.Collections;

public class CuttingBoard : MonoBehaviour {

	public ArrayList ingredients = new ArrayList();

	//lol
	public int maxIngredients {
		get {
			return GameObject.FindGameObjectWithTag("Cutting Board").GetComponent<ManageMath>().NumbersInvolved;
		}
		set {
			GameObject.FindGameObjectWithTag("Cutting Board").GetComponent<ManageMath>().NumbersInvolved = value;
		}
	}

	//public int maxIngredients = 2;
	public TextMesh maxIngredientsDisplay;
	public int ingredientCount = 0;
	public Vector2 currentSlotPos;

	private float caloriesTotal;
	private float fatTotal;
	private float sodiumTotal;
	private float proteinTotal;
	private float sugarTotal;
	
	private int vitaminATotal;
	private int vitaminCTotal;
	private int calciumTotal;
	private int ironTotal;

	public TextMesh caloriesText;
	public TextMesh fatText;
	public TextMesh sodiumText;
	public TextMesh proteinText;
	public TextMesh sugarText;
	
	public TextMesh vitaminAText;
	public TextMesh vitaminCText;
	public TextMesh calciumText;
	public TextMesh ironText;

	private bool calculateFactTotals;

	// Use this for initialization
	void Start() {
		if (ingredients == null) {
			ingredients = new ArrayList();
		}
		
		maxIngredientsDisplay.text = maxIngredients.ToString();
	}

	void Update() {
		if (ingredients.Count == maxIngredients && calculateFactTotals == false)
		{
			GameObject[] ingredientsArray = new GameObject[maxIngredients];
			for (int i = 0; i < maxIngredients; i++) {
				ingredientsArray[i] = (GameObject)ingredients[i];
			}
					
			for (int i = 0; i < ingredients.Count; i++) {
				caloriesTotal += ingredientsArray[i].GetComponent<NutritionalInformation>().calories;
				fatTotal += ingredientsArray[i].GetComponent<NutritionalInformation>().fat;
				sodiumTotal += ingredientsArray[i].GetComponent<NutritionalInformation>().sodium;
				proteinTotal += ingredientsArray[i].GetComponent<NutritionalInformation>().protein;
				sugarTotal += ingredientsArray[i].GetComponent<NutritionalInformation>().sugar;
				vitaminATotal += ingredientsArray[i].GetComponent<NutritionalInformation>().vitaminA;
				vitaminCTotal += ingredientsArray[i].GetComponent<NutritionalInformation>().vitaminC;
				calciumTotal += ingredientsArray[i].GetComponent<NutritionalInformation>().calcium;
				ironTotal += ingredientsArray[i].GetComponent<NutritionalInformation>().iron;
			}
			caloriesText.text = caloriesTotal.ToString();
				fatText.text = fatTotal.ToString();
				sodiumText.text = sodiumTotal.ToString();
				proteinText.text = proteinTotal.ToString();
				sugarText.text = sugarTotal.ToString();
				vitaminAText.text = vitaminATotal.ToString();
				vitaminCText.text = vitaminCTotal.ToString();
				calciumText.text = calciumTotal.ToString();
				ironText.text = ironTotal.ToString();
			calculateFactTotals = true;
		}
		if(ingredients.Count == 0)
			calculateFactTotals = false;
	}
	
	// Update is called once per frame
	void LateUpdate() {
		maxIngredientsDisplay.text = (maxIngredients - ingredientCount).ToString();
	}

}
