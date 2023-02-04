using System.Collections.Generic;
using System.Linq;
using RootRacer.Exceptions;
using RootRacer.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace RootRacer.Behaviours
{
	public class ItemSpawnerBehaviour : MonoBehaviour
	{
		[SerializeField] private float spawnerTimerMax;
		[SerializeField] private float spawnerTimerMin;
		[SerializeField] private float spawnerTimerCountdown;
		[SerializeField] private BaseSpawnedItemBehaviour spawnerPrefab;

		private CameraBorderResult cameraBorder;
		private GameManager gameManager;

		private void Awake()
		{
			Assert.IsNotNull(spawnerPrefab);

			var mainCamera = Camera.main;
			Assert.IsNotNull(mainCamera);

			cameraBorder = OrthographicCameraPositionUtil.GetCameraCorners(mainCamera);
			gameManager = FindObjectOfType<GameManager>();
		}

		private void Update()
		{
			var deltaTime = Time.deltaTime;
			var targetSpeed = gameManager.GetTargetSpeed();

			UpdateTimer(deltaTime, targetSpeed);

			if (spawnerTimerCountdown <= 0)
			{
				SpawnItem();
			}
		}

		private void UpdateTimer(float deltaTime, float targetSpeed)
		{
			spawnerTimerCountdown -= deltaTime * targetSpeed;
		}

		private void SpawnItem()
		{
			var spawnPosition = GetValidSpawnPosition();
			Instantiate(spawnerPrefab, spawnPosition, Quaternion.identity);
			ResetSpawnTimer();
		}

		private void ResetSpawnTimer()
		{
			var nextSpawnTime = Random.Range(spawnerTimerMin, spawnerTimerMax);
			spawnerTimerCountdown += nextSpawnTime;
		}

		private Vector2 GetValidSpawnPosition()
		{
			const int MAX_ATTEMPTS = 100;

			for (var attempts = 0; attempts < MAX_ATTEMPTS; attempts++)
			{
				var spawnPosition = RandomizeSpawningPosition();
				if (spawnPosition != null)
				{
					return spawnPosition.Value;
				}
			}

			throw new InfiniteLoopException();
		}

		private Vector2? RandomizeSpawningPosition()
		{
			var radius = spawnerPrefab.GetComponent<CircleCollider2D>().radius;
			var spawnPosition = new Vector2(
				x: Random.Range(cameraBorder.Left, cameraBorder.Right),
				y: cameraBorder.Bottom - radius);


			var hasGoodSpawnPosition = CollisionSystemUtil.IsGoodItemSpawnLocation(spawnPosition, radius);
			if (!hasGoodSpawnPosition)
			{
				return null;
			}

			return spawnPosition;
		}
	}
}