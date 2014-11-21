using UnityEngine;
using System.Collections;

public class BasicMath : MonoBehaviour {
	//Three initialized numbers
	public int number1;
	public int number2;
	public int number3;

	//Three text displayed numbers
	public TextMesh number1Object;
	public TextMesh number2Object;
	public TextMesh number3Object;

	//Answer text displayed
	public TextMesh answerObject;

	//Operator boxes
	public OperatorBox operatorBoxObject1;
	public OperatorBox operatorBoxObject2;

	//Number generated from first two numbers
	public int firstAns;

	//Given attempts and answer
	public int attempt;
	public int answer;

	public bool gottaDoMath;

	//Number of Operator Boxes
	public int numOfBoxes;

	void Start() {

		numOfBoxes = 2;

		//Set numbers to random values
		number1 = Random.Range(2, 14);
		number2 = Random.Range(2, 14);
		number3 = Random.Range(2, 14);

		//Set the Objects text to that number
		number1Object.text = number1.ToString();
		number2Object.text = number2.ToString();
		number3Object.text = number3.ToString();

		//Randomize answer
		if(numOfBoxes == 1){
			answer = randomAnswerGenOneBox(number1, number2);
		}
		else if(numOfBoxes == 2){
			answer = randomAnswerGenTwoBoxes(number1, number2, number3);
		}
		answerObject.text = answer.ToString();

		//All numbers on text objects, can now do math
		gottaDoMath = true;
	
	}

	public int DoMathOneBox(int a, int b, string op) {
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

	public int DoMathTwoBoxes(int a, int b, int c, string op1, string op2) {

		switch (op1) {
		case "+":
			firstAns = a + b;
			gottaDoMath = true;
			break;
		case "-":
			firstAns = a - b;
			gottaDoMath = true;
			break;
		case "*":
			firstAns = a * b;
			gottaDoMath = true;
			break;
		case "/":
			firstAns = a / b;
			gottaDoMath = true;
			break;
		default:
			return -1000000000;
			break;
		}

		switch (op2) {
		case "+":
			return firstAns + c;
			gottaDoMath = false;
			break;
		case "-":
			return firstAns - c;
			gottaDoMath = false;
			break;
		case "*":
			return firstAns * c;
			gottaDoMath = false;
			break;
		case "/":
			return firstAns / c;
			gottaDoMath = false;
			break;
		default:
			return -1000000000;
			break;
		}
	}

	public int randomAnswerGenOneBox(int a, int b) {

		int randSign = Random.Range(0, 3);

		//0 = +, 1 = -, 2 = *, 3 = /
		switch (randSign) {
			case 0:
				return a + b;
				break;
			case 1:
				return a - b;
				break;
			case 2:
				return a * b;
				break;
			case 3:
				return a / b;
			default:
				return -1000000000;
				break;
		}
	}

	public int randomAnswerGenTwoBoxes(int a, int b, int c) {

		int randSign = Random.Range(0, 3);
		
		//0 = +, 1 = -, 2 = *, 3 = /
		switch (randSign) {
		case 0:
			firstAns = a + b;
			break;
		case 1:
			firstAns = a - b;
			break;
		case 2:
			firstAns = a * b;
			break;
		case 3:
			firstAns = a / b;
			break;
		default:
			return -1000000000;
			break;
		}

		int randSign2 = Random.Range(0, 3);

		//0 = +, 1 = -, 2 = *, 3 = /
		switch (randSign2) {
		case 0:
			return firstAns + c;
			break;
		case 1:
			return firstAns - c;
			break;
		case 2:
			return firstAns * c;
			break;
		case 3:
			return firstAns / c;
			break;
		default:
			return -1000000000;
			break;
		}
	}

	void Update() {
		if (gottaDoMath) {

			if(numOfBoxes == 1){
				attempt = DoMathOneBox(number1, number2, operatorBoxObject1.chosenOperator);
			}
			else if(numOfBoxes == 2){
				attempt = DoMathTwoBoxes(number1, number2, number3, operatorBoxObject1.chosenOperator, operatorBoxObject2.chosenOperator);
			}

		}
	}

}