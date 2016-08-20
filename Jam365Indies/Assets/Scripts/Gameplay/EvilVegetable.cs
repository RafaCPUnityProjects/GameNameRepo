using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvilVegetable : MonoBehaviour {


	public float thinkingInterval = 1.0f;
	public float distanceToHide = 5.0f;
	public float nearDistance = 2.5f;
	public Dictionary<VegetableType, Sprite> mapSprites = new Dictionary<VegetableType, Sprite> ();
	public Sprite[] sprites;

	private HashSet<VegetableType> acceptableVegetableTypes = new HashSet<VegetableType>();
	private List<GameObject> nearVegetables = new List<GameObject> ();
	private Vector3 targetPosition;
	private float timer = 0;
	private GameObject scarecrow;
	private SpriteRenderer myRenderer;


	// Use this for initialization
	void Start () {
		scarecrow = GameObject.FindGameObjectWithTag ("Player");
		myRenderer = gameObject.GetComponent<SpriteRenderer> ();
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
		ResetAcceptableVegetableTypes ();

		// Ignore the vegetable types around
		foreach (GameObject veg in nearVegetables) {
			acceptableVegetableTypes.Remove (veg.GetComponent<Vegetable> ().vegetableType);
		}


	}

	private void ResetAcceptableVegetableTypes () {
		acceptableVegetableTypes.Clear ();
		acceptableVegetableTypes.Add (VegetableType.Abobora);
		acceptableVegetableTypes.Add (VegetableType.Alface);
		acceptableVegetableTypes.Add (VegetableType.Beterraba);
		acceptableVegetableTypes.Add (VegetableType.Cenoura);
		acceptableVegetableTypes.Add (VegetableType.Tomate);
	}

	// TODO : Refactor this little monster!!!
	private void LoadSpriteMap () {
		mapSprites.Clear();
		mapSprites.Add(VegetableType.Abobora, sprites[0]);
		mapSprites.Add(VegetableType.Alface, sprites[1]);
		mapSprites.Add(VegetableType.Beterraba, sprites[2]);
		mapSprites.Add(VegetableType.Cenoura, sprites[3]);
		mapSprites.Add(VegetableType.Tomate, sprites[4]);
	}

}
