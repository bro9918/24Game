﻿using UnityEngine;
using System.Collections;

public class NutritionalInformation : MonoBehaviour {

	private float fat;
	private float sodium;
	private float protein;
	private float sugar;

	private int vitaminA;
	private int vitaminC;
	private int calcium;
	private int iron;

	// Use this for initialization
	void Start () {
		if(gameObject.name == "Grapes")
		{
			fat = 0.0f;
			sodium = 3.0f;
			protein = 1.0f;
			sugar = 23.0f;

			vitaminA = 2;
			vitaminC = 27;
			calcium = 2;
			iron = 3;
		}
		if(gameObject.name == "Radish")
		{
			fat = 0.0f;
			sodium = 45.0f;
			protein = 1.0f;
			sugar = 2.0f;
			
			vitaminA = 0;
			vitaminC = 29;
			calcium = 3;
			iron = 2;
		}
		if(gameObject.name == "Yellow Pepper")
		{
			fat = 0.0f;
			sodium = 4.0f;
			protein = 2.0f;
			sugar = 0.0f;
			
			vitaminA = 7;
			vitaminC = 569;
			calcium = 2;
			iron = 5;
		}
		if(gameObject.name == "Mushroom")
		{
			fat = 0.0f;
			sodium = 4.0f;
			protein = 2.0f;
			sugar = 1.0f;
			
			vitaminA = 0;
			vitaminC = 2;
			calcium = 0;
			iron = 2;
		}
		if(gameObject.name == "Tomato")
		{
			fat = 0.0f;
			sodium = 7.0f;
			protein = 1.0f;
			sugar = 4.0f;
			
			vitaminA = 25;
			vitaminC = 32;
			calcium = 1;
			iron = 2;
		}
		if(gameObject.name == "Apple")
		{
			fat = 0.0f;
			sodium = 1.0f;
			protein = 0.0f;
			sugar = 13.0f;
			
			vitaminA = 1;
			vitaminC = 10;
			calcium = 1;
			iron = 1;
		}
		if(gameObject.name == "Chicken")
		{
			fat = 9.0f;
			sodium = 67.0f;
			protein = 20.0f;
			sugar = 0.0f;
			
			vitaminA = 0;
			vitaminC = 0;
			calcium = 1;
			iron = 5;
		}
		if(gameObject.name == "Steak")
		{
			fat = 6.0f;
			sodium = 118.0f;
			protein = 49.0f;
			sugar = 0.0f;
			
			vitaminA = 0;
			vitaminC = 0;
			calcium = 2;
			iron = 22;
		}
		if(gameObject.name == "Cookie")
		{
			fat = 5.0f;
			sodium = 55.0f;
			protein = 1.0f;
			sugar = 10.0f;
			
			vitaminA = 2;
			vitaminC = 0;
			calcium = 1;
			iron = 2;
		}
		if(gameObject.name == "Carrot")
		{
			fat = 0.0f;
			sodium = 88.0f;
			protein = 1.0f;
			sugar = 6.0f;
			
			vitaminA = 428;
			vitaminC = 13;
			calcium = 4;
			iron = 2;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
