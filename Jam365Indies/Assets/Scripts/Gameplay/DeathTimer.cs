using UnityEngine;
using System.Collections;

public class DeathTimer : MonoBehaviour
{
	public float deathTimer;

	void Start()
	{
		Destroy(this.gameObject, deathTimer);
	}
}
