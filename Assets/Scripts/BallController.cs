using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour {
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpForce = 5f;

	[SerializeField] private float airControlMultiplier = 0.5f;
	[SerializeField] private float coyoteTime = 0.1f;
	[SerializeField] private float moveSmoothing = 10f;

	private float coyoteCounter;

	private Rigidbody2D rb;
	private CircleCollider2D coll;
	private IGroundChecker groundChecker;	
	private float horizontalInput;
	private float realRadius;
	

	void Awake() {
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<CircleCollider2D>();
		realRadius = coll.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
		groundChecker = Installer.GetService<IGroundChecker>();
		groundChecker.CheckRadius(realRadius);
		groundChecker.SetOffset(0.01f);
		groundChecker.SetSlopeThreshold(0.7f); //45 grad
		


	}

	void Update() {
		horizontalInput = Input.GetAxisRaw("Horizontal");

		
		if (groundChecker.IsGrounded(rb.position, out bool canJump)) {
			coyoteCounter = coyoteTime;
			if (!canJump) {
				coyoteCounter = 0; 
			}
		}
		else {
			coyoteCounter -= Time.deltaTime;
		}

		
		if (Input.GetButtonDown("Jump") && coyoteCounter > 0) {
			Jump();
		}

		
		if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) {
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
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
		float controlMultiplier = groundChecker.IsGrounded(rb.position) ? 1f : airControlMultiplier;
		Vector2 velocity = rb.velocity;
		velocity.x = Mathf.Lerp(velocity.x, targetSpeed * controlMultiplier, Time.fixedDeltaTime * moveSmoothing);
		rb.velocity = velocity;
	}



}
