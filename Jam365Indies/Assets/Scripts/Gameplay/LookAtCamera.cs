using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour
{
	public Camera targetCamera;

	void Update()
	{
		transform.localRotation = Quaternion.LookRotation(targetCamera.transform.position - transform.position);
	}
}
