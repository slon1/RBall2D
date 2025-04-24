using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour {
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpForce = 5f;

	[SerializeField] private float airControlMultiplier = 0.5f;
	[SerializeField] private float coyoteTime = 0.1f;
	
	
	private float coyoteCounter;	

	private Rigidbody2D rb;
	private CircleCollider2D coll;
	private IGroundChecker groundChecker;
	private BallSquashAnimator ballSquashAnimator;
	private float horizontalInput;
	private float realRadius;
	
	void Awake() {
		rb = GetComponent<Rigidbody2D>();		
		coll = GetComponent<CircleCollider2D>();
		realRadius = coll.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
		groundChecker = Installer.GetService<IGroundChecker>();
		groundChecker.CheckRadius(realRadius);

		
	}

	void Update() {
		horizontalInput = Input.GetAxisRaw("Horizontal");

		// coyote time
		if (groundChecker.IsGrounded(rb.position))
			coyoteCounter = coyoteTime;
		else
			coyoteCounter -= Time.deltaTime;

		
		if (Input.GetButtonDown("Jump")&& coyoteCounter > 0) { 
			Jump();		
		}
	}

	void FixedUpdate() {
		Move();

	}

	private void Move() {
		Vector2 velocity = rb.velocity;
		velocity.x = horizontalInput * moveSpeed;
		rb.velocity = velocity;
	}

	private void Jump() {		
		rb.velocity = new Vector2(rb.velocity.x, 0);
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
	}
}
