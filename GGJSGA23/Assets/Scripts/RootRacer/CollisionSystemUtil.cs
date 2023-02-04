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

		private static bool isDirty;
		
		public static void RegisterItem(BaseSpawnedItemBehaviour item)
		{
			registeredItems.Add(item);
			isDirty = true;
		}

		public static void UnregisterItem(BaseSpawnedItemBehaviour item)
		{
			registeredItems.Remove(item);
			isDirty = true;
		}

		public static void RegisterPlayer(CircleCollider2D player) 
		{
			registeredPlayers.Add(player);
			collisions.Add(player, new List<BaseSpawnedItemBehaviour>());
			isDirty = true;
		}

		public static void UnregisterPlayer(CircleCollider2D player)
		{
			registeredPlayers.Remove(player);
			collisions.Remove(player);
			isDirty = true;
		}

		public static void UpdateCollisions()
		{
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

			isDirty = false;
		}

		public static List<BaseSpawnedItemBehaviour> GetItemsTouched(CircleCollider2D player)
		{
			if (isDirty)
			{
				UpdateCollisions();
			}
		
			return collisions[player];
		}

		public static bool IsGoodItemSpawnLocation(Vector2 spawnPosition, float radius)
		{
			if (isDirty)
			{
				UpdateCollisions();
			}
			
			return registeredItems.All(x => !x.GetIsTouching(spawnPosition, radius));
		}
	}
}