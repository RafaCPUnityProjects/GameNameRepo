using UnityEngine;
using System.Collections;

public class Vegetable : MonoBehaviour {
	
	public VegetableType vegetableType;

	void Start () {
		AlignOnGarden ();
	}

	private void AlignOnGarden () {
		transform.position.Set (Mathf.Round(transform.position.x), 0, Mathf.Round(transform.position.z));
	}
}
