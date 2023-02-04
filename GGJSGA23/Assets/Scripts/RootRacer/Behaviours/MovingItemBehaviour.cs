using RootRacer.Utils;
using UnityEngine;

namespace RootRacer.Behaviours
{
	[RequireComponent(typeof(CircleCollider2D))]
	public class MovingItemBehaviour : MonoBehaviour
	{
		[SerializeField] private Vector3 movementPerSecond;
		[SerializeField] private float gameSpeedMultiplier = 1;

		private GameManager gameManager;
		private CircleCollider2D circleCollider2D;
		private CameraBorderResult cameraBorder;

		public float BottomPositionY => transform.position.y - circleCollider2D.radius;

		private void Awake()
		{
			gameManager = FindObjectOfType<GameManager>();
			circleCollider2D = GetComponent<CircleCollider2D>();
			cameraBorder = OrthographicCameraPositionUtil.GetCameraCorners(GameManager.MainCamera);
		}

		private void Update()
		{
			var deltaTime = Time.deltaTime;

			var deltaGameSpeed = Vector3.up * (deltaTime * gameManager.GetTargetSpeed() * gameSpeedMultiplier);
			var deltaMovement = movementPerSecond * deltaTime;
			var totalMovement = deltaMovement + deltaGameSpeed;

			transform.position += totalMovement;

			if (BottomPositionY > cameraBorder.Top)
			{
				Destroy(gameObject);
			}
		}
	}
}