using UnityEngine;
using System.Collections;

public class TargetNumberText : MonoBehaviour {

	public TextMesh mesh;

	void Start () {
				
	}
	
	// Update is called once per frame
	void Update () {
		mesh.text = "Make " + GameObject.Find("Cutting Board").GetComponent<ManageMath>().TargetNumber.ToString() + "!";
	}
}
