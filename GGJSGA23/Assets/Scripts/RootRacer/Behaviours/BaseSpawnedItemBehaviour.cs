using UnityEngine;

namespace RootRacer.Behaviours
{
	[RequireComponent(typeof(CircleCollider2D))]
	public abstract class BaseSpawnedItemBehaviour : MonoBehaviour
	{
		private CircleCollider2D circleCollider2D;

		public float Radius => circleCollider2D.radius;

		private void Awake()
		{
			circleCollider2D = GetComponent<CircleCollider2D>();
			CollisionSystemUtil.RegisterItem(this);
		}

		private void OnDestroy()
		{
			CollisionSystemUtil.UnregisterItem(this);
		}

		public bool GetIsPointInsideRadius(Vector2 point) => circleCollider2D.OverlapPoint(point);

		public bool GetIsTouching(CircleCollider2D otherCollider)
		{
			return GetIsTouching(otherCollider.transform.position, otherCollider.radius);
		}

		public bool GetIsTouching(Vector2 position, float radius)
		{
			var distance = Vector3.Distance(transform.position, position);
			return distance < Radius + radius;
		}

		public virtual void TriggerEffect(PlayerController playerController)
		{ }
	}
}