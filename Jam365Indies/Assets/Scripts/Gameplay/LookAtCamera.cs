using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour
{
	public Camera targetCamera;

	void Update()
	{
		transform.rotation = Quaternion.LookRotation(targetCamera.transform.position - transform.position);
	}
}
