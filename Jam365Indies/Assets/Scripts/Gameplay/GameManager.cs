using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public float gameTime;
	public float minPercentToWin;
	public string nextLevel;

	private float remainingTime;
	private float remainingVegetablesPercent;
	private float totalVegetables;
	private float remainingVegetables;
	private bool gameEnded = false;

	public Text txtTime;
	public Text txtMinPercent;
	public Text txtPercent;
	public Image menu;
	public Text txtVictory;
	public Text txtLose;
	public Button btnNext;
	public Button btnRestart;

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

		if (remainingVegetablesPercent > 0 && remainingVegetablesPercent < minPercentToWin) {
			GameOver ();
		}

		if (remainingTime <= 0 && remainingVegetablesPercent >= minPercentToWin) {
			WinLevel ();
		}

		UpdateUI ();
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
		menu.gameObject.SetActive (true);
		txtVictory.gameObject.SetActive (true);
		btnNext.gameObject.SetActive (true);
		musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		FMODUnity.RuntimeManager.PlayOneShot(WinEvent, transform.position);
	}

	void GameOver () {
		gameEnded = true;
		menu.gameObject.SetActive (true);
		txtLose.gameObject.SetActive (true);
		btnRestart.gameObject.SetActive (true);
		musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		FMODUnity.RuntimeManager.PlayOneShot(LoseEvent, transform.position);
	}

	void UpdateUI () {
		txtTime.text = "Remaining Time: " + FormatTime ();
		txtMinPercent.text = "Min: " + Mathf.Floor (minPercentToWin * 100) + " %";
		txtPercent.text = "Vegetables: " + Mathf.Floor (remainingVegetablesPercent * 100) + " %";
	}

	public void NextLevel () {
		SceneManager.LoadScene(nextLevel);
	}

	public void RestartLevel () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void Quit () {
		SceneManager.LoadScene("mainMenu");
	}
}
