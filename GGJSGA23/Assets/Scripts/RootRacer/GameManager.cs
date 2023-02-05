using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RootRacer
{
	public class GameManager : MonoBehaviour
	{
		public delegate void GamePauseDelegate();

		// Events
		public event GamePauseDelegate OnGamePause;
		public event GamePauseDelegate OnGameUnPause;

		// Static fields
		public static GameManager Instance;
		public static Camera MainCamera;
		
		// Static Getters
		public static IReadOnlyList<PlayerController> Players => Instance.players;
		public static float Depth => Instance.depth;
		public static GameObject ShieldPrefab => Instance.shieldPrefab;

		// Public fields
		public DepthMusicSO gameDepthMusic;
		public MeshRenderer worldMeshRenderer;
		public bool isPaused;

		// Private fields (Shown in editor)
		[SerializeField] private float startSpeed = 0.05f;
		[SerializeField] private float speedIncrease = 0.1f;
		[SerializeField] private GameObject shieldPrefab;

		// Private fields (hidden in editor)
		private Material worldMaterial;
		private float depth;
		private float currentSpeed = 0.5f;
		private int shaderPropID;
		private List<PlayerController> players;
		private MenuManager menuManager;
		private int currentlyPlayingDepthMusic;

		private void Awake()
		{
			MainCamera = FindObjectOfType<Camera>();
			players = FindObjectsOfType<PlayerController>().ToList();
			menuManager = FindObjectOfType<MenuManager>();
			
			if (menuManager == null)
			{
				Debug.LogError("No menuManager in scene");
			}
			
			Instance = this;
			isPaused = true;
			Time.timeScale = 0;
			
			worldMaterial = worldMeshRenderer.material;
		}
		
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
			CheckDepthMusic(depth);
			CollisionSystemUtil.UpdateCollisions();
		}

		private void CheckDepthMusic(float depth)
		{
			//DepthMusic deepestSelectedMusic = gameDepthMusic[currentlyPlayingDepthMusic];
			int selectedIndex = currentlyPlayingDepthMusic;

			for (int i = currentlyPlayingDepthMusic; i < gameDepthMusic.gameDepthMusic.Length; i++)
			{
				DepthMusic depthMusic = gameDepthMusic.gameDepthMusic[i];
				if (depth > depthMusic.depth)
				{
					//deepestSelectedMusic = depthMusic;
					selectedIndex = i;
				}
			}

			if (selectedIndex != currentlyPlayingDepthMusic)
			{
				gameDepthMusic.gameDepthMusic[currentlyPlayingDepthMusic].music.Stop2D();
				gameDepthMusic.gameDepthMusic[selectedIndex].music.Play2D();
				currentlyPlayingDepthMusic = selectedIndex;
			}
		}

		public float GetTargetSpeed()
		{
			return currentSpeed;
		}

		private void ScrollWorld(float deltaTime)
		{
			depth -= deltaTime * currentSpeed;
			currentSpeed += speedIncrease * deltaTime;
			worldMaterial.SetVector(shaderPropID, new Vector2(0, depth));
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
			OnGameUnPause?.Invoke();
			gameDepthMusic.gameDepthMusic[currentlyPlayingDepthMusic].music.Play2D();

			menuManager.ShowGameOver("",false);

		}

		void PauseGame()
		{
			OnGamePause?.Invoke();
			gameDepthMusic.gameDepthMusic[currentlyPlayingDepthMusic].music.Stop2D();
			isPaused = true;
			Time.timeScale = 0;
		}

		public void ResetGame()
		{
			currentSpeed = startSpeed;
			depth = 0;
			var players = FindObjectsOfType<PlayerController>();
			foreach (var player in players)
			{
				player.ResetPlayer();
			}
		}

		public static void RemovePlayer(PlayerController playerController)
		{
			Instance.players.Remove(playerController);
			CollisionSystemUtil.UnregisterPlayer(playerController);
			if (Instance.players.Count == 1)
			{
				Instance.GameOver(Instance.players[0]);
			}
		}

		public void GameOver(PlayerController playerWin)
		{
			PauseGame();
			menuManager.ShowGameOver(playerWin.gameObject.name,true);
		}
	}
}