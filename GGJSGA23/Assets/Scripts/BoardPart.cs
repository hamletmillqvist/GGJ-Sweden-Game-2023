using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPart : MonoBehaviour
{
    private int piece = 0;
    public Color p1Color;
    public Color p2Color;
    public int GetPiece()
    {
        return piece;
    }
    public void SetPiece(int value)
    {
        if (piece != 0)
            return;
        piece = value;
        if (value == -1)
        {
            GetComponent<MeshRenderer>().material.color = p1Color;
            
        }
        if (value == 1)
        {
            GetComponent<MeshRenderer>().material.color = p2Color;
        }
    }
    public void Reset()
    {
        piece = 0;
        GetComponent<MeshRenderer>().material.color = Color.grey;

    }
}
