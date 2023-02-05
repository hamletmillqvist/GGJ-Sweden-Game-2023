using System;
using RootRacer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	[SerializeField] private Canvas MenuCanvas;
	[SerializeField] private Canvas creditsCanvas;
	[SerializeField] private Canvas placingsCanvas;
	private int activeScene = 0;
	[SerializeField] private string nextScene;
	public Image logo;
	public Sprite[] logos;
	public int players = 2;
	public Image[] placingsImages;
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
		placingsCanvas.gameObject.SetActive(false);
		MenuCanvas.gameObject.SetActive(false);
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

	public void ShowGameOver(PlayerController playerController)
	{
		//SceneManager.LoadScene(gameOverScene, LoadSceneMode.Single);
		SetPlace(0, playerController);
        for (int i = 0; i <= GameManager.Instance.playerDeaths.Count; i++)
        {
			SetPlace(i+1);
		}
		placingsCanvas.gameObject.SetActive(true);
	}
	private void SetPlace(int i, PlayerController player)
    {
		placingsImages[i].sprite = player.winFaces[i];
		Color c = placingsImages[i].color;
		c.a = 1f;
		placingsImages[i].color = c;
	}
	private void SetPlace(int i)
	{
		placingsImages[i].sprite = GameManager.Instance.playerDeaths.Pop().Player.winFaces[i];
		Color c = placingsImages[i].color;
		c.a = 1f;
		placingsImages[i].color = c;
	}

	public void SelectPlayers(int players)
	{
		logo.sprite = logos[players - 2];
		this.players = players;
	}
}