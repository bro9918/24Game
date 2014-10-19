using UnityEngine;
using System.Collections;

public class BasicMath : MonoBehaviour {
	public int number1;
	public int number2;
	public int targetNumber;

	public TextMesh number1Object;
	public TextMesh number2Object;
	public OperatorBox operatorBoxObject;
	public int answer;
	public bool gottaDoMath;

	void Start() {
		number1Object.text = number1.ToString();
		number2Object.text = number2.ToString();
		gottaDoMath = true;
	}

	public int DoMath(int a, int b, string op) {
		switch (op) {
		case "+":
			return a + b;
			gottaDoMath = false;
			break;
		case "-":
			return a - b;
			gottaDoMath = false;
			break;
		case "*":
			return a * b;
			gottaDoMath = false;
			break;
		case "/":
			return a / b;
			gottaDoMath = false;
			break;
		default:
			return -1000000000;
			break;
		}
	}

	void Update() {
		if (gottaDoMath) {
			answer = DoMath(number1, number2, operatorBoxObject.chosenOperator);
		}
	}

}