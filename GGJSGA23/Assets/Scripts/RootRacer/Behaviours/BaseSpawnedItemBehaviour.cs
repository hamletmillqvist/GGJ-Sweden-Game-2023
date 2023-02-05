using Sonity;
using UnityEngine;

namespace RootRacer.Behaviours
{
	[RequireComponent(typeof(CircleCollider2D), typeof(SpriteRenderer))]	
	public abstract class BaseSpawnedItemBehaviour : MonoBehaviour
	{
		[SerializeField] private Sprite[] sprites;
		[SerializeField] private SoundEvent triggerSound;

		private CircleCollider2D circleCollider2D;

		public float Radius => circleCollider2D.radius;

		private void Awake()
		{
			circleCollider2D = GetComponent<CircleCollider2D>();
			CollisionSystemUtil.RegisterItem(this);

			var spriteRenderer = GetComponent<SpriteRenderer>();
			if (sprites == null || sprites.Length == 0)
			{
				return;
			}

			var index = Random.Range(0, sprites.Length - 1);
			spriteRenderer.sprite = sprites[index];
		}

		private void OnDestroy()
		{
			CollisionSystemUtil.UnregisterItem(this);
		}

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
		{ 
			triggerSound?.Play(GameManager.Instance.transform);
		}
	}
}