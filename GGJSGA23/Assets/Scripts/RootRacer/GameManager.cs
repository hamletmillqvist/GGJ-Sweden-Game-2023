using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RootRacer
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager instance;
		[SerializeField] private float startSpeed = 0.05f;
		[SerializeField] private float speedIncrease = 0.1f;

		public static Camera MainCamera;

		public TextMeshProUGUI depthTM;
		public MeshRenderer worldMeshRenderer;
		public bool isPaused;

		private Material worldMaterial;
		private float yPosition;
		private float currentSpeed = 0.5f;
		private int shaderPropID;
		private List<PlayerController> players;
		private void Awake()
		{
			instance = this;
			MainCamera = FindObjectOfType<Camera>();
			worldMaterial = worldMeshRenderer.material;
			players = FindObjectsOfType<PlayerController>().ToList();
		}
		public static List<PlayerController> Players => instance.players;

		void Start()
		{
			
			shaderPropID = worldMaterial.shader.GetPropertyNameId(worldMaterial.shader.FindPropertyIndex("_Position"));
			StartGame();
		}

		void Update()
		{
			ScrollWorld(Time.deltaTime);
			UpdateDepthText(-yPosition);
			CollisionSystemUtil.UpdateCollisions();
		}

		public float GetTargetSpeed()
		{
			return currentSpeed;
		}

		private void UpdateDepthText(float yPosition)
		{
			depthTM.text = yPosition.ToString("0.#m");
		}

		private void ScrollWorld(float deltaTime)
		{
			if (isPaused)
			{
				return;
			}

			yPosition -= deltaTime * currentSpeed;
			currentSpeed += speedIncrease * deltaTime;
			worldMaterial.SetVector(shaderPropID, new Vector2(0, yPosition));
		}

		public void StartGame()
		{
			ResetGame();
			isPaused = false;
		}

		public void ResetGame()
		{
			Time.timeScale = 1;
			currentSpeed = startSpeed;
			yPosition = 0;

			var players = FindObjectsOfType<PlayerController>();
			foreach (var player in players)
			{
				player.ResetPlayer();
			}
		}

		public void GameOver(PlayerController playerFail)
		{
			isPaused = true;
			Time.timeScale = 0;
		}
	}
}