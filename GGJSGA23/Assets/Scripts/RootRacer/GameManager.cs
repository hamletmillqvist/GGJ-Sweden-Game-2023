using TMPro;
using UnityEngine;

namespace RootRacer
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private float startSpeed = 0.05f;
		[SerializeField] private float speedIncrease = 0.1f;

		public static Camera MainCamera;

		public TextMeshProUGUI depthTM;
		public MeshRenderer worldMeshRenderer;
		public bool IsPaused;

		private Material worldMaterial;
		private float ypos = 0;
		private float currentSpeed = 0.5f;
		private int shaderPropID;

		private void Awake()
		{
			MainCamera = FindObjectOfType<Camera>();;
			worldMaterial = worldMeshRenderer.material;
		}

		void Start()
		{
			shaderPropID = worldMaterial.shader.GetPropertyNameId(worldMaterial.shader.FindPropertyIndex("_Position"));
			StartGame();
		}

		void Update()
		{
			CollisionSystemUtil.UpdateCollisions();
			ScrollWorld(Time.deltaTime);
			UpdateDepthText(-ypos);
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
			if (IsPaused) return;

			ypos -= deltaTime * currentSpeed;
			currentSpeed += speedIncrease * deltaTime;
			worldMaterial.SetVector(shaderPropID, new Vector2(0, ypos));
		}

		public void StartGame()
		{
			ResetGame();
			IsPaused = false;
		}

		public void ResetGame()
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

		public void GameOver(PlayerController playerFail)
		{
			IsPaused = true;
			Time.timeScale = 0;
		}
	}
}