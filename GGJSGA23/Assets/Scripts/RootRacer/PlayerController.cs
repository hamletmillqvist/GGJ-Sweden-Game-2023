using UnityEngine;

namespace RootRacer
{
	/* Todo: This behaviour is growing to become quite complex.
	 * Let's try refactor it if more methods are added.
	 * This is to prevent the GOD-CLASS code smell!
	 */

	[RequireComponent(typeof(CircleCollider2D))]
	public class PlayerController : MonoBehaviour
	{
		public KeyCode moveLeft;
		public KeyCode moveRight;
		public Color playerColor;
		public float horizontalMoveSpeed;
		public float downSpeed;
		public float boostReduceAmount;
		public float baseEatAnimationSpeed = 3;
		public float minDistanceForLineUpdate = 0.1f;
		public int linePositions = 50;

		[Header("Player Effects")] public float invertTime = 5;
		public float shieldTime = 5;

		private Animator headAnimator;
		private new Camera camera;
		private Vector2 screenSize;
		private GameManager gameManager;
		private float invertTimer;
		private float shieldTimer;
		private bool invertControls;
		private bool hasShield;
		private Vector3 startPosition;
		private LineRenderer lineRenderer;
		private CircleCollider2D circleCollider2D;

		#region UnityEvents

		private void Awake()
		{
			startPosition = transform.position;

			camera = FindObjectOfType<Camera>();
			gameManager = FindObjectOfType<GameManager>();
			headAnimator = GetComponentInChildren<Animator>();
			lineRenderer = GetComponentInChildren<LineRenderer>();

			GetComponentInChildren<SpriteRenderer>().material.color = playerColor;

			circleCollider2D = GetComponent<CircleCollider2D>();
			CollisionSystemUtil.RegisterPlayer(circleCollider2D);
		}

		void Start()
		{
			downSpeed = gameManager.GetTargetSpeed();
			lineRenderer.positionCount = linePositions;
			lineRenderer.material.SetColor("_PlayerColor", playerColor);
			ResetPlayer();
		}

		void Update()
		{
			if (gameManager.isPaused)
			{
				return;
			}

			var deltaTime = Time.deltaTime;

			EffectTimers(deltaTime);

			var downSpeed = gameManager.GetTargetSpeed();
			var aMulti = (downSpeed + baseEatAnimationSpeed) / baseEatAnimationSpeed;
			headAnimator.SetFloat("AnimationMultiplier", aMulti);

			ControlHorizontalPosition(deltaTime);
			ControlVerticalPosition(deltaTime);

			UpdateLine(deltaTime);

			NormalizeDownSpeed(deltaTime);

			HandleTouchedItems();

			if (GetIsOutsideOfScreen())
			{
				gameManager.GameOver(this);
				Destroy(gameObject);
			}
		}
		private void EffectTimers(float deltaTime)
		{
            if (invertControls)
            {
                invertTimer -= deltaTime;
                if (invertTimer <= 0)
                {
                    invertControls = false;
                }
            }
			if (hasShield)
			{
				shieldTimer -= deltaTime;
				if (shieldTimer <= 0)
				{
					hasShield = false;
				}
			}
        }

		private void OnDestroy()
		{
			CollisionSystemUtil.UnregisterPlayer(circleCollider2D);
		}

		#endregion

		private void HandleTouchedItems()
		{
			var touchedItems = CollisionSystemUtil.GetTouchedItems(circleCollider2D);
			foreach (var touchedItem in touchedItems)
			{
				touchedItem.TriggerEffect(this);
			}
		}

		public void ResetPlayer()
		{
			transform.position = startPosition;
			downSpeed = gameManager.GetTargetSpeed();
			var pos = transform.position;
			for (var i = 0; i < lineRenderer.positionCount; i++)
			{
				lineRenderer.SetPosition(i, pos);
			}
			hasShield = false;
			invertControls = false;
		}

		[ContextMenu("Stun Player")]
		public void StunPlayer()
		{
			if (hasShield)
			{
				hasShield = false;
				return;
			}
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
		public void Shield()
		{
			shieldTimer = shieldTime;
			hasShield = true;
		}

		private void NormalizeDownSpeed(float deltaTime)
		{
			var targetSpeed = gameManager.GetTargetSpeed();

			if (downSpeed == targetSpeed)
			{
				return;
			}

			// I think there's a clamp like method that should move towards the target value linearly?
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

		private void UpdateLine(float deltaTime)
		{
			for (var i = 0; i < lineRenderer.positionCount; i++)
			{
				lineRenderer.SetPosition(i,
					lineRenderer.GetPosition(i) + new Vector3(0, gameManager.GetTargetSpeed() * 100 * deltaTime, 0));
			}

			var lastPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
			if (Vector3.Distance(lastPoint, transform.position) < minDistanceForLineUpdate)
			{
				return;
			}

			for (var i = 0; i < lineRenderer.positionCount - 1; i++)
			{
				lineRenderer.SetPosition(i, lineRenderer.GetPosition(i + 1));
			}

			lineRenderer.SetPosition((lineRenderer.positionCount - 1), transform.position);
		}

		private void ControlVerticalPosition(float deltaTime)
		{
			var deltaY = downSpeed - gameManager.GetTargetSpeed();

			if (deltaY == 0)
			{
				return;
			}

			transform.position += new Vector3(0, deltaY * deltaTime, 0);
		}

		private void ControlHorizontalPosition(float deltaTime)
		{
			float movementX = 0;
			if (Input.GetKey(moveLeft))
			{
				movementX -= invertControls ? -1 : 1;
			}

			if (Input.GetKey(moveRight))
			{
				movementX += invertControls ? -1 : 1;
			}

			UpdateHorizontalPosition(deltaTime * movementX);
		}

		private void UpdateHorizontalPosition(float movementDirectionDelta)
		{
			var position = transform.position;
			position += new Vector3(horizontalMoveSpeed * movementDirectionDelta, 0, 0);

			var screenPoint = camera.WorldToScreenPoint(position);
			screenPoint.x = Mathf.Clamp(screenPoint.x, 0, Screen.width);

			transform.position = camera.ScreenToWorldPoint(screenPoint);
		}

		private bool GetIsOutsideOfScreen()
		{
			var screenPoint = camera.WorldToScreenPoint(transform.position);
			return screenPoint.y > Screen.height;
		}
	}
}