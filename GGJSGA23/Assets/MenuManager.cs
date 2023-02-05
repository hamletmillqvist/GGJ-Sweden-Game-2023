using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RootRacer;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Canvas MenuCanvas;
    [SerializeField]
    private Canvas creditsCanvas;
    private int activeScene = 0;
    [SerializeField]
    string nextScene;
    public Image logo;
    public Sprite[] logos;
    public int players = 2;
    [SerializeField] string twoPlayerScene;
    [SerializeField] string threePlayerScene;
    [SerializeField] string fourPlayerScene;
    //bool changingScene = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
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
    public void RestartGame()
    {
        GameManager.instance.StartGame();
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
    public void ShowGameOver(string winnerName, bool show)
    {
        if (show)
        {
            // logic
        }
        MenuCanvas.gameObject.SetActive(show);
    }
    public void SelectPlayers(int players)
    {
        logo.sprite = logos[players - 2];
        this.players = players;
    }
}
