using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Canvas mainMenuCanvas;
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
    public void StartGame()
    {
        //changingScene = true;
        //activeScene++;
        
        //SceneManager.LoadScene(activeScene, LoadSceneMode.Single);
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
    public void Exitgame()
    {
        Application.Quit();
    }
    public void ShowCredits()
    {
        creditsCanvas.gameObject.SetActive(true);
        mainMenuCanvas.gameObject.SetActive(false);
    }
    public void GoBack()
    {
        creditsCanvas.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);
    }
}
