using RootRacer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	[SerializeField] private Canvas MenuCanvas;
	[SerializeField] private Canvas creditsCanvas;
	private int activeScene = 0;
	[SerializeField] string nextScene;

	public void LoadNextScene()
	{
		SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void RestartGame()
	{
		GameManager.instance.ResetGame();
	}

	public void ShowCredits()
	{
		creditsCanvas.gameObject.SetActive(true);
		MenuCanvas.gameObject.SetActive(false);
	}

	public void GoBack()
	{
		creditsCanvas.gameObject.SetActive(false);
		MenuCanvas.gameObject.SetActive(true);
	}

	public void ShowGameOver(string winnerName)
	{
		MenuCanvas.gameObject.SetActive(true);
	}
}