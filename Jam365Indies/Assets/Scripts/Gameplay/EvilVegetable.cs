using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvilVegetable : MonoBehaviour
{
	public enum EnemyStates { Hidden, Eating, Walking, Idle };

	public float nearDistance = 2.5f;
	public float timeToTarget = 3f;
	public float eatingTime = 2f;

	private Dictionary<VegetableType, float> hideMap = new Dictionary<VegetableType, float>();
	private List<VegetableType> acceptableVegetableTypes = new List<VegetableType>();
	private List<GameObject> nearVegetables = new List<GameObject>();
	private GameObject targetVegetable;
	private float eatTimer = 0;
	private Animator myAnimator;
	private Vector3 startAttackPos;
	public bool isOnLight = false;
	private EnemyStates myState = EnemyStates.Idle;
	[FMODUnity.EventRef]
	public string EnemyHit;
	[FMODUnity.EventRef]
	public string EnemyEat;

	FMOD.Studio.EventInstance eatEvent;
	// Use this for initialization
	void Start()
	{
		eatEvent = FMODUnity.RuntimeManager.CreateInstance(EnemyEat);
		myAnimator = GetComponent<Animator>();
		LoadHideMap();
	}

	// Update is called once per frame
	void Update()
	{
		switch (myState)
		{
			case EnemyStates.Idle:
				Attack();
				break;

			case EnemyStates.Hidden:
				if (isOnLight)
				{
					return;
				}
				else
				{
					Show();
				}
				break;

			case EnemyStates.Walking:
				Walk();
				break;

			case EnemyStates.Eating:
				Eat();
				break;
		}

		if (isOnLight)
		{
			Hide();
		}

	}

	void OnDestroy()
	{
		eatEvent.release();
	}

	private void Attack()
	{
		CheckNearVegetables();

		if (nearVegetables.Count == 0)
			return;

		// Choose a target
		targetVegetable = nearVegetables[Random.Range(0, nearVegetables.Count)];

		// Start walking to the target
		myState = EnemyStates.Walking;
		myAnimator.SetBool("IsWalking", true);
	}

	private void Walk()
	{
		if (!targetVegetable)
		{
			myState = EnemyStates.Idle;
			myAnimator.SetBool("IsWalking", false);
		}

		transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetVegetable.transform.localPosition, timeToTarget * Time.deltaTime);
		if (Vector3.Distance(transform.localPosition, targetVegetable.transform.localPosition) <= 0.25)
		{
			//transform.localPosition = targetVegetable.transform.localPosition;
			transform.localPosition.Set(targetVegetable.transform.localPosition.x, targetVegetable.transform.localPosition.y, targetVegetable.transform.localPosition.z - 0.1f);
			myState = EnemyStates.Eating;
			eatTimer = eatingTime;
			myAnimator.SetBool("IsWalking", false);
			myAnimator.SetBool("IsEating", true);
		}

	}

	private void Eat()
	{
		if (eatTimer > 0)
		{
			FMOD.Studio.PLAYBACK_STATE state;
			eatEvent.getPlaybackState(out state);
			if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
			{
				eatEvent.start();
			}
			eatTimer -= Time.deltaTime;
			return;
		}
		else
		{
			eatEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			if (targetVegetable) Destroy(targetVegetable);
			myState = EnemyStates.Idle;
			myAnimator.SetBool("IsEating", false);
		}
	}

	private void CheckNearVegetables()
	{
		nearVegetables.Clear();
		foreach (GameObject nearVeg in GameObject.FindGameObjectsWithTag("Vegetable"))
		{
			if (Vector3.Distance(transform.position, nearVeg.transform.position) <= nearDistance)
			{
				nearVegetables.Add(nearVeg);
			}
		}
	}

	private void Hide()
	{
		myState = EnemyStates.Hidden;

		ResetAcceptableVegetableTypes();
		CheckNearVegetables();

		// Ignore the vegetable types around
		foreach (GameObject veg in nearVegetables)
		{
			VegetableType nearVegType = veg.GetComponent<Vegetable>().vegetableType;
			if (acceptableVegetableTypes.Contains(nearVegType))
				acceptableVegetableTypes.Remove(nearVegType);
		}

		// Change the animation
		myAnimator.SetBool("IsHiding", true);

		myAnimator.SetFloat("HideType", hideMap[acceptableVegetableTypes[Random.Range(0, acceptableVegetableTypes.Count)]]);

	}

	private void Show()
	{
		myState = EnemyStates.Idle;

		// Change the animation
		myAnimator.SetBool("IsHiding", false);

	}

	private void Die()
	{
		FMODUnity.RuntimeManager.PlayOneShot(EnemyHit, transform.position);
		Destroy(gameObject);
	}

	private void ResetAcceptableVegetableTypes()
	{
		acceptableVegetableTypes.Clear();
		acceptableVegetableTypes.Add(VegetableType.Abobora);
		acceptableVegetableTypes.Add(VegetableType.Alface);
		acceptableVegetableTypes.Add(VegetableType.Beterraba);
		acceptableVegetableTypes.Add(VegetableType.Cenoura);
		acceptableVegetableTypes.Add(VegetableType.Tomate);
	}


	// TODO : Refactor this little monster!!!
	private void LoadHideMap()
	{
		hideMap.Clear();
		hideMap.Add(VegetableType.Abobora, 0f);
		hideMap.Add(VegetableType.Alface, 0.25f);
		hideMap.Add(VegetableType.Beterraba, 0.5f);
		hideMap.Add(VegetableType.Cenoura, 0.75f);
		hideMap.Add(VegetableType.Tomate, 1f);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Light")
		{
			isOnLight = true;
		}
		if (col.tag == "Slash")
		{
			Die();
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Light")
		{
			isOnLight = false;
		}
	}

}
