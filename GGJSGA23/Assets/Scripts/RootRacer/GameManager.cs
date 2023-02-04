using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace RootRacer
{
    public class GameManager : MonoBehaviour
    {
        public TextMeshProUGUI depthTM;
        private Material worldMaterial;
        public MeshRenderer worldMeshRenderer;
        private float ypos = 0;
        [SerializeField] private float startSpeed = 0.05f;
        private float currentSpeed = 0.5f;
        [SerializeField]private float speedIncrease = 0.1f;
        private int shaderPropID;
        public static Camera MainCamera;
        public bool IsPaused = false;

        void Start()
        {
            MainCamera = FindObjectOfType<Camera>();
            worldMaterial = worldMeshRenderer.material;
            shaderPropID = worldMaterial.shader.GetPropertyNameId(worldMaterial.shader.FindPropertyIndex("_Position"));
            StartGame();
        }
        public float GetTargetSpeed()
        {
            return currentSpeed;
        }
        void Update()
        {
            ScrollWorld(Time.deltaTime);
            UpdateDepthText(-ypos);
        }

        private void UpdateDepthText(float ypos)
        {
            depthTM.text = ypos.ToString("0.#m");
        }

        private void ScrollWorld(float deltaTime)
        {
            if (IsPaused) return;
            
            ypos -= deltaTime * currentSpeed;
            currentSpeed += speedIncrease * deltaTime;
            worldMaterial.SetVector(shaderPropID, new Vector2(0, ypos));
        }

        public void GameOver(PlayerController playerFail)
        {
            IsPaused = true;
            Time.timeScale = 0;
        }
        public void StartGame()
        {
            ResetGame();
            IsPaused = false;
        }
        private void ResetGame()
        {
            Time.timeScale = 1;
            currentSpeed = startSpeed;
            ypos = 0;
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            for (int i = 0; i < players.Length; i++)
            {
                players[i].ResetPlayer();
            }
        }
    } 
}
