using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public new Camera camera;
    public LayerMask hitMask;
    public int playerTurn = 1;
    void Start()
    {
        if (camera == null)
        {
            camera = FindObjectOfType<Camera>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out RaycastHit hit, float.MaxValue,hitMask))
            {
                BoardPart boardPart = hit.transform.gameObject.GetComponent<BoardPart>();
                if (boardPart == null) return;
                if (boardPart.GetPiece() != 0) return;
                boardPart.SetPiece(playerTurn);
                playerTurn *= -1;
            }
        }
    }
}
