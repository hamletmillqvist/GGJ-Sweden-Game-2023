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

	public Image logo;
	public Sprite[] logos;
	public int players = 2;
	public Image[] placingsImages;

	[SerializeField] private string nextScene;
	[SerializeField] private string twoPlayerScene;
	[SerializeField] private string threePlayerScene;
	[SerializeField] private string fourPlayerScene;

	private void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public void LoadNextScene()
	{
		SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
	}

	public void StartGame()
	{
        placingsCanvas.gameObject.SetActive(false);
        MenuCanvas.gameObject.SetActive(false);
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
		SceneManager.LoadScene(nextScene);
		SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        placingsCanvas.gameObject.SetActive(false);
        MenuCanvas.gameObject.SetActive(false);
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
		SceneManager.LoadScene("MainMenu");
		placingsCanvas.gameObject.SetActive(false);
		Destroy(gameObject);
	}

	public void ShowGameOver(PlayerController playerController)
	{
		SetPlace(0, playerController);
		Debug.Log(GameManager.Instance.playerDeaths.Count);
		int i = 1;
        while (GameManager.Instance.playerDeaths.Count>0)
        {
            if (i > 3)
            {
				break;
            }
			SetPlace(i);
			i++;
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