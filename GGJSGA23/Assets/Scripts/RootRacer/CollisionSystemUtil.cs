using System.Collections.Generic;
using System.Linq;
using RootRacer.Behaviours;
using UnityEngine;

namespace RootRacer
{
	public static class CollisionSystemUtil
	{
		private static readonly List<BaseSpawnedItemBehaviour> registeredItems = new();
		private static readonly List<CircleCollider2D> registeredPlayers = new();
		private static readonly Dictionary<CircleCollider2D, List<BaseSpawnedItemBehaviour>> collisions = new();

		public static bool IsDirty { get; private set; }

		public static void RegisterItem(BaseSpawnedItemBehaviour item)
		{
			registeredItems.Add(item);
			IsDirty = true;

			Debug.Log($"Registered item {item.name}");
		}

		public static void UnregisterItem(BaseSpawnedItemBehaviour item)
		{
			registeredItems.Remove(item);
			IsDirty = true;
			
			Debug.Log($"Un-registered item {item.name}");
		}

		public static void RegisterPlayer(CircleCollider2D player)
		{
			registeredPlayers.Add(player);
			collisions.Add(player, new List<BaseSpawnedItemBehaviour>());
			IsDirty = true;
			
			Debug.Log($"Registered player: {player.name}");
		}

		public static void UnregisterPlayer(PlayerController player)
		{
			var playerCollider = player.CircleCollider2D;
			registeredPlayers.Remove(playerCollider);
			collisions.Remove(playerCollider);
			IsDirty = true;
			
			Debug.Log($"Un-registered player: {player.name}");
		}

		public static bool UpdateCollisionsIfDirty()
		{
			if (!IsDirty) return false;

			UpdateCollisions();
			return true;
		}

		public static void UpdateCollisions()
		{
			Debug.Log("Collision system update.");
			foreach (var player in registeredPlayers)
			{
				var currentCollisions = registeredItems
					.Where(item =>
						item.gameObject.activeSelf &&
						item.enabled &&
						item.GetIsTouching(player))
					.ToList();

				collisions[player] = currentCollisions;
			}

			IsDirty = false;
		}

		public static List<BaseSpawnedItemBehaviour> GetTouchedItems(CircleCollider2D player)
		{
			UpdateCollisionsIfDirty();
			return collisions[player];
		}

		public static bool IsGoodItemSpawnLocation(Vector2 spawnPosition, float radius)
		{
			UpdateCollisionsIfDirty();
			return registeredItems.All(x => !x.GetIsTouching(spawnPosition, radius));
		}
	}
}