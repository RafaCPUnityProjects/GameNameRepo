using UnityEngine;
using System.Collections;

public class VegetableFieldSpawner : MonoBehaviour
{
	public GameObject[] vegetablesPrefabs;
	public Transform sizeMarker;
	public int vegetablePerRoll;
	public int vegetablePerCollum;

	private Vector3 fieldDimensions;
	private Vector2 fieldStepValues;
	private Vector3 startPos;
	private GameObject[,] plantedVegetables;

	void Start()
	{
		Initialize();
		PlantVegetables();
	}

	private void PlantVegetables()
	{
		GameObject randomVegetable = GetRandomVegetable ();
		Vector3 pos;
		for (int y = 0; y < vegetablePerCollum; y++)
		{
			for (int x = 0; x < vegetablePerRoll; x++)
			{
				pos = new Vector3(fieldStepValues.x * x, fieldStepValues.y * y);
				plantedVegetables[x, y] = (GameObject)Instantiate(randomVegetable, startPos + pos, Quaternion.identity);
			}
		}
	}

	private void Initialize()
	{
		startPos = transform.position;

		fieldDimensions = sizeMarker.position - startPos;

		float xStep = fieldDimensions.x / vegetablePerRoll;
		float yStep = fieldDimensions.y / vegetablePerCollum;
		fieldStepValues = new Vector2(xStep, yStep);

		plantedVegetables = new GameObject[vegetablePerRoll, vegetablePerCollum];
	}

	GameObject GetRandomVegetable()
	{
		return vegetablesPrefabs[Random.Range(0, vegetablesPrefabs.Length)];
	}

	void OnDrawGizmos()
	{
		startPos = transform.position;

		fieldDimensions = sizeMarker.position - startPos;
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(startPos + (fieldDimensions * 0.5f), fieldDimensions);
	}
}
