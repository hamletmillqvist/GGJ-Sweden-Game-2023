using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RootRacer
{
	public class GameManager : MonoBehaviour
	{
		public delegate void OnGamePausingDelegate();

		public event OnGamePausingDelegate OnGamePause;
		public event OnGamePausingDelegate OnGameUnPause;
		
		public static GameManager Instance { get; set; }
		public static Camera MainCamera { get; private set; }
		public MeshRenderer WorldMeshRenderer { get; }
		public DepthMusicSO GameDepthMusic { get; }
		public bool IsPaused { get; set; }

		[SerializeField]
		private float startSpeed = 0.05f;
		[SerializeField]
		private float speedIncrease = 0.1f;

		private Material worldMaterial;
		private float yPosition;
		private float currentSpeed = 0.5f;
		private int shaderPropID;
		private List<PlayerController> players;
		private MenuManager menuManager;
		private int currentlyPlayingDepthMusic = 0;

		private void Awake()
		{
			Instance = this;
			MainCamera = FindObjectOfType<Camera>();
			worldMaterial = WorldMeshRenderer.material;
			players = FindObjectsOfType<PlayerController>().ToList();
			IsPaused = true;
			Time.timeScale = 0;
			menuManager = FindObjectOfType<MenuManager>();
			if (menuManager == null)
			{
				Debug.LogError("No menuManager in scene");
			}
		}

		public static List<PlayerController> Players => Instance.players;
		public static float Depth => Instance.yPosition;

		void Start()
		{
			shaderPropID = worldMaterial.shader.GetPropertyNameId(worldMaterial.shader.FindPropertyIndex("_Position"));

			StartGame();
		}

		void Update()
		{
            if (IsPaused)
            {
                return;
            }
            ScrollWorld(Time.deltaTime);
			currentlyPlayingDepthMusic = GameDepthMusic.SetDepthMusic(currentlyPlayingDepthMusic,-yPosition);
			CollisionSystemUtil.UpdateCollisions();
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
            IsPaused = false;
            OnGameUnPause?.Invoke();
			GameDepthMusic.gameDepthMusic[currentlyPlayingDepthMusic].music.Play2D();
        }
		void PauseGame()
		{
            OnGamePause?.Invoke();
			GameDepthMusic.gameDepthMusic[currentlyPlayingDepthMusic].music.Stop2D();
			IsPaused = true;
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
			menuManager.ShowGameOver(playerWin.gameObject.name);
		}
	}
	
}