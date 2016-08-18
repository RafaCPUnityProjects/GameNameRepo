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
}
