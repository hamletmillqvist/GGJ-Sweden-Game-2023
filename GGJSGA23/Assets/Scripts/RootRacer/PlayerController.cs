using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public Color playerColor;
    public float horizontalMoveSpeed;
    private new Camera camera;
    private Vector2 screenSize;
    void Start()
    {
        camera = FindObjectOfType<Camera>();
        GetComponentInChildren<SpriteRenderer>().material.color = playerColor;
        
    }

    void Update()
    {
        float xMove = 0;
        if (Input.GetKey(moveLeft))
        {
            xMove -= 1;
        }
        if (Input.GetKey(moveRight))
        {
            xMove += 1;
        }
        transform.position += new Vector3( horizontalMoveSpeed * xMove * Time.deltaTime,0,0);
        Vector3 screenPoint = camera.WorldToScreenPoint(transform.position);
        
        screenPoint.x = Mathf.Clamp(screenPoint.x,0, Screen.width);
        transform.position = camera.ScreenToWorldPoint(screenPoint);

    }
}
