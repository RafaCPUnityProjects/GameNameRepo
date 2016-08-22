﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public float gameTime;
	public float remainingTime;
	public float remainingVegetablesPercent;
	public float minPercentToWin;

	private float totalVegetables;
	private float remainingVegetables;
	private bool gameEnded = false;

	public Text txtTime;
	public Text txtMinPercent;
	public Text txtPercent;

	[FMODUnity.EventRef]
	public string MusicEvent;
	FMOD.Studio.EventInstance musicInstance;

	[FMODUnity.EventRef]
	public string WinEvent;
	[FMODUnity.EventRef]
	public string LoseEvent;
	// Use this for initialization
	void Start () {
		Invoke ("CountTotalVegetables", 0.4f);
		InvokeRepeating("CountRemainingVegetables", 0.5f, 2);
		musicInstance = FMODUnity.RuntimeManager.CreateInstance(MusicEvent);
		musicInstance.start();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameEnded)
			return;

		remainingTime = gameTime - Time.time;

		UpdateUI ();

		if (remainingVegetablesPercent > 0 && remainingVegetablesPercent < minPercentToWin) {
			GameOver ();
		}

		if (remainingTime <= 0 && remainingVegetablesPercent >= minPercentToWin) {
			WinLevel ();
		}

	}

	void OnDestroy()
	{
		musicInstance.release();
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
		musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		FMODUnity.RuntimeManager.PlayOneShot(WinEvent, transform.position);
		Debug.Log ("You Win!");
	}

	void GameOver () {
		gameEnded = true;
		musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		FMODUnity.RuntimeManager.PlayOneShot(LoseEvent, transform.position);
		Debug.Log ("You Lose!");
	}

	void UpdateUI () {
		txtTime.text = "Remaining Time: " + FormatTime ();
		txtMinPercent.text = "Min: " + Mathf.Floor (minPercentToWin * 100) + " %";
		txtPercent.text = "Vegetables: " + Mathf.Floor (remainingVegetablesPercent * 100) + " %";
	}
}
