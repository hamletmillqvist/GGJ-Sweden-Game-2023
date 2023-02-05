using Sonity;
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
		public KeyCode MoveLeft { get; set; }
		public KeyCode MoveRight { get; set; }
		public Color PlayerColor { get; set; }
		public float HorizontalMoveSpeed { get; set; }
		public float DownSpeed { get; set; }
		public float BoostReduceAmount { get; set; }
		public float BaseEatAnimationSpeed { get; set; } = 3;
		public float MinDistanceForLineUpdate { get; set; } = 0.1f;
		public int LinePositions { get; set; } = 50;
		public CircleCollider2D CircleCollider2D { get; private set; }

		[Header("Player Effects")] public float invertTime = 5;
		public float ShieldTime { get; set; } = 5;
		public float SizeMultiplier { get; set; } = 2;
		public float SizeTime { get; set; } = 5;
		public bool HasGodMode { get; set; } = false;

		[Header("Sounds")] [SerializeField] private SoundEvent footstepsSoundEvent;
		[SerializeField] private SoundEvent deathSoundEvent;

		private Animator headAnimator;
		private new Camera camera;
		private Vector2 screenSize;
		private GameManager gameManager;
		private float invertTimer;
		private float shieldTimer;
		private float sizeTimer;
		private Vector3 baseSizeScale;
		private bool invertControls;
		private bool hasShield;
		private bool hasSizeUp;
		private Vector3 startPosition;
		private LineRenderer lineRenderer;
		
		private void Awake()
		{
			startPosition = transform.position;
			baseSizeScale = transform.localScale;
			camera = FindObjectOfType<Camera>();
			gameManager = FindObjectOfType<GameManager>();
			headAnimator = GetComponentInChildren<Animator>();
			lineRenderer = GetComponentInChildren<LineRenderer>();

			//GetComponentInChildren<SpriteRenderer>().material.color = playerColor;

			CircleCollider2D = GetComponent<CircleCollider2D>();
			CollisionSystemUtil.RegisterPlayer(CircleCollider2D);
			gameManager.OnGamePause += OnPause;
			gameManager.OnGameUnPause += OnUnPause;
		}

		void Start()
		{
			DownSpeed = gameManager.GetTargetSpeed();
			lineRenderer.positionCount = LinePositions;
			lineRenderer.material.SetColor("_PlayerColor", PlayerColor);
			ResetPlayer();
		}

		void OnPause()
		{
			footstepsSoundEvent.Stop(transform);
		}

		void OnUnPause()
		{
			footstepsSoundEvent.Play(transform);
		}

		void Update()
		{
			if (gameManager.IsPaused)
			{
				return;
			}

			var deltaTime = Time.deltaTime;

			EffectTimers(deltaTime);

			var downSpeed = gameManager.GetTargetSpeed();
			var aMulti = (downSpeed + BaseEatAnimationSpeed) / BaseEatAnimationSpeed;
			headAnimator.SetFloat("AnimationMultiplier", aMulti);

			HandleHorizontalMovement(deltaTime);
			HandleVerticalMovement(deltaTime);

			UpdateLine(deltaTime);

			NormalizeDownSpeed(deltaTime);

			HandleTouchedItems();

			if (GetIsOutsideOfScreen())
			{
				deathSoundEvent?.Play(gameManager.transform);
				footstepsSoundEvent.Stop(transform);
				GameManager.RemovePlayer(this);
			}
		}

		private void OnDestroy()
		{
			
			gameManager.OnGamePause -= OnPause;
			gameManager.OnGameUnPause -= OnUnPause;
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

			if (hasSizeUp)
			{
				sizeTimer -= deltaTime;
				if (sizeTimer <= 0)
				{
					hasSizeUp = false;
					transform.localScale = baseSizeScale;
				}
			}
		}

		private void HandleTouchedItems()
		{
			var touchedItems = CollisionSystemUtil.GetTouchedItems(CircleCollider2D);
			foreach (var touchedItem in touchedItems)
			{
				touchedItem.TriggerEffect(this);
			}
		}

		public void ResetPlayer()
		{
			transform.position = startPosition;
			DownSpeed = gameManager.GetTargetSpeed();
			var pos = transform.position;
			for (var i = 0; i < lineRenderer.positionCount; i++)
			{
				lineRenderer.SetPosition(i, pos);
			}

			hasShield = false;
			invertControls = false;
			hasSizeUp = false;
			transform.localScale = baseSizeScale;
		}

		[ContextMenu("Stun Player")]
		public void StunPlayer()
		{
			if (hasShield || HasGodMode)
			{
				hasShield = false;
				return;
			}

			DownSpeed = gameManager.GetTargetSpeed() * 100;
		}

		[ContextMenu("Speed Player")]
		public void SpeedUp(float amount)
		{
			DownSpeed -= amount;
		}

		[ContextMenu("Invert Controls")]
		public void InvertControls()
		{
			if (HasGodMode)
			{
				return;
			}

			invertTimer = invertTime;
			invertControls = true;
		}

		public void Shield()
		{
			shieldTimer = ShieldTime;
			hasShield = true;
		}

		public void SizeUp()
		{
			if (HasGodMode)
			{
				return;
			}

			transform.localScale = baseSizeScale * SizeMultiplier;
			hasSizeUp = true;
			sizeTimer = SizeTime;
		}

		private void NormalizeDownSpeed(float deltaTime)
		{
			var targetSpeed = gameManager.GetTargetSpeed();

			if (DownSpeed == targetSpeed)
			{
				return;
			}

			DownSpeed = Mathf.MoveTowards(DownSpeed, targetSpeed, BoostReduceAmount * deltaTime);
		}

		private void UpdateLine(float deltaTime)
		{
			for (var i = 0; i < lineRenderer.positionCount; i++)
			{
				lineRenderer.SetPosition(i,
					lineRenderer.GetPosition(i) + new Vector3(0, gameManager.GetTargetSpeed() * 100 * deltaTime, 0));
			}

			var lastPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
			if (Vector3.Distance(lastPoint, transform.position) < MinDistanceForLineUpdate)
			{
				return;
			}

			for (var i = 0; i < lineRenderer.positionCount - 1; i++)
			{
				lineRenderer.SetPosition(i, lineRenderer.GetPosition(i + 1));
			}

			lineRenderer.SetPosition((lineRenderer.positionCount - 1), transform.position);
		}

		private void HandleVerticalMovement(float deltaTime)
		{
			var deltaY = DownSpeed - gameManager.GetTargetSpeed();

			if (deltaY == 0)
			{
				return;
			}

			transform.position += new Vector3(0, deltaY * deltaTime, 0);
		}

		private void HandleHorizontalMovement(float deltaTime)
		{
			float movementX = 0;
			if (Input.GetKey(MoveLeft))
			{
				movementX -= invertControls ? -1 : 1;
			}

			if (Input.GetKey(MoveRight))
			{
				movementX += invertControls ? -1 : 1;
			}

			UpdateHorizontalPosition(deltaTime * movementX);
		}

		private void UpdateHorizontalPosition(float movementDirectionDelta)
		{
			var position = transform.position;
			position += new Vector3(HorizontalMoveSpeed * movementDirectionDelta, 0, 0);
			
			var minScreenBounds = camera.ScreenToWorldPoint(Vector3.zero);
			var maxScreenBounds = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
			var radius = CircleCollider2D.radius * transform.localScale.x;
			position.x = Mathf.Clamp(position.x, minScreenBounds.x + radius, maxScreenBounds.x - radius);
			
			transform.position = position;
		}

		private bool GetIsOutsideOfScreen()
		{
			var screenPoint = camera.WorldToScreenPoint(transform.position);
			return screenPoint.y > Screen.height;
		}
	}
}