using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RootRacer;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Canvas MenuCanvas;
    [SerializeField]
    private Canvas creditsCanvas;
    private int activeScene = 0;
    [SerializeField]
    string nextScene;
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
    public void ShowGameOver(string winnerName)
    {
        MenuCanvas.gameObject.SetActive(true);
    }
}
