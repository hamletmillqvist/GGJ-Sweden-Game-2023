using Sonity;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace RootRacer
{
	public class GameManager : MonoBehaviour
	{		
		public static GameManager instance;
		public DepthMusic[] gameDepthMusic;
		[SerializeField] private float startSpeed = 0.05f;
		[SerializeField] private float speedIncrease = 0.1f;
		public static Camera MainCamera;

		//public TextMeshProUGUI depthTM;
		public MeshRenderer worldMeshRenderer;
		public bool isPaused;

		private Material worldMaterial;
		private float yPosition;
		private float currentSpeed = 0.5f;
		private int shaderPropID;
		private List<PlayerController> players;
		public delegate void OnGamePause();
		public event OnGamePause onGamePause;
		public event OnGamePause onGameUnPause;
		private MenuManager menuManager;
		private int currentlyPlayingDepthMusic = 0;

		private void Awake()
		{
			instance = this;
			MainCamera = FindObjectOfType<Camera>();
			worldMaterial = worldMeshRenderer.material;
			players = FindObjectsOfType<PlayerController>().ToList();
			isPaused = true;
			Time.timeScale = 0;
			menuManager = FindObjectOfType<MenuManager>();
            if (menuManager == null)
            {
				Debug.LogError("No menuManager in scene");
            }
		}
		public static List<PlayerController> Players => instance.players;
		public static float Depth => instance.yPosition;

		void Start()
		{			
			shaderPropID = worldMaterial.shader.GetPropertyNameId(worldMaterial.shader.FindPropertyIndex("_Position"));
			
			StartGame();
		}

		void Update()
		{
            if (isPaused)
            {
                return;
            }
            ScrollWorld(Time.deltaTime);
			CheckDepthMusic(yPosition);
			CollisionSystemUtil.UpdateCollisions();
		}

        private void CheckDepthMusic(float depth)
        {
			//DepthMusic deepestSelectedMusic = gameDepthMusic[currentlyPlayingDepthMusic];
			int selectedIndex = currentlyPlayingDepthMusic;

			for (int i = currentlyPlayingDepthMusic; i < gameDepthMusic.Length; i++)
            {
				DepthMusic depthMusic = gameDepthMusic[i];
                if (depth > depthMusic.depth)
                {
					//deepestSelectedMusic = depthMusic;
					selectedIndex = i;
				}
            }
            if (selectedIndex != currentlyPlayingDepthMusic)
            {
				gameDepthMusic[currentlyPlayingDepthMusic].music.Stop2D();
				gameDepthMusic[selectedIndex].music.Play2D();
				currentlyPlayingDepthMusic = selectedIndex;
			}
        }

        public float GetTargetSpeed()
		{
			return currentSpeed;
		}

		

		private void ScrollWorld(float deltaTime)
		{
			

			yPosition -= deltaTime * currentSpeed;
			currentSpeed += speedIncrease * deltaTime;
			worldMaterial.SetVector(shaderPropID, new Vector2(0, yPosition));
		}
		[ContextMenu("Start")]
		public void StartGame()
		{
			ResetGame();
			UnPauseGame();
		}
		void UnPauseGame()
		{
            Time.timeScale = 1;
            isPaused = false;
            onGameUnPause?.Invoke();
			gameDepthMusic[currentlyPlayingDepthMusic].music.Play2D();
        }
		void PauseGame()
		{
            onGamePause?.Invoke();
			gameDepthMusic[currentlyPlayingDepthMusic].music.Stop2D();
			isPaused = true;
            Time.timeScale = 0;
			
        }

		public void ResetGame()
		{
			
			currentSpeed = startSpeed;
			yPosition = 0;
			var players = FindObjectsOfType<PlayerController>();
			foreach (var player in players)
			{
				player.ResetPlayer();
			}
		}
		public static void RemovePlayer(PlayerController playerController)
		{
			instance.players.Remove(playerController);
			if (instance.players.Count == 1)
			{
				instance.GameOver(instance.players[0]);
			}
		}
		public void GameOver(PlayerController playerWin)
		{
			PauseGame();
			menuManager.ShowGameOver(playerWin.gameObject.name);
		}
	}
	[System.Serializable]
	public class DepthMusic
    {
		public SoundEvent music;
		public float depth;
    }
}