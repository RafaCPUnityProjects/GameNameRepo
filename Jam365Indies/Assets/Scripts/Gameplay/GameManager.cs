using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public float gameTime;
	public float remainingTime;
	public float remainingVegetablesPercent;
	public int minPercentToWin;

	private float totalVegetables;
	private float remainingVegetables;
	private bool gameEnded = false;

	// Use this for initialization
	void Start () {
		Invoke ("CountTotalVegetables", 1);
		InvokeRepeating("CountRemainingVegetables", 2, 2);
	}
	
	// Update is called once per frame
	void Update () {
		if (gameEnded)
			return;

		remainingTime = gameTime - Time.time;
					
		if (remainingVegetablesPercent < minPercentToWin) {
			GameOver ();
		}

		if (remainingTime <= 0 && remainingVegetablesPercent >= minPercentToWin) {
			WinLevel ();
		}
	}

	private void CountRemainingVegetables () {
		remainingVegetables = GameObject.FindGameObjectsWithTag ("Vegetable").Length;
		remainingVegetablesPercent = remainingVegetables / totalVegetables;
	}

	private void CountTotalVegetables () {
		totalVegetables = GameObject.FindGameObjectsWithTag ("Vegetable").Length;
	}

	private string FormatTime () {
		float minutes = Mathf.Floor(remainingTime / 60);
		float seconds = Mathf.Floor (remainingTime % 60);
		return minutes.ToString() + ":" + (seconds > 9 ? seconds.ToString() : "0"+seconds.ToString());
	}

	void WinLevel(){
		gameEnded = true;
		Debug.Log ("You Win!");
	}

	void GameOver () {
		gameEnded = true;
		Debug.Log ("You Lose!");
	}
}
