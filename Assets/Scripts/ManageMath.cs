using UnityEngine;
using System.Collections;

public class ManageMath : MonoBehaviour {

	public int NumbersInvolved = 4;
	public int TargetMaxNumber = 24;
	public int TargetMinNumber = 240;
	public int TargetNumber;

	private int targetNumber;

	public ArrayList numberList;

	public static ManageMath instance;
	
	void Awake() {

		if (instance == null) {
			instance = this;
		}
		numberList = new ArrayList ();
		GenerateMath ();
	}

	public void GenerateMath() {
	
		TargetNumber = Random.Range(TargetMinNumber, TargetMaxNumber + 1);
		targetNumber = TargetNumber;
		numberList.Clear ();
		numberList.Add(targetNumber);

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
	}

	public void PickAndSplit(ArrayList numbers) {
		// randomly pick a number and split it
		int indexToPick = Random.Range(0, numbers.Count);
		int numberToSplit = (int)numbers[indexToPick];
		if (numberToSplit == 1 && numberToSplit == 0) {
			return;
		}
		numbers.RemoveAt(indexToPick);
		AddNumbers(SplitWithRandomOperator(numberToSplit));
	}

	public void AddNumbers(int[] toAdd) {
		if (toAdd.Length != 2) {
			return;
		}
		numberList.Add(toAdd[0]);
		numberList.Add(toAdd[1]);
	}

	public int[] SplitWithRandomOperator(int toSplit) {
		ArrayList okOperations = new ArrayList();
		if (toSplit > 1) {
			okOperations.Add('a');
			okOperations.Add('m');
		}
		if (toSplit < TargetMaxNumber) {
			okOperations.Add('s');
			if (toSplit < TargetMaxNumber / 2) { // don't div-split big numbers
				okOperations.Add('d');
			}
		}

		char op = (char)okOperations[Random.Range(0, okOperations.Count)];

		switch (op) {
			case 'a':
				return SplitAdd(toSplit);
			case 's':
				return SplitSub(toSplit);
			case 'm':
				return SplitMult(toSplit);
			case 'd':
				return SplitDiv(toSplit);
		}
		return null;
	}

	public int[] SplitMult(int prod) {
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
		nums[0] = (int)factors[Random.Range(1, factors.Count)];
		nums[1] = prod / nums[0];
		if (nums[0] > TargetMaxNumber * 2) {
			throw new UnityException("" + nums[0].ToString());
		}
		if (nums[1] > TargetMaxNumber * 2) {
			throw new UnityException("" + nums[1].ToString());
		}


		return nums;
	}

	public int[] SplitDiv(int quot) {
		int[] nums = new int[2];
		nums[0] = Random.Range(1, (TargetMaxNumber / 2 + 1) - quot + 1);
		nums[1] = quot * nums[0];
		if (nums[0] > TargetMaxNumber * 2 || nums[1] > TargetMaxNumber * 2) {
			throw new UnityException("" + quot + "\n" + nums[0].ToString() + "\n" + nums[1].ToString());
		}
		return nums;
	}

	public int[] SplitAdd(int sum) {
		int[] nums = new int[2];
		nums[0] = Random.Range(1, sum);
		nums[1] = sum - nums[0];
		if (nums[0] < 0) {
			throw new UnityException("" + nums[0].ToString());
		}
		if (nums[1] < 0) {
			throw new UnityException(sum.ToString() + " - " + nums[1].ToString());
		}
		return nums;
	}

	public int[] SplitSub(int diff) {
		int[] nums = new int[2];
		nums[0] = Random.Range(diff + 1, (int)(diff * 1.5f));
		nums[1] = nums[0] - diff;
		if (nums[0] > TargetMaxNumber * 2) {
			throw new UnityException("" + nums[0].ToString());
		}
		if (nums[1] > TargetMaxNumber * 2) {
			throw new UnityException(nums[0].ToString() + " - " + diff.ToString());
		}
		return nums;
	}

	// Update is called once per frame
	void Update() {
	
	}
}
