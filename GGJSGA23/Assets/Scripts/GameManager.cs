using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    BoardPart[] boardParts = new BoardPart[9];
    public GameObject part;

    private void InitBoard()
    {
        for (int i = 0; i < 9; i++)
        {
            GameObject go = Instantiate(part);
            go.transform.position = new Vector3(0,0,0);

            boardParts[i] = go.GetComponent<BoardPart>();
        }
    }
    private void Start()
    {
        boardParts = FindObjectsOfType<BoardPart>();
    }
    public void ResetGame()
    {
        foreach (var part in boardParts)
        {
            part.Reset();
        }
    }
}
