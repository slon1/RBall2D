using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour {
	[Header("Movement Settings")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float airControlMultiplier = 0.5f;
	[SerializeField] private float moveSmoothing = 10f;

	[Header("Jump Settings")]
	[SerializeField] private float jumpForce = 9f;
	[SerializeField] private float coyoteTime = 0.25f;

	private float coyoteCounter;
	private float horizontalInput;
	private Vector2 startPosition;
	private float cameraLowerBound;

	private Rigidbody2D rb;
	private IGroundChecker groundChecker;
	private UserInput input;


	void Awake() {
		InitializeComponents();
		InitializeCameraBounds();
		startPosition = transform.position;
	}

	private void InitializeComponents() {
		rb = GetComponent<Rigidbody2D>();
		groundChecker = Installer.GetService<IGroundChecker>();
		input = Installer.GetService<UserInput>();

		if (groundChecker == null) {
			Debug.LogError("GroundChecker not found in DI container!");
			return;
		}
		if (input == null) {
			Debug.LogError("UserInput not found in DI container!");
			return;
		}

		var coll = GetComponent<CircleCollider2D>();
		float realRadius = coll.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
		groundChecker.CheckRadius(realRadius);
		groundChecker.SetOffset(0.01f);
		groundChecker.SetSlopeThreshold(0.7f);

		input.OnLeft += OnLeftInput;
		input.OnRight += OnRightInput;
		input.OnJump += OnJumpInput;
	}

	private void InitializeCameraBounds() {
		Camera mainCamera = Camera.main;
		float orthoSize = mainCamera.orthographicSize;
		float cameraY = mainCamera.transform.position.y;
		var coll = GetComponent<CircleCollider2D>();
		cameraLowerBound = cameraY - orthoSize - coll.radius;
	}


	private void OnLeftInput(bool pressed) {
		horizontalInput = pressed ? -1f : 0f;
	}

	private void OnRightInput(bool pressed) {
		horizontalInput = pressed ? 1f : 0f;
	}

	private void OnJumpInput(bool pressed) {
		if (pressed && coyoteCounter > 0) {
			Jump();
		}
		else if (!pressed && rb.velocity.y > 0) {
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
		}
	}


	private void Restart() {
		transform.position = startPosition;
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0f;
	}

	void Update() {
		UpdateCoyoteTime();
		CheckFall();
	}


	private void UpdateCoyoteTime() {
		if (groundChecker.IsGrounded(rb.position, out bool canJump)) {
			coyoteCounter = coyoteTime;
			if (!canJump) {
				coyoteCounter = 0;
			}
		}
		else {
			coyoteCounter -= Time.deltaTime;
		}
	}

	private void CheckFall() {
		if (transform.position.y < cameraLowerBound) {
			Restart();
		}
	}

	private void Jump() {
		rb.velocity = new Vector2(rb.velocity.x, 0);
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

	}

	void FixedUpdate() {
		Move();

	}
	private void Move() {
		float targetSpeed = horizontalInput * moveSpeed;
		float controlMultiplier = groundChecker.IsGrounded(rb.position, out _) ? 1f : airControlMultiplier;
		Vector2 velocity = rb.velocity;
		velocity.x = Mathf.Lerp(velocity.x, targetSpeed * controlMultiplier, Time.fixedDeltaTime * moveSmoothing);
		rb.velocity = velocity;

	}

	private void OnDestroy() {
		if (input != null) {
			input.OnLeft -= OnLeftInput;
			input.OnRight -= OnRightInput;
			input.OnJump -= OnJumpInput;
		}
		rb = null;
	}
}
