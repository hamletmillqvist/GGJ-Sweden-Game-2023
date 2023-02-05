using RootRacer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	[SerializeField] private Canvas menuCanvas;
	[SerializeField] private Canvas creditsCanvas;
	[SerializeField] string nextScene;
	private int activeScene = 0;

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
		GameManager.Instance.ResetGame();
	}

	public void ShowCredits()
	{
		creditsCanvas.gameObject.SetActive(true);
		menuCanvas.gameObject.SetActive(false);
	}

	public void GoBack()
	{
		creditsCanvas.gameObject.SetActive(false);
		menuCanvas.gameObject.SetActive(true);
	}

	public void ShowGameOver(string winnerName)
	{
		menuCanvas.gameObject.SetActive(true);
	}
}