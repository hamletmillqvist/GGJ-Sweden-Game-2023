using UnityEngine;

namespace RootRacer
{
	[RequireComponent(typeof(CircleCollider2D))]
	public class PlayerController : MonoBehaviour
	{
		[Header("Powerups etc")] public float invertTime = 5;
		
		public KeyCode moveLeft;
		public KeyCode moveRight;
		public Color playerColor;
		public float horizontalMoveSpeed;
		public float downSpeed = 0;
		public float boostReduceAmount;
		public float baseEatAnimationSpeed = 3;

		private Animator headAnimator;
		private new Camera camera;
		private Vector2 screenSize;
		private GameManager gameManager;
		private float invertTimer = 0;
		private bool invertControls = false;
		private Vector3 startPosition;
		private CircleCollider2D circleCollider2D;

		private void Awake()
		{
			camera = FindObjectOfType<Camera>();
			gameManager = FindObjectOfType<GameManager>();
			headAnimator = GetComponentInChildren<Animator>();
			circleCollider2D = GetComponent<CircleCollider2D>();
			
			GetComponentInChildren<SpriteRenderer>().material.color = playerColor;
			
			CollisionSystemUtil.RegisterPlayer(circleCollider2D);
		}

		void Start()
		{
			downSpeed = gameManager.GetTargetSpeed();
			startPosition = transform.position;
		}

		public void ResetPlayer()
		{
			transform.position = startPosition;
			downSpeed = gameManager.GetTargetSpeed();
		}

		//public void SetDownSpeed(float speed)
		//{
		//    this.downSpeed = speed;
		//}

		[ContextMenu("Stun Player")]
		public void StunPlayer()
		{
			downSpeed = gameManager.GetTargetSpeed() * 100;
		}

		[ContextMenu("Speed Player")]
		public void SpeedUp(float amount)
		{
			downSpeed -= amount;
		}

		[ContextMenu("Invert Controls")]
		public void InvertControls()
		{
			invertTimer = invertTime;
			invertControls = true;
		}

		private void NormalizeDownSpeed(float deltaTime)
		{
			float targetSpeed = gameManager.GetTargetSpeed();
			if (downSpeed == targetSpeed) return;
			if (downSpeed < targetSpeed)
			{
				downSpeed += boostReduceAmount * deltaTime;
				if (downSpeed > targetSpeed)
				{
					downSpeed = targetSpeed;
				}
			}
			else if (downSpeed > targetSpeed)
			{
				downSpeed -= boostReduceAmount * deltaTime;
				if (downSpeed < targetSpeed)
				{
					downSpeed = targetSpeed;
				}
			}
		}

		void Update()
		{
			if (gameManager.IsPaused) return;
			float deltaTime = Time.deltaTime;
			if (invertControls)
			{
				invertTimer -= deltaTime;
				if (invertTimer <= 0)
				{
					invertControls = false;
				}
			}

			float downSpeed = gameManager.GetTargetSpeed();
			float aMulti = (downSpeed + baseEatAnimationSpeed) / baseEatAnimationSpeed;
			headAnimator.SetFloat("AnimationMultiplier", aMulti);
			ControlHorizontalPosition(deltaTime);
			float deltaY = ControlVerticalPosition(deltaTime);

			NormalizeDownSpeed(deltaTime);
			HandleTouching(deltaTime);
			if (IsOutsideOfScreen())
			{
				gameManager.GameOver(this);
				Destroy(gameObject, 0.0001f);
			}
		}

		private void OnDestroy()
		{
			CollisionSystemUtil.UnregisterPlayer(circleCollider2D);
		}

		private float ControlVerticalPosition(float deltaTime)
		{
			float deltaY = downSpeed - gameManager.GetTargetSpeed();
			if (deltaY == 0) return 0;
			transform.position += new Vector3(0, deltaY * deltaTime, 0);
			return deltaY;
		}

		private void HandleTouching(float deltaTime)
		{
			var touchedItems = CollisionSystemUtil.GetItemsTouched(circleCollider2D);
			foreach (var touchedItem in touchedItems)
			{
				touchedItem.TriggerEffect(this);
			}
		}

		private void ControlHorizontalPosition(float deltaTime)
		{
			float xMove = 0;
			if (Input.GetKey(moveLeft))
			{
				xMove -= invertControls ? -1 : 1;
			}

			if (Input.GetKey(moveRight))
			{
				xMove += invertControls ? -1 : 1;
			}

			transform.position += new Vector3(horizontalMoveSpeed * xMove * deltaTime, 0, 0);
			Vector3 screenPoint = camera.WorldToScreenPoint(transform.position);

			screenPoint.x = Mathf.Clamp(screenPoint.x, 0, Screen.width);
			transform.position = camera.ScreenToWorldPoint(screenPoint);
		}

		private bool IsOutsideOfScreen()
		{
			Vector3 screenPoint = camera.WorldToScreenPoint(transform.position);
			return screenPoint.y > Screen.height;
		}
	}
}