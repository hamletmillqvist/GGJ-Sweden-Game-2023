using System;
using RootRacer;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public static MenuManager Instance;
	[SerializeField] private Canvas MenuCanvas;
	[SerializeField] private Canvas creditsCanvas;
	[SerializeField] private Canvas placingsCanvas;
	//private int activeScene = 0;
	[SerializeField] private string gameScene;
	[SerializeField] private string titleScene;
	public Image logo;
	public Sprite[] logos;
	public int players = 2;
	public Image[] placingsImages;
	[SerializeField] private string twoPlayerScene;
	[SerializeField] private string threePlayerScene;
	[SerializeField] private string fourPlayerScene;

	//[SerializeField] private string gameOverScene;

	//bool changingScene = false;


	void Start()
	{
		if (Instance == null)
		{
            Instance = this;
        }
        if (Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		
		DontDestroyOnLoad(this.gameObject);
	}

	void Update()
	{ }


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
		SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
		SceneManager.LoadScene(scene, LoadSceneMode.Additive);
	}
	public void BackToTitle()
	{
        SceneManager.LoadScene(titleScene, LoadSceneMode.Single);
        ShowRootMenu();
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
		HidePanels();
		creditsCanvas.gameObject.SetActive(true);
	}

	public void ShowRootMenu()
	{
		HidePanels();
		MenuCanvas.gameObject.SetActive(true);
	}
	private void HidePanels()
	{
        placingsCanvas.gameObject.SetActive(false);
        creditsCanvas.gameObject.SetActive(false);
        MenuCanvas.gameObject.SetActive(false);
    }

	public void ShowGameOver(PlayerController playerController)
	{
		HidePanels();
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