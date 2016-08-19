using UnityEngine;
using System.Collections;

public class Vegetable : MonoBehaviour {
	
	public VegetableType vegetableType;


	// Use this for initialization
	void Start () {
		AlignOnGarden ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	private void AlignOnGarden () {
		transform.position.Set (Mathf.Round(transform.position.x), 0, Mathf.Round(transform.position.z));
	}
}
