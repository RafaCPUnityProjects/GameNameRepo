using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public int sceneToLoadID;


	public void ChangeScene()
	{
		SceneManager.LoadScene(sceneToLoadID);
	}

	public void Quit () {
		Application.Quit ();
	}

	public GameObject credits;
	public void ShowCredits () {
		credits.SetActive (true);
	}

	public void HideCredits () {
		credits.SetActive (false);
	}
}
