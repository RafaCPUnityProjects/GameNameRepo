using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvilVegetable : MonoBehaviour
{
	public enum EnemyStates { Hidden, Eating, Walking, Idle };

	public float nearDistance = 2.5f;
	public float timeToTarget = 3f;
	public float eatingTime = 2f;
	public Sprite[] sprites;


	private Sprite mySprite;
	private Dictionary<VegetableType, Sprite> mapSprites = new Dictionary<VegetableType, Sprite> ();
	private List<VegetableType> acceptableVegetableTypes = new List<VegetableType> ();
	private List<GameObject> nearVegetables = new List<GameObject> ();
	private GameObject targetVegetable;
	private float eatTimer = 0;
	private SpriteRenderer myRenderer;
	private Animator myAnimator;
	private Vector3 startAttackPos;
	public bool isOnLight = false;
	private EnemyStates myState = EnemyStates.Idle;

	// Use this for initialization
	void Start ()
	{
		myRenderer = gameObject.GetComponent<SpriteRenderer> ();
		myAnimator = GetComponent<Animator> ();
		mySprite = myRenderer.sprite;
		LoadSpriteMap ();
	}

	// Update is called once per frame
	void Update ()
	{
		switch (myState) {
		case EnemyStates.Idle:
			Attack ();
			break;
	
		case EnemyStates.Hidden:
			if (isOnLight) {
				return;
			} else {
				Show ();
			}
			break;

		case EnemyStates.Walking:
			Walk ();	
			break;

		case EnemyStates.Eating:
			Eat ();
			break;
		}
			
		if (isOnLight) {
			Hide ();
		}

	}

	private void Attack ()
	{
		CheckNearVegetables ();

		if (nearVegetables.Count == 0)
			return;

		// Choose a target
		targetVegetable = nearVegetables [Random.Range (0, nearVegetables.Count)];

		// Start walking to the target
		myState = EnemyStates.Walking;
	}

	private void Walk () {
		myAnimator.SetBool ("IsWalking", true);

		transform.localPosition = Vector3.MoveTowards (transform.localPosition, targetVegetable.transform.localPosition, timeToTarget * Time.deltaTime);
		if (Vector3.Distance (transform.localPosition,  targetVegetable.transform.localPosition) <= 0.25) {
			transform.localPosition = targetVegetable.transform.localPosition;
			myState = EnemyStates.Eating;
			eatTimer = eatingTime;
			myAnimator.SetBool ("IsWalking", false);
			myAnimator.SetBool ("IsEating", true);
		}

	}

	private void Eat () {
		if (eatTimer > 0) {
			eatTimer -= Time.deltaTime;
			return;
		} else {
			Destroy (targetVegetable);
			myState = EnemyStates.Idle;
			myAnimator.SetBool ("IsEating", false);
		}
	}
		
	private void CheckNearVegetables ()
	{
		nearVegetables.Clear ();
		foreach (GameObject nearVeg in GameObject.FindGameObjectsWithTag("Vegetable")) {
			if (Vector3.Distance (transform.position, nearVeg.transform.position) <= nearDistance) {
				nearVegetables.Add (nearVeg);
			}
		}
	}

	private void Hide ()
	{
		myState = EnemyStates.Hidden;

		// Change the animation
		// myAnimator.SetBool ("IsHiding", true);

		ResetAcceptableVegetableTypes ();
		CheckNearVegetables ();

		// Ignore the vegetable types around
		foreach (GameObject veg in nearVegetables) {
			VegetableType nearVegType = veg.GetComponent<Vegetable> ().vegetableType;
			if (acceptableVegetableTypes.Contains(nearVegType))
				acceptableVegetableTypes.Remove(nearVegType);
		}

		// Change my sprite to hide
		myRenderer.sprite = mapSprites[acceptableVegetableTypes [Random.Range (0, acceptableVegetableTypes.Count)]];

	}


	private void Show ()
	{
		myState = EnemyStates.Idle;

		// Change the animation
		//myAnimator.SetBool ("IsHiding", false);

		// Change my sprite back to original
		myRenderer.sprite = mySprite;

	}

	

	private void ResetAcceptableVegetableTypes ()
	{
		acceptableVegetableTypes.Clear ();
		acceptableVegetableTypes.Add (VegetableType.Abobora);
		acceptableVegetableTypes.Add (VegetableType.Alface);
		acceptableVegetableTypes.Add (VegetableType.Beterraba);
		acceptableVegetableTypes.Add (VegetableType.Cenoura);
		acceptableVegetableTypes.Add (VegetableType.Tomate);
	}
	

	// TODO : Refactor this little monster!!!
	private void LoadSpriteMap ()
	{
		mapSprites.Clear ();
		mapSprites.Add (VegetableType.Abobora, sprites [0]);
		mapSprites.Add (VegetableType.Alface, sprites [1]);
		mapSprites.Add (VegetableType.Beterraba, sprites [2]);
		mapSprites.Add (VegetableType.Cenoura, sprites [3]);
		mapSprites.Add (VegetableType.Tomate, sprites [4]);
	}


	void OnTriggerEnter (Collider col) {
		if (col.tag == "Light") {
			isOnLight = true;
		}
	}

	void OnTriggerExit (Collider col) {
		if (col.tag == "Light") {
			isOnLight = false;
		}
	}

}
