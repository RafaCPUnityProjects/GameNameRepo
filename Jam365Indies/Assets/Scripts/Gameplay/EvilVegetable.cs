using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvilVegetable : MonoBehaviour {


	public float thinkingInterval = 1.0f;
	public float distanceToHide = 5.0f;
	public float nearDistance = 2.5f;
	public VegetableType vegetableType;


	private HashSet<VegetableType> nearVegetableTypes;
	private List<GameObject> nearVegetables = new List<GameObject> ();
	private Vector3 targetPosition;
	private float timer = 0;
	private GameObject scarecrow;

	// Use this for initialization
	void Start () {
		scarecrow = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(transform.position, scarecrow.transform.position) <= distanceToHide){
			Hide();
			return;
		}

		if (timer > 0) {
			timer -= Time.deltaTime;
			return;
		}

		CheckNearVegetables ();
		Eat ();
	}

	private void CheckNearVegetables () {
		nearVegetables.Clear ();
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Vegetable")) {
			if (Vector3.Distance (transform.position, go.transform.position) <= nearDistance) {
				nearVegetables.Add(go);
			}
		}
	}

	private void Eat() {
		if (nearVegetables.Count == 0)
			return;

		// Choose a target
		GameObject targetVegetable = nearVegetables [Random.Range (0, nearVegetables.Count)];
		targetPosition = targetVegetable.transform.position;

		// Eats the target
		// TODO : Call a "Kill" method on target
		Destroy (targetVegetable);

		// TODO : Make a smooth transition here
		transform.position = targetPosition;

		timer = thinkingInterval;
	}
		
	private void Hide () {
		
	}

}
