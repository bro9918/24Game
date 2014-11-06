using UnityEngine;
using System.Collections;

public class ManageMath : MonoBehaviour {

	public int NumbersInvolved = 4;
	public int OperandMaxNumber = 12; // must be > 2
	public int TargetMaxNumber = 24;
	public int TargetMinNumber = 24;

	private int targetNumber;
	private ArrayList numberList;
	
	void Start () {
		targetNumber = Random.Range(TargetMinNumber, TargetMaxNumber);
		numberList = new ArrayList();
		numberList.Add (targetNumber);

		// (recursively and randomly) pick a number to split from the available numbers in numberList
		// until numberList.Count == NumbersInvolved
		while (numberList.Count != NumbersInvolved) {
			PickAndSplit(numberList);
		}

		string msg = "";
		msg += targetNumber.ToString() + ": ";
		foreach (var num in numberList) {
			msg += num.ToString() + " ";	
		}
		Debug.Log(msg);
	}

	public void PickAndSplit(ArrayList numbers) {
		// randomly pick a number and split it
		int indexToPick = Random.Range(0, numbers.Count-1);
		int numberToSplit = (int)numbers[indexToPick];
		numbers.RemoveAt(indexToPick);
		AddNumbers(SplitWithRandomOperator(numberToSplit));
	}

	public void AddNumbers (int[] toAdd) {
		if (toAdd.Length != 2) {
			return;
		}
		numberList.Add(toAdd[0]);
		numberList.Add(toAdd[1]);
	}

	public int[] SplitWithRandomOperator (int toSplit) {
		int op = Random.Range(1,6);
		switch (op) {
			case 1:
			case 2:
				return SplitAdd(toSplit);
				break;
			case 3:
			case 4:
				return SplitSub(toSplit);
				break;
			case 5:
				return SplitMult(toSplit);
				break;
			case 6:
				return SplitDiv(toSplit);
				break;
		}
		return null;
	}

	public int[] SplitMult (int prod) {
		// find factors
		ArrayList factors = new ArrayList();
		factors.Add(prod);
		factors.Add(1);
		for (int i = 2; i < prod; i++) {
			if (prod % i == 0 && !factors.Contains(i) && !factors.Contains(prod / i)) {
				factors.Add(i);
			}
		}
		int[] nums = new int[2];
		nums[0] = (int)factors[Random.Range(1, factors.Count-1)];
		nums[1] = prod / nums[0];

		return nums;
	}

	public int[] SplitDiv (int quot) {
		int[] nums = new int[2];
		nums[0] = Random.Range(1, (targetNumber + 1) - quot);
		nums[1] = quot * nums[0];

		return nums;
	}

	public int[] SplitAdd (int sum) {
		int[] nums = new int[2];
		nums[0] = Random.Range (1, sum);
		nums[1] = sum - nums[0];

		return nums;
	}

	public int[] SplitSub (int diff) {
		int[] nums = new int[2];
		nums[0] = Random.Range (diff, OperandMaxNumber);
		nums[1] = nums[0] - diff;
		return nums;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
