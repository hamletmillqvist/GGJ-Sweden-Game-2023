using System;
using RootRacer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	[SerializeField] private Canvas MenuCanvas;
	[SerializeField] private Canvas creditsCanvas;
	private int activeScene = 0;
	[SerializeField] private string nextScene;
	public Image logo;
	public Sprite[] logos;
	public int players = 2;
	[SerializeField] private string twoPlayerScene;
	[SerializeField] private string threePlayerScene;
	[SerializeField] private string fourPlayerScene;

	[SerializeField] private string gameOverScene;
	//bool changingScene = false;

	// Start is called before the first frame update
	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	// Update is called once per frame
	void Update()
	{ }

	public void LoadNextScene()
	{
		//changingScene = true;
		//activeScene++;
		//SceneManager.LoadScene(activeScene, LoadSceneMode.Single);

		SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
	}

	public void StartGame()
	{
		string scene;
		switch (players)
		{
			default:
			case 2:
				scene = twoPlayerScene;
				break;
			case 3:
				scene = threePlayerScene;
				break;
			case 4:
				scene = fourPlayerScene;
				break;
		}

		SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
		SceneManager.LoadScene(scene, LoadSceneMode.Additive);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	[Obsolete]
	public void RestartGame()
	{
		GameManager.Instance.StartGame();
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

	public void ShowGameOver()
	{
		SceneManager.LoadScene(gameOverScene, LoadSceneMode.Single);
	}

	public void SelectPlayers(int players)
	{
		logo.sprite = logos[players - 2];
		this.players = players;
	}
}