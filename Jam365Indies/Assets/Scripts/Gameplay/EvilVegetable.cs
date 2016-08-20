using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvilVegetable : MonoBehaviour
{

	public float thinkingInterval = 1.0f;
	public float distanceToHide = 5.0f;
	public float nearDistance = 2.5f;
	public float timeToTarget = 5f;
	public float eatingTime = 5f;
	public Sprite[] sprites;


	private Sprite mySprite;
	private Dictionary<VegetableType, Sprite> mapSprites = new Dictionary<VegetableType, Sprite> ();
	private List<VegetableType> acceptableVegetableTypes = new List<VegetableType> ();
	private List<GameObject> nearVegetables = new List<GameObject> ();
	private Vector3 targetPosition;
	private float timer = 0;
	private GameObject scarecrow;
	private SpriteRenderer myRenderer;
	private Animator myAnimator;
	private Vector3 startAttackPos;
	private bool isEating;
	private bool isHidden = false;

	// Use this for initialization
	void Start ()
	{
		scarecrow = GameObject.FindGameObjectWithTag ("Player");
		myRenderer = gameObject.GetComponent<SpriteRenderer> ();
		myAnimator = GetComponent<Animator> ();
		mySprite = myRenderer.sprite;
		LoadSpriteMap ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (isHidden) {
			if (Vector3.Distance (transform.position, scarecrow.transform.position) <= distanceToHide) {
				return;
			} else {
				Show ();
			}
		} else {
			if (Vector3.Distance (transform.position, scarecrow.transform.position) <= distanceToHide) {
				Hide ();
				return;
			}
		}
			
		if (timer > 0) {
			timer -= Time.deltaTime;
			return;
		}

		CheckNearVegetables ();
		//Attack ();
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

	private void Attack ()
	{
		if (nearVegetables.Count == 0)
			return;

		// Choose a target
		GameObject targetVegetable = nearVegetables [Random.Range (0, nearVegetables.Count)];
		targetPosition = targetVegetable.transform.position;

		if (!myAnimator.GetBool ("IsWalking") && !myAnimator.GetBool ("IsEating")) {
			StartCoroutine (GoToTarget ());
			Destroy (targetVegetable);
		}
		timer = thinkingInterval;
	}

	IEnumerator GoToTarget ()
	{
		float elapsedTime = 0;
		startAttackPos = transform.position;
		myAnimator.SetBool ("IsWalking", true);
		while (elapsedTime < timeToTarget) {
			elapsedTime += Time.deltaTime;
			transform.position = Vector3.Lerp (startAttackPos, targetPosition, elapsedTime / timeToTarget);
			yield return null;
		}
		myAnimator.SetBool ("IsWalking", false);
		StartCoroutine (StartEating ());
	}

	IEnumerator StartEating ()
	{
		myAnimator.SetBool ("IsEating", true);
		yield return new WaitForSeconds (eatingTime);
		myAnimator.SetBool ("IsEating", false);
	}

	private void Hide ()
	{
		isHidden = true;
		ResetAcceptableVegetableTypes ();

		// Ignore the vegetable types around
		foreach (GameObject veg in nearVegetables) {
			VegetableType nearVegType = veg.GetComponent<Vegetable> ().vegetableType;
			if (acceptableVegetableTypes.Contains(nearVegType))
				acceptableVegetableTypes.Remove (nearVegType);
		}

		// Change my sprite to hide
		myRenderer.sprite = mapSprites[acceptableVegetableTypes [Random.Range (0, acceptableVegetableTypes.Count)]];

		// Change the animation
		myAnimator.SetBool ("IsHiding", isHidden);
	}


	private void Show ()
	{
		isHidden = false;

		// Change my sprite back to original
		myRenderer.sprite = mySprite;

		// Change the animation
		myAnimator.SetBool ("IsHiding", isHidden);
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

}
