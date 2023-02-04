using UnityEngine;

namespace RootRacer
{
	[RequireComponent(typeof(CircleCollider2D))]
	public class SpawnedItemBehaviour : MonoBehaviour
	{
		private CircleCollider2D circleCollider2D;

		public float Radius => circleCollider2D.radius;
		
		private void Awake()
		{
			circleCollider2D = GetComponent<CircleCollider2D>();
		}

		private void OnDestroy()
		{
			ItemSpawnerBehaviour.UnsubscribeSpawnedItem(this);
		}

		public bool GetIsInsideRadius(Vector2 point) => circleCollider2D.OverlapPoint(point);
	}
}